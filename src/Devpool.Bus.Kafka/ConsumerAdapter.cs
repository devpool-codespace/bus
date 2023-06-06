using Devpool.Bus.Abstractions;

namespace Devpool.Bus.Kafka;

public class ConsumerAdapter<T> : IConsumer<IEvent> where T : IEvent
{
    private readonly IConsumer<T> _inner;

    public ConsumerAdapter(IConsumer<T> inner)
    {
        _inner = inner;
    }

    public Task Consume(CancellationToken cancellationToken = default)
    {
        return _inner.Consume(cancellationToken);
    }

    public void Close()
    {
       _inner.Close();
    }
}