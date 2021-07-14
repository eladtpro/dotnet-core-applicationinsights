namespace App.Demo.ApplicationInsight.Web.Logging.Configuration
{
    public static class AppInsightConfiguration
    {
        public static double DependencyCallThresholdMS = 100;
        public static string[] ExcludedPathPatterns = new string[0];
    }
}