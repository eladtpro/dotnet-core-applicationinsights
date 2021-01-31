using App.Demo.ApplicationInsight.Definitions.Middleware;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace App.Demo.ApplicationInsight.Definitions.Filter
{
    public class SuccessfulDependencyFilter : TelemetryProcessor
    {
        public SuccessfulDependencyFilter(ITelemetryProcessor next) : base(next) { }

        protected override bool Continue(ITelemetry telemetry)
        {
            if ((telemetry is DependencyTelemetry dependency) && dependency.Success == true) return false;
            return true;
        }
    }
}
