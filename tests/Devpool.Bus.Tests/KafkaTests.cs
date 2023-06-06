using Devpool.Bus.Abstractions;
using Devpool.Bus.Kafka;
using Devpool.Bus.Tests.KafkaTestData;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Devpool.Bus.Tests;

public class KafkaTests
{
    [Fact]
    public void AddKafka_TestEventHandlerRegisteredInDI()
    {
        // Arrange
        var services = new ServiceCollection();
        
        var configuration = new ConfigurationBuilder().Build();
        
        var testEventEventHandlerType = typeof(IEventHandler<TestEvent>);
        
        services.AddKafka(configuration, (busBuilder) => { });

        // Act
        var provider = services.BuildServiceProvider();
        var eventHandler = provider.GetService(testEventEventHandlerType);
        
        // Assert
        Assert.NotNull(eventHandler);

    }
}