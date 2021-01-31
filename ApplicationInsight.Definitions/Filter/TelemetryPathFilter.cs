using App.Demo.ApplicationInsight.Definitions.Extensions;
using App.Demo.ApplicationInsight.Definitions.Middleware;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;

namespace App.Demo.ApplicationInsight.Definitions.Filter
{
    public class TelemetryPathFilter : TelemetryProcessor
    {
        private readonly string[] excludes;

        public TelemetryPathFilter(ITelemetryProcessor next, IConfiguration config) : base(next)
        {
            excludes = config.GetExcludedPathPatterns();
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
