using System.Configuration;

namespace App.Demo.ApplicationInsight.Web.Logging.Configuration
{
    public class AppInsightSection : ConfigurationSection
    {
        const string DEPENDENCY_THRESHOLD = "dependencythreshold";
        const string EXCLUDED = "excluded";

        [ConfigurationProperty(DEPENDENCY_THRESHOLD, IsRequired = true)]
        public int DependencyThreshold
        {
            get { return (int)this[DEPENDENCY_THRESHOLD]; }
            set { this[DEPENDENCY_THRESHOLD] = value; }
        }

        [ConfigurationProperty(EXCLUDED, IsDefaultCollection = true)]
        public ExcludedElementCollection Exluded
        {
            get { return (ExcludedElementCollection)this[EXCLUDED]; }
            set { this[EXCLUDED] = value; }
        }
    }
}