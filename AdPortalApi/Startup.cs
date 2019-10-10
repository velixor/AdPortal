using AdPortalApi.Configurations;
using AdPortalApi.Data;
using AdPortalApi.Services;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace AdPortalApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            var connectionString = Configuration.GetConnectionString("LocalPgSql");
            services.AddDbContext<AdPortalContext>(options =>
                options.UseNpgsql(connectionString));

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IAdService, AdService>();
            services.AddAutoMapper(typeof(Startup));

            var userConfigs = new UserConfigs();
            Configuration.Bind(nameof(UserConfigs), userConfigs);
            services.AddSingleton(userConfigs);

            services.Configure<UserConfigs>(Configuration.GetSection(nameof(UserConfigs)));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}