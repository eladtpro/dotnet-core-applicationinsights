using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Extensions.Logging;

namespace App.Demo.ApplicationInsight.Definitions.Extensions
{
    public static class LogLevelExtensions
    {
        public static SeverityLevel ToSeverity(this LogLevel logLevel)
        {
            return logLevel switch
            {
                LogLevel.Information => SeverityLevel.Information,
                LogLevel.Warning => SeverityLevel.Warning,
                LogLevel.Error => SeverityLevel.Error,
                LogLevel.Critical => SeverityLevel.Critical,
                _ => SeverityLevel.Verbose,
            };
        }
    }
}
