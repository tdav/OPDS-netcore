using System.Text.Json.Serialization;
using DotOPDS.Database.Services;
using DotOPDS.ImportBook.Service;
using DotOPDS.Server.Extensions;
using DotOPDS.Server.Services;
using DotOPDS.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using Serilog;

namespace DotOPDS.Server
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
            services
                .AddHttpContextAccessor()
                .AddLocalization()
                .AddControllersWithViews(ConfigureMvcOptions)
                .AddJsonOptions(options => options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);

            SharedServices.ConfigureServices(services, Configuration);

            services.AddMyDatabaseService(Configuration);
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<BookParsersPool>();
            services.AddSingleton<ConverterService>();
            services.AddSingleton<FileUtils>();
            services.AddSingleton<MimeHelper>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add(HeaderNames.Server, "DotOPDS");
                await next.Invoke();
            });

            var supportedCultures = new[] { "en", "ru" };
            var localizationOptions = new RequestLocalizationOptions()
                .SetDefaultCulture(supportedCultures[0])
                .AddSupportedCultures(supportedCultures)
                .AddSupportedUICultures(supportedCultures);

            app.UseRequestLocalization(localizationOptions);

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            app.UseSerilogRequestLogging();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });

            app.UpdateMigrateDatabase();
        }

        private void ConfigureMvcOptions(MvcOptions options)
        {
            options.OutputFormatters.Insert(0, new AtomXmlMediaTypeFormatter());
            options.RespectBrowserAcceptHeader = true;
        }
    }
}
