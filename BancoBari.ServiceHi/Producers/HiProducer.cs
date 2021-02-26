using BancoBari.Infra.Settings;
using MassTransit;
using Microsoft.Extensions.Options;
using System;
using static BancoBari.Contracts.Contracts;

namespace BancoBari.ServiceHi.Producers
{

    public class HiProducer
    {
        private readonly IPublishEndpoint publishEndpoint;
        public MicroserviceOptions microserviceOptions;
        public HiProducer(IPublishEndpoint publishEndpoint, IOptions<MicroserviceOptions> microserviceSettings)
        {
            microserviceOptions = microserviceSettings.Value;
            this.publishEndpoint = publishEndpoint;
        }

        public void PublishMessage()
        {
            publishEndpoint.Publish(new Hi(Guid.NewGuid(), microserviceOptions.Id, "Hi", DateTimeOffset.Now));
        }
    }
}
