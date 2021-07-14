using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using System.Web;

namespace App.Demo.ApplicationInsight.Web.Logging.Initializers
{
    public class HttpRequestTelemetryInitializer : ITelemetryInitializer
    {
        public void Initialize(ITelemetry telemetry)
        {
            if (null == HttpContext.Current) return;
            RequestTelemetry request = telemetry as RequestTelemetry;
            if (null == telemetry) return;

            request.Properties["Referer"] = HttpContext.Current.Request.Headers["Referer"].ToString();
        }
    }
}
