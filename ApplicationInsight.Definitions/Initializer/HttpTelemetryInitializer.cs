using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System;

namespace App.Demo.ApplicationInsight.Definitions.Initializer
{
    public class HttpTelemetryInitializer : ITelemetryInitializer
    {
        private readonly IHttpContextAccessor context;
        public HttpTelemetryInitializer(IHttpContextAccessor httpContextAccessor)
        {
            context = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public void Initialize(ITelemetry telemetry)
        {
            telemetry.Context.GlobalProperties["Ip"] = context.HttpContext?.Connection.RemoteIpAddress.ToString();
            telemetry.Context.GlobalProperties["RoutePath"] = context.HttpContext?.Request.Path;
            telemetry.Context.GlobalProperties["Host"] = context.HttpContext?.Request.Host.Value;
            if (null != context.HttpContext?.Features.Get<ISessionFeature>())
                telemetry.Context.GlobalProperties["SessionId"] = context.HttpContext?.Session?.Id;
        }
    }
}
