using System.Configuration;

namespace App.Demo.ApplicationInsight.Web.Logging.Configuration
{
    [ConfigurationCollection(typeof(AppInsightElement))]

    public class AppInsightElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new AppInsightElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((AppInsightElement)element).Name;
        }
    }
}