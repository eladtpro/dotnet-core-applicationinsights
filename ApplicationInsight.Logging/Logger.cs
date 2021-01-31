using App.Demo.ApplicationInsight.Logging.Extensions;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace App.Demo.ApplicationInsight.Logging
{
    public class Logger : ILogger
    {
        private readonly string category;
        private readonly TelemetryClient client;
        private readonly Dictionary<string, LogLevel> LogLevels;
        private readonly LogLevel Default = LogLevel.Information;
        private readonly IExternalScopeProvider scopeProvider;

        public Logger(string name, IConfiguration configuration, TelemetryClient client, IExternalScopeProvider scopeProvider)
        {
            this.category = name;
            this.client = client;
            this.scopeProvider = scopeProvider;
            Dictionary<string, string> logLevels = new Dictionary<string, string>();
            LogLevels = new Dictionary<string, LogLevel>();
            configuration.GetSection("Logging:LogLevel").Bind(LogLevels);
            foreach (KeyValuePair<string, string> kvp in logLevels)
                if (Enum.TryParse(kvp.Value, out LogLevel logLevel))
                    LogLevels[kvp.Key] = logLevel;
                else
                    throw new ArgumentOutOfRangeException("Logging.LogLevel", $"appsetting.*.json contains invalid [Logging:LogLevel] '${kvp.Value}'");

            if (LogLevels.TryGetValue("Default", out LogLevel defaultLevel))
                Default = defaultLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel)) return;

            IDictionary<string, string> properties = new Dictionary<string, string>() { { "EventId", eventId.ToString() } };

            // gather info about scope(s), if any
            if (scopeProvider != null)
                scopeProvider.ForEachScope((value, loggingProps) =>
                {
                    if (value is string)
                        properties["ScopeValue"] = value.ToString();
                    else if (value is IEnumerable<KeyValuePair<string, object>> props)
                        foreach (KeyValuePair<string, object> pair in props)
                            properties[pair.Key] = pair.Value.ToString();
                }, state);

            string message = $"[{category}] {formatter(state, exception)}";

            client.TrackTrace(message, logLevel.ToSeverity(), properties);
            if (null != exception)
                client.TrackException(exception, properties);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            if (LogLevel.None == logLevel)
                return false;

            LogLevel minLevel = Default;
            // TODO: refactor match algoritm for assemblies trees, e.g. Microsoft & Microsoft.Hosting.Lifetime
            string name = Assembly.GetEntryAssembly().GetName().Name;
            if (LogLevels.TryGetValue(name, out LogLevel level))
                minLevel = level;

            return (logLevel > minLevel);
        }

        public IDisposable BeginScope<TState>(TState state) => scopeProvider.Push(state);
    }
}
