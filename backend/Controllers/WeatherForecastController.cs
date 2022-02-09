using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace backend.Controllers;

[ApiController]
[Route ("[controller]")]
public class WeatherForecastController : ControllerBase {
    private static readonly string[] Summaries = new [] {
        "Freezing",
        "Bracing",
        "Chilly",
        "Cool",
        "Mild",
        "Warm",
        "Balmy",
        "Hot",
        "Sweltering",
        "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IDistributedCache _cache;

    public WeatherForecastController (
        ILogger<WeatherForecastController> logger,
        IDistributedCache cache) {
        _cache = cache;
        _logger = logger;
    }

    [HttpGet (Name = "GetWeatherForecast")]
    public async Task<IActionResult> Get () {

        var weather = await _cache.GetStringAsync ("weather");

        if (weather == null) {
            var forecasts = Enumerable.Range (1, 5).Select (index => new WeatherForecast {
                    Date = DateTime.Now.AddDays (index),
                        TemperatureC = Random.Shared.Next (-20, 55),
                        Summary = Summaries[Random.Shared.Next (Summaries.Length)]
                })
                .ToArray ();

            weather = JsonSerializer.Serialize (forecasts);

            await _cache.SetStringAsync ("weather", weather, new DistributedCacheEntryOptions {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds (5)
            });
        }

        return Ok (weather);
    }
}