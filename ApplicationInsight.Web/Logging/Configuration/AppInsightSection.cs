using System.Configuration;

namespace App.Demo.ApplicationInsight.Web.Logging.Configuration
{
    public class AppInsightSection : ConfigurationSection
    {
        //<section name = "appinsight" type="App.Demo.ApplicationInsight.Web.Logging.Configuration.AppInsightSection"/>

        public static AppInsightSection Config = 
                    ConfigurationManager.GetSection("appinsight") as AppInsightSection;


        [ConfigurationProperty("settings", IsDefaultCollection = true)]
        public AppInsightElementCollection Feeds
        {
            get { return (AppInsightElementCollection)this["settings"]; }
            set { this["settings"] = value; }
        }
    }
}