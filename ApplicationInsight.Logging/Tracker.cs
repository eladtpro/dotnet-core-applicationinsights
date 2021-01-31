using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using System;
using System.Collections.Generic;

namespace App.Demo.ApplicationInsight.Logging
{
    public class Tracker
    {
        private readonly TelemetryClient client;

        public Tracker(TelemetryClient client)
        {
            this.client = client;
        }

        public void TrackEvent(string eventName, IDictionary<string, string> properties = null)
        {
            client.TrackEvent(eventName, properties);
        }

        public void TrackException(Exception exception, IDictionary<string, string> properties = null)
        {
            client.TrackException(exception, properties);
        }

        public void TrackRequest(string name, IDictionary<string, string> properties)
        {
            RequestTelemetry request = new RequestTelemetry
            {
                Name = name
            };

            //ISupportProperties props = (request.Context.Properties as ISupportProperties)
            //(telemetry as ISupportProperties).Properties["MyCustomKey"] = "MyCustomValue";

            foreach (KeyValuePair<string, string> property in properties)
                request.Context.GlobalProperties.Add(property);

            client.TrackRequest(request);
        }

    }
}
