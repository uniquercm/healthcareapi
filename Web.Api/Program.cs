﻿using System.IO;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Web.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new HostBuilder()
                            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                            .ConfigureWebHostDefaults(webBuilder =>
                            {
                                webBuilder
                                .UseContentRoot(Directory.GetCurrentDirectory())
                                .UseIISIntegration()
                                .UseUrls("http://0.0.0.0:2003")
                                //.UseUrls("http://0.0.0.0")
                                //.UseUrls("https://api.uniquercm.org")
                                .UseStartup<Startup>();
                            })
                            .Build();
            host.Run();
        }
    }
}
