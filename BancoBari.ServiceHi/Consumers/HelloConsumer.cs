using System;
using System.Threading.Tasks;
using BancoBari.Infra.Settings;
using MassTransit;
using Microsoft.Extensions.Options;
using static BancoBari.Contracts.Contracts;

namespace BancoBari.ServiceHi.Consumers
{
    public class HelloConsumer : IConsumer<HelloWorld>
    {
        private MicroserviceOptions microserviceOptions;
        public IPublishEndpoint publishEndpoint;
        public HelloConsumer(IPublishEndpoint publishEndpoint, IOptions<MicroserviceOptions> microserviceSettings)
        {
            this.publishEndpoint = publishEndpoint;
            microserviceOptions = microserviceSettings.Value;
        }
        public async Task Consume(ConsumeContext<HelloWorld> context)
        {
            var message = context.Message;
            Console.WriteLine($"Received Message From: {message.Identifier}");
            Console.WriteLine($"Id: {message.Id}");
            Console.WriteLine($"Message: {message.Message}");
            Console.WriteLine($"Date: {message.DateRequest.LocalDateTime}");
            Console.WriteLine("----------------------------------------------");            

            await publishEndpoint.Publish(new Hi(Guid.NewGuid(), microserviceOptions.Id, "Hi", DateTimeOffset.Now));
            await Task.FromResult(message);
        }
    }
}