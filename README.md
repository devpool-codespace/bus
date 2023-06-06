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
````csharp
var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((host,services) =>
    {
        services.AddKafka(host.Configuration,  busBuilder =>
        {
            // Add subscribers for events
            busBuilder.AddConsumer<MyTestEvent>();
            busBuilder.AddConsumer<TestTopic>();
        });
    })
    .Build();

host.Run();
````