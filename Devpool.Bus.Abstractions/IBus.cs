namespace Devpool.Bus.Abstractions;

public interface IBus
{
    Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : IEvent;
}