using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace App.Maintenance.Configuration;

internal static class AppApiConfigurationLoader
{
    private const string AppSettingsDirectoryEnvironmentVariableName = "AA_APP_API_SETTINGS_DIR";

    public static IConfigurationRoot Load()
    {
        var appApiSettingsDirectory = ResolveAppApiSettingsDirectory();
        var environmentName = ResolveEnvironmentName();

        return new ConfigurationBuilder()
            .SetBasePath(appApiSettingsDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
            .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: false)
            .AddEnvironmentVariables()
            .Build();
    }

    private static string ResolveAppApiSettingsDirectory()
    {
        var configuredDirectory = Environment.GetEnvironmentVariable(AppSettingsDirectoryEnvironmentVariableName);
        if (!string.IsNullOrWhiteSpace(configuredDirectory))
        {
            var fullPath = Path.GetFullPath(configuredDirectory);
            EnsureAppSettingsJsonExists(fullPath);
            return fullPath;
        }

        var fromCurrentDirectory = Path.Combine(Directory.GetCurrentDirectory(), "src", "App.Api");
        if (File.Exists(Path.Combine(fromCurrentDirectory, "appsettings.json")))
        {
            return fromCurrentDirectory;
        }

        DirectoryInfo? probeDirectory = new(AppContext.BaseDirectory);
        while (probeDirectory is not null)
        {
            var candidateDirectory = Path.Combine(probeDirectory.FullName, "src", "App.Api");
            if (File.Exists(Path.Combine(candidateDirectory, "appsettings.json")))
            {
                return candidateDirectory;
            }

            probeDirectory = probeDirectory.Parent;
        }

        throw new InvalidOperationException(
            $"Could not locate App.Api appsettings.json. Set {AppSettingsDirectoryEnvironmentVariableName} to the directory that contains it.");
    }

    private static string ResolveEnvironmentName()
    {
        return Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")
            ?? Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
            ?? Environments.Production;
    }

    private static void EnsureAppSettingsJsonExists(string directoryPath)
    {
        if (!File.Exists(Path.Combine(directoryPath, "appsettings.json")))
        {
            throw new InvalidOperationException(
                $"App.Api appsettings.json was not found in {directoryPath}.");
        }
    }
}