# Devpool Bus

Create event

`MyTestEvent.cs`
````csharp

public class MyTestEvent:IEvent
{
    public string Name { get; set; }
}
````

Create event handler

`MyTestEventHandler.cs`
````csharp
public class MyTestEventHandler:IEventHanlder<MyTestEvent>
{
    public void Handle(MyTestEvent @event)
    {
        //handle event
        Console.WriteLine(@event.Name);
    }
}
````

Add configure consumer and producer in appsettings.json

`appsettings.json`
````json
{
  "Kafka": {
    "ProducerConfig": {
      "BootstrapServers": "YOUR_BOOTSTRAP_SERVERS",
      "SaslMechanism": "ScramSha512",
      "SecurityProtocol": "SaslPlaintext",
      "SaslUsername": "YOUR_USERNAME",
      "SaslPassword": "YOUR_PASSWORD",
      "ClientId": "your-client-name"
    },
    "ConsumerConfig": {
      "BootstrapServers": "YOUR_BOOTSTRAP_SERVERS",
      "SaslMechanism": "ScramSha512",
      "SecurityProtocol": "SaslPlaintext",
      "SaslUsername": "YOUR_USERNAME",
      "SaslPassword": "YOUR_PASSWORD",
      "GroupId": "your-group-name"
    }
  }
}
````

Add kafka consumer and producer (IBus) to service collection

`MyTestEvent` - `my-test-event` topic name

`TestTopic` - `test-topic` topic name

````csharp
var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((host,services) =>
    {
        services.AddKafka(host.Configuration,  busBuilder =>
        {
            // Add subscribers for events
            busBuilder.AddConsumer<MyTestEvent>(); //subscribe to my-test-event topic
            busBuilder.AddConsumer<TestTopic>(); //subscribe to test-topic topic
        });
    })
    .Build();

host.Run();
````

 ## ConsumerConfig

| Field                             | Description                                                        |
|----------------------------------|--------------------------------------------------------------------|
| ConsumeResultFields              | List of fields that can be set in `ConsumeResult` objects |
| AutoOffsetReset                  | Action to take when there is no initial offset in offset store |
| GroupId                          | The consumer group id |
| GroupInstanceId                  | Enabling static group membership identifier |
| PartitionAssignmentStrategy      | The name of one or more partition assignment strategies |
| SessionTimeoutMs                 | Consumer group session and failure detection timeout |
| HeartbeatIntervalMs              | The expected time between heartbeats to the consumer group coordinator |
| GroupProtocolType                | Group protocol type |
| CoordinatorQueryIntervalMs       | How often to query for the current consumer group coordinator |
| MaxPollIntervalMs                | Maximum allowed time between calls to consume messages |
| EnableAutoCommit                 | If true the consumer's offset will be periodically committed in the background |
| AutoCommitIntervalMs             | The frequency in milliseconds that the consumer offsets are auto-committed to Kafka |
| EnableAutoOffsetStore            | Automatically store the offset of the last message provided to the application |
| QueuedMinMessages                | Minimum number of messages per topic+partition in the local consumer queue |
| QueuedMaxMessagesKbytes          | Maximum number of kilobytes of messages per topic+partition in the local consumer queue |
| FetchWaitMaxMs                   | Maximum time the broker may wait to fill the response with fetch.min.bytes |
| MaxPartitionFetchBytes           | Maximum amount of data per topic+partition the broker will return for a fetch request |
| FetchMaxBytes                    | Maximum amount of data the broker shall return for a fetch request |
| FetchMinBytes                    | Minimum amount of data the broker shall return for a fetch request |
| FetchErrorBackoffMs              | How long to back off when a fetch request has failed |
| IsolationLevel                   | How to read messages written transactionally |
| EnablePartitionEof               | Whether to forward `RD_KAFKA_RESP_ERR__PARTITION_EOF` events when the consumer reaches the end of a partition |

## ProducerConfig
| Property Name              | Description                                                                                           |
|----------------------------|-------------------------------------------------------------------------------------------------------|
| EnableBackgroundPoll       | Specifies whether the producer should start a background poll thread to receive delivery reports and event notifications.                                      |
| EnableDeliveryReports      | Specifies whether to enable notification of delivery reports.                                        |
| DeliveryReportFields       | A comma-separated list of fields that may be optionally set in delivery reports.                    |
| RequestTimeoutMs           | The ack timeout of the producer request in milliseconds. This value is only enforced by the broker and relies on `request.required.acks` being != 0.       |
| MessageTimeoutMs           | Local message timeout. This is the maximum time librdkafka may use to deliver a message (including retries).                                                 |
| Partitioner                | Specifies the partitioner to be used. Options include: `random`, `consistent`, `consistent_random`, `murmur2`, `murmur2_random`, `fnv1a`, `fnv1a_random`. |
| CompressionLevel           | Compression level parameter for algorithm selected by configuration property `compression.codec`.   |
| TransactionalId            | Enables the transactional producer. The transactional.id is used to identify the same transactional producer instance across process restarts.              |
| TransactionTimeoutMs       | The maximum amount of time in milliseconds that the transaction coordinator will wait for a transaction status update from the producer before proactively aborting the ongoing transaction.  |
| EnableIdempotence          | When set to `true`, the producer will ensure that messages are successfully produced exactly once and in the original produce order.                          |
| EnableGaplessGuarantee     | When set to `true`, any error that could result in a gap in the produced message series when a batch of messages fails, will raise a fatal error (ERR__GAPLESS_GUARANTEE) and stop the producer.                          |
| QueueBufferingMaxMessages  | Maximum number of messages allowed on the producer queue.                                           |
| QueueBufferingMaxKbytes    | Maximum total message size sum allowed on the producer queue.                                      |
| LingerMs                   | Delay in milliseconds to wait for messages in the producer queue to accumulate before constructing message batches (MessageSets) to transmit to brokers.                         |
| MessageSendMaxRetries      | How many times to retry sending a failing Message.                                                   |
| RetryBackoffMs             | The backoff time in milliseconds before retrying a protocol request.                                |
| QueueBufferingBackpressureThreshold | The threshold of outstanding not yet transmitted broker requests needed to backpressure the producer's message accumulator.                 |
| CompressionType            | Compression codec to use for compressing message sets.                                              |
| BatchNumMessages           | Maximum number of messages batched in one MessageSet.                                               |
| BatchSize                  | Maximum size (in bytes) of all messages batched in one MessageSet, including protocol framing overhead.  |