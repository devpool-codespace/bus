using Confluent.Kafka;
using Devpool.Bus.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Devpool.Bus.Kafka;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddKafka(
        this IServiceCollection services, 
        IConfiguration configuration, 
        Action<BusBuilder> busBuilder)
    {
        var bus = new BusBuilder(services);
        busBuilder(bus);
        
        services.AddOptions<KafkaOptions>()
            .Bind(configuration.GetSection("Kafka"));

        services.AddSingleton<IBus, KafkaProducer>();

        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        services.Scan(scan => scan
            .FromAssemblies(assemblies)
            .AddClasses(classes => classes.AssignableTo(typeof(IEventHandler<>)))
            .AsImplementedInterfaces()
            .WithTransientLifetime());
        
        services.AddHostedService<KafkaConsumerHost>();

        return services;
    }
}