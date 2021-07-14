using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;

namespace ApplicationInsight.Web.Core
{
    public abstract class TelemetryProcessor : ITelemetryProcessor
    {
        private readonly ITelemetryProcessor next;

        public TelemetryProcessor(ITelemetryProcessor next)
        {
            this.next = next;
        }

        abstract protected bool Continue(ITelemetry telemetry);

        public virtual void Process(ITelemetry telemetry)
        {
            if (Continue(telemetry))
                next?.Process(telemetry);
        }
    }
}
