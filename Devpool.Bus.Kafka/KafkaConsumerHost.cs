using Devpool.Bus.Abstractions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Devpool.Bus.Kafka;


public class KafkaConsumerHost : IHostedService
{
    private readonly IEnumerable<IConsumer<IEvent>> _consumers;
    private readonly ILogger<KafkaConsumerHost> _logger;
    private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
    private List<Task> _consumerTasks;


    public KafkaConsumerHost(
        IEnumerable<IConsumer<IEvent>> consumers, 
        ILogger<KafkaConsumerHost> logger)
    {
        _consumers = consumers;
        _logger = logger;
        _consumerTasks = new List<Task>();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting Kafka consumer host");
        
        _consumerTasks = _consumers.Select(consumer =>
            Task.Run(() => consumer.Consume(_cancellationTokenSource.Token), cancellationToken)).ToList();

        _logger.LogInformation("{ConsumerCount} consumers started", _consumerTasks.Count);
        
        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping Kafka consumer host");
        
        // Посылаем отмену всем потокам
        _cancellationTokenSource.Cancel();
        
        try
        {
            // Ждем завершения всех потоков
            await Task.WhenAll(_consumerTasks);
        }
        catch (Exception ex)
        {
            // Если что-то пошло не так, то логируем ошибку
            _logger.LogError(ex, "Error when stopping consumers");
        }
        
        // Закрываем все подключения
        foreach (var consumer in _consumers)
        {
            try
            {
                consumer.Close();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when closing consumer");
            }
        }

        _logger.LogInformation("Stopped Kafka consumer host");
    }
}