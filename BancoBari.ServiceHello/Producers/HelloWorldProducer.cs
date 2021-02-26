using BancoBari.Infra.Settings;
using MassTransit;
using Microsoft.Extensions.Options;
using System;
using static BancoBari.Contracts.Contracts;

namespace BancoBari.ServiceHello.Producers
{
    public class HelloWorldProducer
    {
        private readonly IPublishEndpoint publishEndpoint;
        public MicroserviceOptions microserviceSettings;
        public HelloWorldProducer(IPublishEndpoint publishEndpoint, IOptions<MicroserviceOptions> microserviceSettings)
        {
            this.microserviceSettings = microserviceSettings.Value;
            this.publishEndpoint = publishEndpoint;
        }

        public void PublishMessage()
        {
            Console.WriteLine("Send message after 5 seconds...");
            publishEndpoint.Publish(new HelloWorld(Guid.NewGuid(), microserviceSettings.Id, "Hello World!", DateTimeOffset.Now));
        }
    }
}