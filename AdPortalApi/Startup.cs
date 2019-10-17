﻿using AdPortalApi.Configurations;
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
            services.AddControllers(options => options.Filters.Add<ValidationFilter>())
                .AddFluentValidation(options => options.RegisterValidatorsFromAssemblyContaining<Startup>());

            services.AddNpgsql(Configuration);

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IAdService, AdService>();

            services.Configure<SieveOptions>(Configuration.GetSection("Sieve"));
            services.AddScoped<ISieveProcessor, ApplicationSieveProcessor>();

            services.AddAutoMapper(typeof(Startup));
            services.AddSwagger();

            services.Configure<UserConfigs>(Configuration.GetSection(nameof(UserConfigs)));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); });
        }
    }
}