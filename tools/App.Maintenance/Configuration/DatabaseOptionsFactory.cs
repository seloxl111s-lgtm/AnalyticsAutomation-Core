using BuildingBlocks.Infrastructure.Persistence;

using Microsoft.Extensions.Configuration;

namespace App.Maintenance.Configuration;

internal static class DatabaseOptionsFactory
{
    public static DatabaseOptions Create(IConfiguration configuration)
    {
        return new DatabaseOptions
        {
            ConnectionString = configuration.GetConnectionString("MainDatabase"),
            Host = configuration["Database:Host"] ?? DatabaseOptions.DefaultHost,
            Port = int.TryParse(configuration["Database:Port"], out var databasePort)
                ? databasePort
                : DatabaseOptions.DefaultPort,
            Database = configuration["Database:Database"] ?? DatabaseOptions.DefaultDatabase,
            Username = configuration["Database:Username"] ?? DatabaseOptions.DefaultUsername,
            PasswordFilePath = configuration["Database:PasswordFilePath"]
        };
    }
}