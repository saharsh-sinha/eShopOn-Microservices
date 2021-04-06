using Microsoft.Extensions.Hosting;
using System;

namespace eShopOn.Basket.PSI.RabbitMQ
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<TimedWorker>();
                });
    }
}