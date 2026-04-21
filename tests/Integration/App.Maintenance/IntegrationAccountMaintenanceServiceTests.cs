using App.Maintenance.IntegrationAccounts;

using BuildingBlocks.Infrastructure.Persistence;
using BuildingBlocks.Infrastructure.Persistence.Entities.Auth;
using BuildingBlocks.Infrastructure.Persistence.Entities.GroupTree;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace App.Maintenance.Tests;

public sealed class IntegrationAccountMaintenanceServiceTests
{
    [Fact]
    public async Task UpsertAsyncCreatesIntegrationAccountWithRootGroupAndPlatformOwnerRole()
    {
        const string password = "S2-11-create-password!";
        const string normalizedLogin = "INTEGRATION-WEB-ANDROID";

        using var passwordScope = new EnvironmentVariableScope(
            IntegrationAccountMaintenanceService.PasswordEnvironmentVariableName,
            password);

        using var dbContext = CreateDbContext();
        var role = new AuthRole
        {
            Id = Guid.NewGuid(),
            Code = IntegrationAccountMaintenanceService.PlatformOwnerRoleCode,
            Name = "Platform Owner"
        };
        var rootNode = new GroupNode
        {
            Id = Guid.NewGuid(),
            Code = IntegrationAccountMaintenanceService.RootGroupNodeCode,
            Name = "Root",
            Depth = 0,
            IsActive = true
        };

        dbContext.AuthRoles.Add(role);
        dbContext.GroupNodes.Add(rootNode);
        await dbContext.SaveChangesAsync();

        var passwordHasher = new PasswordHasher<AuthUser>();
        var service = new IntegrationAccountMaintenanceService(dbContext, passwordHasher);

        var result = await service.UpsertAsync(CancellationToken.None);

        dbContext.ChangeTracker.Clear();

        var user = await dbContext.AuthUsers
            .Include(item => item.UserRoles)
            .SingleAsync(item => item.NormalizedLogin == normalizedLogin);

        Assert.True(result.WasCreated);
        Assert.Equal(IntegrationAccountMaintenanceService.IntegrationAccountLogin, result.Login);
        Assert.Equal(IntegrationAccountMaintenanceService.PlatformOwnerRoleCode, result.AssignedRoleCode);
        Assert.Equal(IntegrationAccountMaintenanceService.RootGroupNodeCode, result.AssignedGroupNodeCode);
        Assert.Equal(IntegrationAccountMaintenanceService.IntegrationAccountDisplayName, user.DisplayName);
        Assert.True(user.IsActive);
        Assert.Equal(rootNode.Id, user.CurrentGroupNodeId);
        Assert.Single(user.UserRoles);
        Assert.Equal(role.Id, user.UserRoles.Single().RoleId);
        Assert.NotEqual(
            PasswordVerificationResult.Failed,
            passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password));
    }

    [Fact]
    public async Task UpsertAsyncUpdatesExistingIntegrationAccountAndAddsMissingRoleLink()
    {
        const string password = "S2-11-updated-password!";
        const string normalizedLogin = "INTEGRATION-WEB-ANDROID";

        using var passwordScope = new EnvironmentVariableScope(
            IntegrationAccountMaintenanceService.PasswordEnvironmentVariableName,
            password);

        using var dbContext = CreateDbContext();
        var role = new AuthRole
        {
            Id = Guid.NewGuid(),
            Code = IntegrationAccountMaintenanceService.PlatformOwnerRoleCode,
            Name = "Platform Owner"
        };
        var rootNode = new GroupNode
        {
            Id = Guid.NewGuid(),
            Code = IntegrationAccountMaintenanceService.RootGroupNodeCode,
            Name = "Root",
            Depth = 0,
            IsActive = true
        };
        var branchNode = new GroupNode
        {
            Id = Guid.NewGuid(),
            ParentNodeId = rootNode.Id,
            Code = "branch-a",
            Name = "Branch A",
            Depth = 1,
            IsActive = true
        };
        var user = new AuthUser
        {
            Id = Guid.NewGuid(),
            Login = IntegrationAccountMaintenanceService.IntegrationAccountLogin,
            NormalizedLogin = normalizedLogin,
            DisplayName = "Old Name",
            PasswordHash = "old-hash",
            IsActive = false,
            CurrentGroupNodeId = branchNode.Id,
            CreatedAtUtc = DateTimeOffset.UtcNow.AddDays(-1)
        };

        dbContext.AuthRoles.Add(role);
        dbContext.GroupNodes.AddRange(rootNode, branchNode);
        dbContext.AuthUsers.Add(user);
        await dbContext.SaveChangesAsync();

        var passwordHasher = new PasswordHasher<AuthUser>();
        var service = new IntegrationAccountMaintenanceService(dbContext, passwordHasher);

        var result = await service.UpsertAsync(CancellationToken.None);

        dbContext.ChangeTracker.Clear();

        var updatedUser = await dbContext.AuthUsers
            .Include(item => item.UserRoles)
            .SingleAsync(item => item.Id == user.Id);

        Assert.False(result.WasCreated);
        Assert.Equal(IntegrationAccountMaintenanceService.IntegrationAccountDisplayName, updatedUser.DisplayName);
        Assert.True(updatedUser.IsActive);
        Assert.Equal(rootNode.Id, updatedUser.CurrentGroupNodeId);
        Assert.Single(updatedUser.UserRoles);
        Assert.Equal(role.Id, updatedUser.UserRoles.Single().RoleId);
        Assert.NotEqual(
            PasswordVerificationResult.Failed,
            passwordHasher.VerifyHashedPassword(updatedUser, updatedUser.PasswordHash, password));
    }

    [Fact]
    public async Task UpsertAsyncThrowsWhenPasswordEnvironmentVariableIsMissing()
    {
        using var passwordScope = new EnvironmentVariableScope(
            IntegrationAccountMaintenanceService.PasswordEnvironmentVariableName,
            null);

        using var dbContext = CreateDbContext();
        var service = new IntegrationAccountMaintenanceService(
            dbContext,
            new PasswordHasher<AuthUser>());

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => service.UpsertAsync(CancellationToken.None));

        Assert.Equal(
            $"Environment variable {IntegrationAccountMaintenanceService.PasswordEnvironmentVariableName} is required. The tool does not prompt for a password.",
            exception.Message);
        Assert.False(await dbContext.AuthUsers.AnyAsync());
    }

    private static PlatformDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<PlatformDbContext>()
            .UseInMemoryDatabase($"app-maintenance-tests-{Guid.NewGuid():N}")
            .Options;

        return new PlatformDbContext(options);
    }

    private sealed class EnvironmentVariableScope : IDisposable
    {
        private readonly string name;
        private readonly string? originalValue;

        public EnvironmentVariableScope(string name, string? value)
        {
            this.name = name;
            originalValue = Environment.GetEnvironmentVariable(name);
            Environment.SetEnvironmentVariable(name, value);
        }

        public void Dispose()
        {
            Environment.SetEnvironmentVariable(name, originalValue);
        }
    }
}