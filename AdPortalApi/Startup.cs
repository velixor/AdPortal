using AdPortalApi.Configurations;
using AdPortalApi.Extensions;
using AdPortalApi.Filters;
using AdPortalApi.Mapping;
using AdPortalApi.Services;
using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Sieve.Models;
using Sieve.Services;


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
            services.AddControllers(options => { options.Filters.Add<ExceptionFilter>(); })
                .AddFluentValidation(options => options.RegisterValidatorsFromAssemblyContaining<Startup>());

            services.AddNpgsql(Configuration);

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAdService, AdService>();

            services.AddScoped<ISieveProcessor, ApplicationSieveProcessor>();

            services.AddSingleton<IImageService, ImageService>();

            services.AddAutoMapper(typeof(Startup));
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "My API", Version = "v1"}); });

            services.Configure<SieveOptions>(Configuration.GetSection("Sieve"));
            services.Configure<UserConfigs>(Configuration.GetSection(nameof(UserConfigs)));
            services.Configure<ImageConfigs>(Configuration.GetSection(nameof(ImageConfigs)));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); });

            app.UseStaticFiles();
        }
    }
}