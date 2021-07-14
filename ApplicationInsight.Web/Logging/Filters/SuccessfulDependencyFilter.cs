using ApplicationInsight.Web.Core;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace App.Demo.ApplicationInsight.Web.Logging.Filters
{
    public class SuccessfulDependencyFilter : TelemetryProcessor
    {
        public SuccessfulDependencyFilter(ITelemetryProcessor next) : base(next) { }

        protected override bool Continue(ITelemetry telemetry)
        {
            DependencyTelemetry dependency = telemetry as DependencyTelemetry;
            if ((null != dependency) && dependency.Success == true) return false;
            return true;
        }
    }
}
