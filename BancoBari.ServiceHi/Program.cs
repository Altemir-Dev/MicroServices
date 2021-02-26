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
        static void Main(string[] args)
        {
            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.ReceiveEndpoint("helloworld-queue", e =>
                {
                    e.Consumer(() => new HelloConsumer(Infra.ServiceProvider.GetServiceProvider().GetRequiredService<IPublishEndpoint>(),
                                                       Infra.ServiceProvider.GetServiceProvider().GetRequiredService<IOptions<MicroserviceOptions>>()));
                });
            });
            busControl.Start();

            Console.ReadLine();
        }
    }
}
