using BancoBari.Infra.Settings;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BancoBari.Infra.MassTransit
{
    public static class Extensions
    {
        public static IServiceCollection AddMassTransitWithRabbitMq(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<MicroserviceOptions>().Bind(configuration.GetSection(MicroserviceOptions.SectionName));
            services.AddOptions<RabbitMQSettings>().Bind(configuration.GetSection(RabbitMQSettings.SectionName));
            services.AddMassTransit(configure =>
            {                
                configure.UsingRabbitMq((context, configurator) =>
                {
                    var rabbitMQSettings = configuration.GetSection(RabbitMQSettings.SectionName).Get<RabbitMQSettings>();
                    configurator.Host(rabbitMQSettings.Host);                    
                });
            });
            services.AddMassTransitHostedService();

            return services;
        }

    }
}