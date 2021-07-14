using System.Configuration;

namespace App.Demo.ApplicationInsight.Web.Logging.Configuration
{
    [ConfigurationCollection(typeof(ExcludedElement))]

    public class ExcludedElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ExcludedElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ExcludedElement)element).Path;
        }
    }
}