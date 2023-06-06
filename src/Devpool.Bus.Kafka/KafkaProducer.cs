using System.Text.Json;
using Confluent.Kafka;
using Devpool.Bus.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Devpool.Bus.Kafka;

public class KafkaProducer: IBus 
{
    private readonly ProducerConfig _config;
    private readonly ILogger<KafkaProducer> _logger;
    private readonly IProducer<string, string> _producer;

    public KafkaProducer(IOptions<KafkaOptions> options, ILogger<KafkaProducer> logger)
    {
        _config = options.Value.ProducerConfig;
        _logger = logger;
        _producer = new ProducerBuilder<string, string>(_config).Build();
    }

    public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken) where TEvent : IEvent
    {
        var topic = TopicNameCreator.Create(typeof(TEvent));

        var message = new Message<string, string>
        {
            Key = Guid.NewGuid().ToString(),
            Value = JsonSerializer.Serialize(@event)
        };

        try
        {
            var deliveryResult = await _producer.ProduceAsync(topic, message, cancellationToken);
            _logger.LogInformation("Produced message to topic {Topic} [Partition: {Partition}, Offset: {Offset}]",
                deliveryResult.Topic,
                deliveryResult.Partition,
                deliveryResult.Offset);
            _producer.Flush(cancellationToken);
        }
        catch (ProduceException<string, string> ex)
        {
            _logger.LogError(ex, "Failed to produce message to topic {Topic}", topic);
            throw;
        }
    }
}