using App.Demo.ApplicationInsight.Definitions.Extensions;
using App.Demo.ApplicationInsight.Definitions.Middleware;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;


namespace App.Demo.ApplicationInsight.Definitions.Filter
{
    public class TelemetryFastRemoteDependencyCallsFilter : TelemetryProcessor
    {
        private readonly double minDuration;

        public TelemetryFastRemoteDependencyCallsFilter(ITelemetryProcessor next, IConfiguration config) : base(next)
        {
            minDuration = config.GetMinDuration();
        }

        /// <summary>                                                                                                               
        /// Filter out fast dependency calls - post only the slow calls
        /// </summary>
        /// <param name="telemetry">telemetry item for filtering</param>
        protected override bool Continue(ITelemetry telemetry)
        {
            if (telemetry is not DependencyTelemetry dependency) return false;

            if (dependency.Duration.TotalMilliseconds < minDuration) return false;

            return true;
        }
    }
}
