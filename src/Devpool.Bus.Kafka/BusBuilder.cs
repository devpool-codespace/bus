using Devpool.Bus.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Devpool.Bus.Kafka;

public class BusBuilder
{
    private readonly IServiceCollection _services;

    public BusBuilder(IServiceCollection services)
    {
        _services = services;
    }

    public void AddConsumer<T>() where T : IEvent
    {
        _services.AddSingleton<IConsumer<T>, KafkaConsumer<T>>();
        _services.AddSingleton<IConsumer<IEvent>, ConsumerAdapter<T>>(sp => 
            new ConsumerAdapter<T>(sp.GetRequiredService<IConsumer<T>>()));
    }
}