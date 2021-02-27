using BancoBari.Infra.Settings;
using BancoBari.ServiceHi.Consumers;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace BancoBari.ServiceHi
{
    class Program
    {
        private static IServiceProvider serviceProvider;
        static void Main(string[] args)
        {
            serviceProvider = Infra.ServiceProvider.GetServiceProvider();
            var rabbitMQSettings = serviceProvider.GetService<IOptions<RabbitMQSettings>>().Value;
            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host(rabbitMQSettings.Host);
                cfg.ReceiveEndpoint("helloworld-queue", e =>
                {
                    e.Consumer(() => new HelloConsumer(serviceProvider.GetRequiredService<IPublishEndpoint>(),
                                                       serviceProvider.GetRequiredService<IOptions<MicroserviceOptions>>()));
                });
            });
            busControl.Start();

            Console.ReadLine();
        }
    }
}
