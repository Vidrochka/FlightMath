using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NLog.Extensions.Hosting;
using NLog.Extensions.Logging;

namespace FlightMath
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(loggerConfiguration =>
                {
#if DEBUG
                    loggerConfiguration.AddDebug();
#endif
                    loggerConfiguration.ClearProviders();
                    loggerConfiguration.SetMinimumLevel(LogLevel.Trace);
                    loggerConfiguration.AddNLog("NLog.config");
                })
                .UseNLog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
