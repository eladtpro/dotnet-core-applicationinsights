using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using System;
using System.Web.Services.Protocols;

namespace App.Demo.ApplicationInsight.Web.Logging.Core
{
    // https://docs.microsoft.com/en-us/dotnet/api/system.web.services.protocols.soapextension?view=netframework-4.8#examples
    // https://docs.microsoft.com/en-us/azure/azure-monitor/app/custom-operations-tracking#long-running-background-tasks
    public class TrackSoapDependency : SoapExtension
    {
        private static TelemetryConfiguration telemetryConfiguration;
        private TelemetryClient telemetryClient;
        IOperationHolder<DependencyTelemetry> operation;

        static TrackSoapDependency()
        {
            telemetryConfiguration = TelemetryConfiguration.CreateDefault();
        }


        public override object GetInitializer(LogicalMethodInfo methodInfo, SoapExtensionAttribute attribute)
        {
            return null;
        }

        public override object GetInitializer(Type WebServiceType)
        {
            return null;
        }

        public override void Initialize(object initializer)
        {
            telemetryClient = new TelemetryClient(telemetryConfiguration);
        }

        public override void ProcessMessage(SoapMessage message)
        {
            switch (message.Stage)
            {
                case SoapMessageStage.BeforeSerialize:
                    operation = telemetryClient.StartOperation<DependencyTelemetry>(message.Action);
                    break;
                case SoapMessageStage.AfterSerialize:
                    break;
                case SoapMessageStage.BeforeDeserialize:
                    break;
                case SoapMessageStage.AfterDeserialize:
                    if (null != message.Exception)
                        telemetryClient.TrackException(message.Exception);
                    telemetryClient.StopOperation(operation);
                    break;
            }
        }
    }
}