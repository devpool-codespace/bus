using System.Text.Json;
using Confluent.Kafka;
using Devpool.Bus.Abstractions;
using Microsoft.Extensions.Logging;

namespace Devpool.Bus.Kafka;

public class KafkaConsumer<T> : IConsumer<T> where T : IEvent
{
    private readonly IConsumer<string, string> _consumer;
    private readonly IEventHandler<T> _handler;
    private readonly ILogger<KafkaConsumer<T>> _logger;

    public KafkaConsumer(ConsumerConfig config, IEventHandler<T> handler, ILogger<KafkaConsumer<T>> logger)
    {
        _consumer = new ConsumerBuilder<string, string>(config).Build();
        _handler = handler;
        _logger = logger;
    }

    public async Task Consume(CancellationToken cancellationToken = default)
    {
        _consumer.Subscribe(TopicNameCreator.Create(typeof(T)));
        _logger.LogInformation("Consumer subscribed to topic {Topic}", TopicNameCreator.Create(typeof(T)));

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var consumeResult = _consumer.Consume(cancellationToken);
                var @event = JsonSerializer.Deserialize<T>(consumeResult.Message.Value)!;
                await _handler.HandleAsync(@event, cancellationToken);
                _consumer.Commit(consumeResult);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Consumer operation was canceled");
            }
            catch (ConsumeException ex)
            {
                if (ex.Error.Code != ErrorCode.UnknownTopicOrPart &&
                    ex.Error.Code != ErrorCode.TopicAuthorizationFailed) throw;
                _logger.LogError("Topic became unavailable, stopping consumer. Error: {Error}", ex.Error.Reason);
                break;
            }
            catch(JsonException ex)
            {
                _logger.LogError(ex, "Error occurred while parsing message");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while consuming message");
            }
        }
        
        _logger.LogWarning("Consumer {typeConsumer} stopped", typeof(T));
    }

    public void Close()
    {
        _consumer?.Close();
        _consumer?.Dispose();
    }
}