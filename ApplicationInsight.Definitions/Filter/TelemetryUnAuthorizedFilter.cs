using App.Demo.ApplicationInsight.Definitions.Middleware;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using System;

namespace App.Demo.ApplicationInsight.Definitions.Filter
{
    public class TelemetryUnAuthorizedFilter : TelemetryProcessor
    {
        public TelemetryUnAuthorizedFilter(ITelemetryProcessor next) : base(next) { }

        /// <summary>
        /// filter out requests with a "401" response
        /// </summary>
        /// <param name="telemetry"></param>
        protected override bool Continue(ITelemetry telemetry)
        {
            if (telemetry is not RequestTelemetry requestTelemetry) return false;

            if (requestTelemetry.ResponseCode.Equals("401", StringComparison.OrdinalIgnoreCase)) return false;

            return true;
        }
    }
}
