using System;
using System.Configuration;
using System.Linq;

namespace App.Demo.ApplicationInsight.Web.Logging.Configuration
{
    public static class AppInsightConfiguration
    {
        //<section name = "appinsight" type="App.Demo.ApplicationInsight.Web.Logging.Configuration.AppInsightSection"/>
        const string SECTION_NAME = "appinsight";

        private static AppInsightSection Section
        {
            get
            {
                AppInsightSection config = ConfigurationManager.GetSection(SECTION_NAME) as AppInsightSection;
                if (null == config) throw new ConfigurationErrorsException($"{SECTION_NAME} configuration section is missing in web.config file");
                return config;
            }
        }

        private static Lazy<int> dependencyThreshold = new Lazy<int>(() =>
        {
            return Section.DependencyThreshold;

        }, System.Threading.LazyThreadSafetyMode.PublicationOnly);

        private static Lazy<string[]> excludedPaths = new Lazy<string[]>(() =>
        {
            return Section.Exluded.Cast<ExcludedElement>().Select(e => e.Path).ToArray();

        }, System.Threading.LazyThreadSafetyMode.PublicationOnly);


        public static int DependencyThreshold { get { return dependencyThreshold.Value; } }
        public static string[] ExcludedPaths { get { return excludedPaths.Value; } }
    }
}