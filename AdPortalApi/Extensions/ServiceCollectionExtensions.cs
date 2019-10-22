using AdPortalApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace AdPortalApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddNpgsql(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("LocalPgSql");
            services.AddDbContext<AdPortalContext>(options =>
                options.UseNpgsql(connectionString));
        }
    }
}