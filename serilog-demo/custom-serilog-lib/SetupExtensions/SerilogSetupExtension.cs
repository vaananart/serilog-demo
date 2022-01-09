
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Serilog;
using Serilog.Templates;
using Serilog.Templates.Themes;

namespace CustomSerilogLib.SetupExtensions;
public static class SerilogSetupExtension
{
	public static IServiceCollection AddCustomSerilog(this IServiceCollection serviceCollection, IConfiguration configuration)
	{
		var serviceProvider = serviceCollection.BuildServiceProvider();
		TelemetryConfiguration? telemetryConfiguration = null;

		telemetryConfiguration = serviceProvider.GetService<TelemetryConfiguration>();
		if (telemetryConfiguration == null)
		{
			serviceCollection.AddApplicationInsightsTelemetry(configuration);
			serviceProvider = serviceCollection.BuildServiceProvider();
			telemetryConfiguration = serviceProvider.GetRequiredService<TelemetryConfiguration>();
		}
		
		string templateString = "{ { timestamp: @t, message: @mt, loglevel: @l, properties: @p } }\n";
		var logConfig = new LoggerConfiguration()
		.ReadFrom.Configuration(configuration)
		.WriteTo.Console(new ExpressionTemplate(templateString, theme: TemplateTheme.Code))
		.WriteTo.ApplicationInsights(telemetryConfiguration,TelemetryConverter.Traces)
		.Enrich.WithEnvironmentName()
		.Enrich.WithThreadName()
		.Enrich.WithMachineName()
		.Enrich.WithProcessName()
		.Enrich.WithCorrelationId()
		.Enrich.WithEnvironmentUserName()
		.Enrich.WithThreadId()
		.Enrich.WithProcessId()
		.Enrich.WithProperty("Version", configuration["Version"]);

		if (!string.IsNullOrEmpty(configuration["Serilog:path"]))
		{
			logConfig.WriteTo.Async(x => x.File(new ExpressionTemplate(templateString)
			, rollingInterval: RollingInterval.Day
			, path: configuration["Serilog:path"]
			, rollOnFileSizeLimit: true)
			);
		}

		Log.Logger = logConfig.CreateLogger();
		serviceCollection.AddLogging(x => x.AddSerilog(Log.Logger));
		return serviceCollection;
	}
}
