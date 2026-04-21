using App.Maintenance.Configuration;
using App.Maintenance.IntegrationAccounts;

using BuildingBlocks.Infrastructure.Persistence;
using BuildingBlocks.Infrastructure.Persistence.Entities.Auth;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

return await AppMaintenanceProgram.RunAsync(args);

internal static class AppMaintenanceProgram
{
    public static async Task<int> RunAsync(string[] args)
    {
        if (!IsIntegrationAccountUpsertCommand(args))
        {
            WriteUsage(Console.Error);
            return 1;
        }

        try
        {
            using IHost host = CreateHost();
            using IServiceScope scope = host.Services.CreateScope();

            var service = scope.ServiceProvider.GetRequiredService<IntegrationAccountMaintenanceService>();
            var result = await service.UpsertAsync(CancellationToken.None);

            Console.Out.WriteLine($"login: {result.Login}");
            Console.Out.WriteLine($"status: {(result.WasCreated ? "created" : "updated")}");
            Console.Out.WriteLine($"role: {result.AssignedRoleCode}");
            Console.Out.WriteLine($"group-node: {result.AssignedGroupNodeCode}");

            return 0;
        }
        catch (InvalidOperationException ex)
        {
            Console.Error.WriteLine(ex.Message);
            return 1;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Maintenance command failed: {ex.Message}");
            return 1;
        }
    }

    private static IHost CreateHost()
    {
        var configuration = AppApiConfigurationLoader.Load();
        var databaseOptions = DatabaseOptionsFactory.Create(configuration);

        var builder = Host.CreateApplicationBuilder();
        builder.Logging.ClearProviders();
        builder.Services.AddPlatformPersistence(databaseOptions);
        builder.Services.AddSingleton<IPasswordHasher<AuthUser>, PasswordHasher<AuthUser>>();
        builder.Services.AddScoped<IntegrationAccountMaintenanceService>();

        return builder.Build();
    }

    private static bool IsIntegrationAccountUpsertCommand(string[] args)
    {
        return args.Length == 2
            && string.Equals(args[0], "integration-account", StringComparison.OrdinalIgnoreCase)
            && string.Equals(args[1], "upsert", StringComparison.OrdinalIgnoreCase);
    }

    private static void WriteUsage(TextWriter writer)
    {
        writer.WriteLine("Usage:");
        writer.WriteLine("  dotnet run --project tools\\App.Maintenance\\App.Maintenance.csproj -- integration-account upsert");
    }
}