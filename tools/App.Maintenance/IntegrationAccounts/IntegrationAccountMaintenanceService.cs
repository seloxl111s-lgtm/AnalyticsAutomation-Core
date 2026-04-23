using BuildingBlocks.Infrastructure.Persistence;
using BuildingBlocks.Infrastructure.Persistence.Entities.Auth;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace App.Maintenance.IntegrationAccounts;

public sealed class IntegrationAccountMaintenanceService(
    PlatformDbContext dbContext,
    IPasswordHasher<AuthUser> passwordHasher)
{
    public const string PasswordEnvironmentVariableName = "AA_INTEGRATION_ACCOUNT_PASSWORD";
    public const string IntegrationAccountLogin = "integration-web-android";
    public const string IntegrationAccountDisplayName = "Web/Android Integration Account";
    public const string PlatformOwnerRoleCode = "platform_owner";
    public const string RootGroupNodeCode = "root";

    public async Task<IntegrationAccountUpsertResult> UpsertAsync(CancellationToken cancellationToken)
    {
        var password = ReadRequiredPassword();
        var role = await dbContext.AuthRoles
            .SingleOrDefaultAsync(
                item => item.Code == PlatformOwnerRoleCode,
                cancellationToken)
            ?? throw new InvalidOperationException(
                $"Role '{PlatformOwnerRoleCode}' was not found. Aborting without changes.");

        var rootGroupNode = await dbContext.GroupNodes
            .SingleOrDefaultAsync(
                item => item.Code == RootGroupNodeCode,
                cancellationToken)
            ?? throw new InvalidOperationException(
                $"Group node '{RootGroupNodeCode}' was not found. Aborting without changes.");

        var normalizedLogin = NormalizeLogin(IntegrationAccountLogin);
        var user = await dbContext.AuthUsers
            .Include(item => item.UserRoles)
            .SingleOrDefaultAsync(
                item => item.NormalizedLogin == normalizedLogin,
                cancellationToken);

        var wasCreated = user is null;
        if (user is null)
        {
            user = new AuthUser
            {
                Id = Guid.NewGuid(),
                CreatedAtUtc = DateTimeOffset.UtcNow
            };

            dbContext.AuthUsers.Add(user);
        }

        user.Login = IntegrationAccountLogin;
        user.NormalizedLogin = normalizedLogin;
        user.DisplayName = IntegrationAccountDisplayName;
        user.IsActive = true;
        user.CurrentGroupNodeId = rootGroupNode.Id;
        user.PasswordHash = passwordHasher.HashPassword(user, password);

        if (!user.UserRoles.Any(item => item.RoleId == role.Id))
        {
            user.UserRoles.Add(new AuthUserRole
            {
                UserId = user.Id,
                RoleId = role.Id
            });
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return new IntegrationAccountUpsertResult(
            user.Login,
            wasCreated,
            role.Code,
            rootGroupNode.Code);
    }

    private static string ReadRequiredPassword()
    {
        var password = Environment.GetEnvironmentVariable(PasswordEnvironmentVariableName);
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new InvalidOperationException(
                $"Environment variable {PasswordEnvironmentVariableName} is required. The tool does not prompt for a password.");
        }

        return password;
    }

    private static string NormalizeLogin(string login)
    {
        return login.Trim().ToUpperInvariant();
    }
}

public sealed record IntegrationAccountUpsertResult(
    string Login,
    bool WasCreated,
    string AssignedRoleCode,
    string AssignedGroupNodeCode);