using App.Demo.ApplicationInsight.Web.Logging.Configuration;
using ApplicationInsight.Web.Core;
using ApplicationInsight.Web.Extensions;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;

namespace App.Demo.ApplicationInsight.Web.Logging.Filters
{
    public class TelemetryPathFilter : TelemetryProcessor
    {
        private readonly string[] excludes;

        public TelemetryPathFilter(ITelemetryProcessor next) : base(next)
        {
            excludes = AppInsightConfiguration.ExcludedPaths;
        }

        protected override bool Continue(ITelemetry telemetry)
        {
            string path = telemetry.Context.GlobalProperties["RoutePath"];
            return (!Excluded(path));
        }

        private bool Excluded(string path)
        {
            foreach (string pattern in excludes)
                return path.IsMatch(pattern);

            return false;
        }
    }
}
