using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using System.Linq;
using System.Web;

namespace App.Demo.ApplicationInsight.Web.Logging.Initializers
{
    public class WebMethodInitializer : ITelemetryInitializer
    {
        public void Initialize(ITelemetry telemetry)
        {
            RequestTelemetry requestTelemetry = telemetry as RequestTelemetry;
            if (requestTelemetry == null) return;
            string webServiceMethod = null;

            // Is this a TrackRequest() ?
            string requestMethodName = HttpContext.Current.Request.Params["op"]; // Item("HTTP_SOAPACTION");

            if (string.IsNullOrWhiteSpace(requestMethodName))
            {
                if (HttpContext.Current.Request.PathInfo != null)
                    requestMethodName = HttpContext.Current.Request.PathInfo;

                if (requestMethodName != "" && requestMethodName != null)
                {
                    requestMethodName = requestMethodName.Replace("/", "");
                    // If we set the Success property, the SDK won't change it:
                    requestTelemetry.Success = true;
                    // Allow us to filter these requests in the portal:
                    requestTelemetry.Properties["WebMethodName"] = requestMethodName;
                    webServiceMethod = requestMethodName;
                }
            }

            string soapAction = HttpContext.Current.Request.Headers["SOAPAction"];

            if (!string.IsNullOrWhiteSpace(soapAction))
            {
                soapAction = soapAction.Replace("\"", "");
                string soapActionMethod = soapAction.Split('/').Last();
                requestTelemetry.Properties["SOAPAction"] = soapAction;
                webServiceMethod = soapActionMethod;
            }

            if (webServiceMethod != null)
                requestTelemetry.Context.Operation.Name = requestTelemetry.Context.Operation.Name.Replace("/" + webServiceMethod, "") + "/" + webServiceMethod;
        }

    }
}