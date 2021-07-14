using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using System.Web;

namespace App.Demo.ApplicationInsight.Web.Logging.Initializers
{
    public class HttpTelemetryInitializer : ITelemetryInitializer
    {
        public void Initialize(ITelemetry telemetry)
        {
            if (null == HttpContext.Current) return;

            telemetry.Context.GlobalProperties["Ip"] = HttpContext.Current?.Request.UserHostAddress;
            telemetry.Context.GlobalProperties["RoutePath"] = HttpContext.Current?.Request.Path;
            telemetry.Context.GlobalProperties["Host"] = HttpContext.Current?.Request.UserHostName;
            telemetry.Context.GlobalProperties["SessionId"] = HttpContext.Current?.Session?.SessionID;
        }
    }
}
