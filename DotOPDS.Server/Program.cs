using System;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace DotOPDS.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

                Log.Logger = new LoggerConfiguration()
                    .Enrich.FromLogContext()
                    .Enrich.WithProperty("Environment", environment)
                    .ReadFrom.Configuration(configuration)
                    .CreateLogger();

                Host.CreateDefaultBuilder(args)
                           .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
                           .ConfigureAppConfiguration(configuration =>
                           {
                               configuration.SetBasePath(AppDomain.CurrentDomain.BaseDirectory);
                               configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                           })
                           .UseSerilog()
                           .Build()
                           .Run();
            }
            catch (System.Exception ex)
            {
                Log.Fatal($"Failed to start {Assembly.GetExecutingAssembly().GetName().Name}", ex);
                throw;
            }
        }
    }
}
