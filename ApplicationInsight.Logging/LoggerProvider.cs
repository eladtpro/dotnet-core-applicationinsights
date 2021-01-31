using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;

namespace App.Demo.ApplicationInsight.Logging
{
    public class LoggerProvider : IDisposable, ILoggerProvider, ISupportExternalScope
    {
        private readonly IConfiguration configuration;
        private readonly TelemetryClient client;
        private readonly ConcurrentDictionary<string, Logger> loggers = new ConcurrentDictionary<string, Logger>();
        private IExternalScopeProvider scopeProvider;

        public LoggerProvider(IConfiguration configuration, TelemetryClient client)
        {
            this.configuration = configuration;
            this.client = client;
        }

        public ILogger CreateLogger(string categoryName) =>
            loggers.GetOrAdd(categoryName, (name) => new Logger(name, configuration, client, scopeProvider ?? new LoggerExternalScopeProvider()));

        public void SetScopeProvider(IExternalScopeProvider scopeProvider)
        {
            this.scopeProvider = scopeProvider;
        }

        public void Dispose()
        {
            loggers.Clear();
            GC.SuppressFinalize(this);
        }
    }
}
