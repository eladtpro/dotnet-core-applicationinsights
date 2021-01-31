using Microsoft.Extensions.Configuration;

namespace App.Demo.ApplicationInsight.Definitions.Extensions
{
    public static class IConfigurationExtensions
    {
        public static string[] GetExcludedPathPatterns(this IConfiguration config)
        {
            IConfigurationSection excludePatterns = config.GetSection("ApplicationInsightsExtensions:ExcludedPatterns");
            return excludePatterns.Get<string[]>();
        }

        public static string[] GetClaims(this IConfiguration config)
        {
            IConfigurationSection claims = config.GetSection("ApplicationInsightsExtensions:Claims");
            return claims.Get<string[]>();
        }


        public static double GetMinDuration(this IConfiguration config)
        {
            IConfigurationSection exclude = config.GetSection("ApplicationInsightsExtensions:RDDMinDurationMilliseconds");
            return exclude.Get<double>();
        }

    }
}
