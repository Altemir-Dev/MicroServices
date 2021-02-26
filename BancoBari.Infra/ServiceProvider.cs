using BancoBari.Infra.MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StructureMap;
using System;

namespace BancoBari.Infra
{
    public static class ServiceProvider
    {
        public static IServiceProvider GetServiceProvider()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Development.json", false, true);

            IConfiguration config = builder.Build();

            var services = new ServiceCollection()
                  .AddOptions()
                  .AddMassTransitWithRabbitMq(config);

            var container = new Container();
            container.Configure(config =>
            {
                config.Scan(_ =>
                {
                   // _.AssemblyContainingType(typeof(Program));
                    _.WithDefaultConventions();
                });
                config.Populate(services);
            });
            return container.GetInstance<IServiceProvider>();
        }
    }
}
