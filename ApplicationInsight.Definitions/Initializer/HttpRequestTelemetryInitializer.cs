using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;


namespace App.Demo.ApplicationInsight.Definitions.Initializer
{
    public class HttpRequestTelemetryInitializer : ITelemetryInitializer
    {
        private readonly IHttpContextAccessor context;

        public HttpRequestTelemetryInitializer(IHttpContextAccessor httpContextAccessor, IConfiguration config)
        {
            this.context = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public void Initialize(ITelemetry telemetry)
        {
            if (!(telemetry is RequestTelemetry request)) return;

            request.Properties["Referer"] = context.HttpContext?.Request.Headers["Referer"].ToString();
        }
    }
}
