using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MinimalApiDemo;

public class Program2
{
    /// <summary>
    /// creates IHostBuilder, used by integration tests
    /// </summary>
    /// <param name="args">command line args</param>
    /// <returns>IHostBuilder</returns>
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, serviceCollection) =>
            {
                
            })
            .ConfigureWebHostDefaults(webHost =>
            {
                webHost.ConfigureKestrel(kestrelOptions =>
                {
                    kestrelOptions.AddServerHeader = false;
                });
                webHost.UseStartup<Startup>();
            })
            .ConfigureAppConfiguration((context, config) =>
            {
            
            });
}