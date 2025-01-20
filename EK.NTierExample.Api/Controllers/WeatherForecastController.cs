using EK.NTierExample.Api.Services.WeatherForecasts;
using Microsoft.AspNetCore.Mvc;

namespace EK.NTierExample.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherForecastService weatherForecastService) : ControllerBase
{    
    private readonly ILogger<WeatherForecastController> _logger = logger;
    private readonly IWeatherForecastService _weatherForecastService = weatherForecastService;

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<IActionResult> Get(int days = 5, CancellationToken cancellationToken = default)
    {
        var ret = await _weatherForecastService.WeatherForecastGetNextAsync(days, cancellationToken);

        return Ok(ret);
    }
}
