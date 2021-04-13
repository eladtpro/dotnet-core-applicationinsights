using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Logging;
using App.Demo.ApplicationInsight.Definitions.Initializer;
using Microsoft.OpenApi.Models;
using App.Demo.ApplicationInsight.Definitions.Filter;

namespace App.Demo.ApplicationInsight.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            Configuration = configuration;
            Logger = logger;
        }

        public IConfiguration Configuration { get; }
        public ILogger Logger { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            // TODO: add scoped request telemetries
            //services.AddScoped(telemetry.GetType());
            services.AddHttpContextAccessor();

            services.AddSingleton<ITelemetryInitializer, HttpTelemetryInitializer>();
            services.AddSingleton<ITelemetryInitializer, HttpRequestTelemetryInitializer>();
            services.AddSingleton<ITelemetryInitializer, ClaimsTelemetryInitializer>();

            ApplicationInsightsExtensions.AddApplicationInsightsTelemetry(services, Configuration)
                //.AddApplicationInsightsTelemetryProcessor<TelemetryUnAuthorizedFilter>()
                //.AddApplicationInsightsTelemetryProcessor<TelemetryPathFilter>()
                //.AddApplicationInsightsTelemetryProcessor<TelemetryBotFilter>()
                //.AddApplicationInsightsTelemetryProcessor<TelemetryFastRemoteDependencyCallsFilter>()
                //.AddApplicationInsightsTelemetryProcessor<SuccessfulDependencyFilter>()
                ;

            services.AddControllers().AddControllersAsServices();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ApplicationInsight", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApplicationInsight v1"));
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
