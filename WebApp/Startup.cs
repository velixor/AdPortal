using System.IO;
using AutoMapper;
using Core.Helpers;
using Core.Mapping;
using Core.Options;
using Core.Services;
using Data;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;

namespace WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var coreAssembly = typeof(MappingAutoMapperProfile).Assembly;
            services.AddAutoMapper(coreAssembly);
            
            services.AddRazorPages()
                .AddFluentValidation(options => options.RegisterValidatorsFromAssembly(coreAssembly));

            var connectionString = Configuration.GetConnectionString("LocalPgSql");
            services.AddDbContext<AdPortalContext>(options =>
                options.UseNpgsql(connectionString));
            
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAdService, AdService>();
            services.AddScoped<ISieveProcessor, MappingSieveProcessor>();
            services.AddScoped<IImageHelper, ImageHelper>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.Configure<SieveOptions>(Configuration.GetSection("Sieve"));
            services.Configure<UserOptions>(Configuration.GetSection(nameof(UserOptions)));
            services.Configure<StaticFilesOptions>(Configuration.GetSection(nameof(StaticFilesOptions)));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            IOptions<StaticFilesOptions> staticFilesOptions)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            EnsureDirectoriesExist(staticFilesOptions);
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(),
                    staticFilesOptions.Value.StaticFilesPath))
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapRazorPages(); });
        }

        private static void EnsureDirectoriesExist(IOptions<StaticFilesOptions> fileOpt)
        {
            Directory.CreateDirectory(Path.Combine(fileOpt.Value.StaticFilesPath, fileOpt.Value.ImagePath));
        }
    }
}