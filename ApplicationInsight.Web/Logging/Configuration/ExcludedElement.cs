using System.Configuration;

namespace App.Demo.ApplicationInsight.Web.Logging.Configuration
{
    public class ExcludedElement : ConfigurationElement
    {
        [ConfigurationProperty("path", IsKey = true, IsRequired = true)]
        public string Path
        {
            get { return (string)this["path"]; }
            set { this["path"] = value; }
        }
    }
}