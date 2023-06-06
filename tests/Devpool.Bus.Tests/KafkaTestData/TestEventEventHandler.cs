using Devpool.Bus.Abstractions;

namespace Devpool.Bus.Tests.KafkaTestData;

public class TestEventEventHandler : IEventHandler<TestEvent>
{
    public Task HandleAsync(TestEvent @event, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}