using System;
using System.Linq;
using System.Reflection.Metadata;
using Api.Extensions;
using Api.Filters;
using AutoMapper;
using Core;
using Core.Helpers;
using Core.Mapping;
using Core.Options;
using Core.Services;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Sieve.Models;
using Sieve.Services;


namespace Api
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
            var coreAssembly = typeof(MappingProfile).Assembly;
            
            services.AddControllers(options => { options.Filters.Add<ExceptionFilter>(); })
                .AddFluentValidation(options => options.RegisterValidatorsFromAssembly(coreAssembly));

            services.AddAutoMapper(coreAssembly);
            services.AddNpgsql(Configuration);
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "My API", Version = "v1"}); });

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAdService, AdService>();
            services.AddScoped<ISieveProcessor, MappingSieveProcessor>();
            services.AddSingleton<IImageHelper, ImageHelper>();

            services.Configure<SieveOptions>(Configuration.GetSection("Sieve"));
            services.Configure<UserOptions>(Configuration.GetSection(nameof(UserOptions)));
            services.Configure<ImageOptions>(Configuration.GetSection(nameof(ImageOptions)));
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