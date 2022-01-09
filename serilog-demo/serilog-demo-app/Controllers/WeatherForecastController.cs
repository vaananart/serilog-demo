using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

using System.Diagnostics;

namespace serilog_demo_app.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IEnumerable<object> Get()
    {
		Debugger.Break();
		_logger.LogInformation("GET request for weather forecast.");

        var result = Enumerable.Range(1, 5).Select(index => new
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();

		using (_logger.BeginScope(JsonConvert.SerializeObject(result)))
		{
			_logger.LogInformation("Returning response");
		}
			return result;
    }
}
