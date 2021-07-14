using App.Demo.ApplicationInsight.Web.Logging.Filters;
using App.Demo.ApplicationInsight.Web.Logging.Initializers;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ApplicationInsight.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //ApplicationInsight
            
            //Initializers:
            TelemetryConfiguration.Active.TelemetryInitializers.Add(new HttpTelemetryInitializer());
            TelemetryConfiguration.Active.TelemetryInitializers.Add(new HttpRequestTelemetryInitializer());
            TelemetryConfiguration.Active.TelemetryInitializers.Add(new WebMethodInitializer());

            //Procesors:
            TelemetryProcessorChainBuilder builder = TelemetryConfiguration.Active.DefaultTelemetrySink.TelemetryProcessorChainBuilder;
            
            builder.Use((next) => new TelemetryUnAuthorizedFilter(next));
            builder.Use((next) => new TelemetryPathFilter(next));
            builder.Use((next) => new TelemetryBotFilter(next));
            builder.Use((next) => new TelemetryFastRemoteDependencyCallsFilter(next));
            builder.Use((next) => new SuccessfulDependencyFilter(next));

            builder.Build();

        }
    }
}
