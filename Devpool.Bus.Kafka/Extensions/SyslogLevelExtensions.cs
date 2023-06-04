using Confluent.Kafka;
using Microsoft.Extensions.Logging;

namespace Devpool.Bus.Kafka.Extensions;

/// <summary>
/// Методы расширения SyslogLevel
/// </summary>
public static class SyslogLevelExtensions
{
    /// <summary>
    /// Маппинг SyslogLevel в LogLevel
    /// </summary>
    /// <param name="level">Уровень логгирования кафки</param>
    /// <returns>Уровень логгирования Microsoft</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static LogLevel ToLogLevel(this SyslogLevel level) => level switch
    {
        SyslogLevel.Emergency => LogLevel.Error,
        SyslogLevel.Alert => LogLevel.Warning,
        SyslogLevel.Critical => LogLevel.Critical,
        SyslogLevel.Error => LogLevel.Error,
        SyslogLevel.Warning => LogLevel.Warning,
        SyslogLevel.Notice => LogLevel.Information,
        SyslogLevel.Info => LogLevel.Information,
        SyslogLevel.Debug => LogLevel.Debug,
        _ => throw new ArgumentOutOfRangeException(nameof(level), level, null)
    };
}