using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenFM_API_Crawler_Service.Factories;
using OpenFM_API_Crawler_Service.HostedServices;
using OpenFM_API_Crawler_Service.Repositories;
using Serilog;
using Serilog.Core;
using Serilog.Formatting.Json;
using System;
using System.IO;
using System.Reflection;

namespace OpenFM_API_Crawler_Service
{
    class Program
    {
        static void Main(string[] args)
        {
            ConfigureSerilog();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((services) => ConfigureServices(services))
                .UseSerilog();
        }

        public static IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddHostedService<CrawlerService>();
            services.AddSingleton<ILogger>((services) => ConfigureSerilog());
            services.AddTransient<HttpClientFactory>();
            services.AddTransient<ApiRepository>();
            services.AddTransient<ILocalRepository, LocalRepository>();

            return services.BuildServiceProvider();
        }

        private static Logger ConfigureSerilog()
        {
            var appPath = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;
            var logPath = Path.Combine(appPath,"Logs","log");

             return new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File(
                    formatter: new JsonFormatter(),
                    path: logPath,
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 15,
                    encoding: System.Text.Encoding.UTF8)
                .CreateLogger();
        }
    }
}
