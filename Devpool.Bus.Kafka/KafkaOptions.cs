using System.Reflection;
using Confluent.Kafka;
using Devpool.Bus.Abstractions;

namespace Devpool.Bus.Kafka;

public record KafkaOptions
{
    public ConsumerConfig? ConsumerConfig { get; set; }
    public ProducerConfig? ProducerConfig { get; set; }
}