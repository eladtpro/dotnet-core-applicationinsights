using App.Demo.ApplicationInsight.Definitions.Middleware;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;

namespace App.Demo.ApplicationInsight.Definitions.Filter
{
    public class TelemetryBotFilter : TelemetryProcessor
    {
        public TelemetryBotFilter(ITelemetryProcessor next) : base(next) { }

        /// <summary>
        /// Filter out bots and web tests
        /// </summary>
        /// <param name="telemetry">telemetry item for filtering</param>
        protected override bool Continue(ITelemetry telemetry)
        {
            if (!string.IsNullOrEmpty(telemetry.Context.Operation.SyntheticSource)) return false;
            return true;
        }
    }
}
