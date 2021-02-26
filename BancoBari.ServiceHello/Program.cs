using BancoBari.Infra.Settings;
using BancoBari.ServiceHello.Consumers;
using BancoBari.ServiceHello.Producers;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace BancoBari.ServiceHello
{
    class Program
    {
        static void Main(string[] args)
        {
            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.ReceiveEndpoint("hey-queue", e =>
                {
                    e.Consumer(() => new HiConsumer(Infra.ServiceProvider.GetServiceProvider().GetRequiredService<IPublishEndpoint>(),
                                                       Infra.ServiceProvider.GetServiceProvider().GetRequiredService<IOptions<MicroserviceOptions>>()));
                });
            });
            busControl.Start();

            Task.Run(() => SendMessageHello());

            Console.ReadLine();
        }

        private static void SendMessageHello()
        {
            var serviceProvider = Infra.ServiceProvider.GetServiceProvider();
            var publishEndpoint = serviceProvider.GetRequiredService<IPublishEndpoint>();
            var options = serviceProvider.GetRequiredService<IOptions<MicroserviceOptions>>();
            var helloWorldProducer = new HelloWorldProducer(publishEndpoint, options);

            while (true)
            {
                helloWorldProducer.PublishMessage();
                Task.Delay(5000).Wait();
            }
        }
    }
}
