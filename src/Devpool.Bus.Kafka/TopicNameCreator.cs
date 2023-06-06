using System.Text.RegularExpressions;
using Devpool.Bus.Abstractions;

namespace Devpool.Bus.Kafka;

public abstract class TopicNameCreator
{
    public static string Create<TEvent>() where TEvent : IEvent
    {
        var eventType = typeof(TEvent);
        var value = eventType
            .ToString()
            .Split(".")
            .Last();

        return Regex.Replace(value, "(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z])", "-$1", RegexOptions.Compiled)
            .Trim()
            .ToLower();
    }
    
    public static string Create(Type eventType)
    {
        var value = eventType
            .ToString()
            .Split(".")
            .Last();

        return Regex.Replace(value, "(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z])", "-$1", RegexOptions.Compiled)
            .Trim()
            .ToLower();
    }
}