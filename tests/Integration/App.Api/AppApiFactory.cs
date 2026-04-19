using BuildingBlocks.Infrastructure.Persistence;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace App.Api.IntegrationTests;

public sealed class AppApiFactory : WebApplicationFactory<global::Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureTestServices(static services =>
        {
            services.RemoveAll<DbContextOptions<PlatformDbContext>>();
            services.RemoveAll<PlatformDbContext>();

            services.AddDbContext<PlatformDbContext>(static options =>
                options.UseInMemoryDatabase("AppApiIntegrationTests"));
        });
    }
}