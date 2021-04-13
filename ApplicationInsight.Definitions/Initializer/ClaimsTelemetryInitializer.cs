using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Security.Claims;
using App.Demo.ApplicationInsight.Definitions.Extensions;
using System.Linq;

namespace App.Demo.ApplicationInsight.Definitions.Initializer
{
    public class ClaimsTelemetryInitializer : ITelemetryInitializer
    {
        private readonly IHttpContextAccessor context;
        private readonly string[] claims;

        public ClaimsTelemetryInitializer(IHttpContextAccessor httpContextAccessor, IConfiguration config)
        {
            context = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            claims = config.GetClaims();
        }

        public void Initialize(ITelemetry telemetry)
        {
            if (telemetry is not RequestTelemetry request) return;

            foreach (Claim claim in context.HttpContext.User.Claims.Where(c => claims.Contains(c.Type)))
                request.Properties[claim.Type] = claim?.Value;
        }
    }
}
