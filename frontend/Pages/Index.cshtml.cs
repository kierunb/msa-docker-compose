using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace frontend.Pages;

public class IndexModel : PageModel {
    private readonly ILogger<IndexModel>? _logger;
    private readonly IHttpClientFactory? _httpClientFactory;
    public IEnumerable<WeatherForecast>? Forecasts { get; set; }

    public IndexModel (ILogger<IndexModel> logger, IHttpClientFactory httpClientFactory) {
        this._httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task OnGet () {
        
        int fizz = 3, buzz = 5;
        _logger?.LogInformation("The current values are {Fizz} and {Buzz}", fizz, buzz);
        
        var httpClient = _httpClientFactory?.CreateClient("weather-client");
        Forecasts = await httpClient?.GetFromJsonAsync<IEnumerable<WeatherForecast>>("weatherforecast");
        ViewData["forecasts"] = Forecasts?.Count();
    }
}