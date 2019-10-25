using System.IO;
using Api.Filters;
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
            var coreAssembly = typeof(MappingAutoMapperProfile).Assembly;

            services.AddControllers(options => { options.Filters.Add<ExceptionFilter>(); })
                .AddFluentValidation(options => options.RegisterValidatorsFromAssembly(coreAssembly));

            services.AddAutoMapper(coreAssembly);

            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "My API", Version = "v1"}); });

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

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            IOptions<StaticFilesOptions> staticFilesOptions)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); });

            EnsureDirectoriesExist(staticFilesOptions);
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(staticFilesOptions.Value.StaticFilesPath)
            });
        }

        private static void EnsureDirectoriesExist(IOptions<StaticFilesOptions> fileOpt)
        {
            Directory.CreateDirectory(Path.Combine(fileOpt.Value.StaticFilesPath, fileOpt.Value.ImagePath));
        }
    }
}