using BancoBari.Infra.Settings;
using BancoBari.ServiceHello.Consumers;
using BancoBari.ServiceHello.Producers;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace BancoBari.ServiceHello
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
                cfg.ReceiveEndpoint("hi-queue", e =>
                {
                    e.Consumer(() => new HiConsumer(serviceProvider.GetRequiredService<IPublishEndpoint>(),
                                                    serviceProvider.GetRequiredService<IOptions<MicroserviceOptions>>()));
                });
            });
            busControl.Start();

            Task.Run(() => SendMessageHello());
            Console.ReadLine();
        }

        private static void SendMessageHello()
        {
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
