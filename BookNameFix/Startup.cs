using System;
using DotOPDS.Database.Services;
using DotOPDS.ImportBook.Service;
using DotOPDS.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace BookNameFix
{
    class Startup
    {
        public static ServiceProvider serviceProvider;

        private static void RegisterServices()
        {
            IConfiguration conf = new ConfigurationBuilder()
                                           .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                                           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                           .Build();

            var serilogLogger = new LoggerConfiguration()
                .ReadFrom.Configuration(conf)
                .CreateLogger();

            var services = new ServiceCollection();

            SharedServices.ConfigureServices(services, conf);

            services.AddMyDatabaseService(conf);
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<BookParsersPool>();
            services.AddSingleton<ConverterService>();
            services.AddSingleton<FileUtils>();

            services.AddSingleton(conf);
            services.AddMemoryCache();
            services.AddProgramService(conf);
            services.AddLogging(builder =>
            {
                builder.SetMinimumLevel(LogLevel.Information);
                builder.AddSerilog(logger: serilogLogger, dispose: true);
            });

            serviceProvider = services.BuildServiceProvider(true);
        }

        private static void DisposeServices()
        {
            if (serviceProvider == null)
            {
                return;
            }
            if (serviceProvider is IDisposable)
            {
                serviceProvider.Dispose();
            }
        }

        static void Main(string[] args)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            RegisterServices();
            IServiceScope scope = serviceProvider.CreateScope();

            scope.ServiceProvider.GetRequiredService<Program>().Run();

            DisposeServices();
        }
    }
}
