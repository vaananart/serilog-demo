
using CustomSerilogLib.SetupExtensions;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using System.Diagnostics;

[assembly: HostingStartup(typeof(CustomSerilogLib.HostingStartups.StartupCustomSerilogEnhancementHostingStartup))]
namespace CustomSerilogLib.HostingStartups;
internal class StartupCustomSerilogEnhancementHostingStartup : IHostingStartup
{
	public void Configure(IWebHostBuilder builder)
	{
		Debugger.Break();
		builder.ConfigureLogging(x => x.ClearProviders());	
		builder.ConfigureServices((context,services) => 
		{	
			
			var configuration = context.Configuration;
			services.AddCustomSerilog(configuration);
			var loggerFactory = services.BuildServiceProvider().GetRequiredService<ILoggerFactory>();
			var logger = loggerFactory.CreateLogger("StartupCostomSerilogEnhancementHostingStartup");
			logger.LogInformation("Testing in StartupCostomSerilogEnhancementHostingStartup");
		});
	}
}