using App.Demo.ApplicationInsight.Web.Logging.Configuration;
using ApplicationInsight.Web.Core;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;


namespace App.Demo.ApplicationInsight.Web.Logging.Filters
{
    public class TelemetryFastRemoteDependencyCallsFilter : TelemetryProcessor
    {
        private readonly double minDuration;

        public TelemetryFastRemoteDependencyCallsFilter(ITelemetryProcessor next) : base(next)
        {
            minDuration = AppInsightConfiguration.DependencyCallThresholdMS;
        }

        /// <summary>                                                                                                               
        /// Filter out fast dependency calls - post only the slow calls
        /// </summary>
        /// <param name="telemetry">telemetry item for filtering</param>
        protected override bool Continue(ITelemetry telemetry)
        {
            DependencyTelemetry dependency = telemetry as DependencyTelemetry;

            if (null == telemetry) return false;

            if (dependency.Duration.TotalMilliseconds < minDuration) return false;

            return true;
        }
    }
}
