using System;
using System.Threading.Tasks;
using BancoBari.Infra.Settings;
using MassTransit;
using Microsoft.Extensions.Options;
using static BancoBari.Contracts.Contracts;

namespace BancoBari.ServiceHello.Consumers
{
    public class HiConsumer : IConsumer<Hi>
    {
        private MicroserviceOptions microserviceSettings;
        public IPublishEndpoint publishEndpoint { get;set; }
        public HiConsumer(IPublishEndpoint publishEndpoint, IOptions<MicroserviceOptions> microserviceSettings)
        {
            this.publishEndpoint = publishEndpoint;
            this.microserviceSettings = microserviceSettings.Value;
        }
        public async Task Consume(ConsumeContext<Hi> context)
        {
            var message = context.Message;
            Console.WriteLine($"Received Message From: {message.Identifier}");
            Console.WriteLine($"Id: {message.Id}");            
            Console.WriteLine($"Text: {message.Message}");
            Console.WriteLine($"Date: {message.DateRequest.LocalDateTime}");
            Console.WriteLine("----------------------------------------------");            
            
            await publishEndpoint.Publish(new HelloWorld(Guid.NewGuid(), microserviceSettings.Id, "Hello World", DateTimeOffset.Now));            
            await Task.FromResult(message);
        }
    }
}