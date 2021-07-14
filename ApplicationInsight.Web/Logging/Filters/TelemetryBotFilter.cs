using ApplicationInsight.Web.Core;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;

namespace App.Demo.ApplicationInsight.Web.Logging.Filters
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
            if (!string.IsNullOrEmpty(telemetry?.Context?.Operation?.SyntheticSource)) return false;
            return true;
        }
    }
}
