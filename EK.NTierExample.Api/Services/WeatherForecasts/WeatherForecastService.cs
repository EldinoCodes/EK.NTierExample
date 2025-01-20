using EK.NTierExample.Api.Data.Repositories.WeatherForecasts;
using EK.NTierExample.Api.Models.WeatherForecasts;

namespace EK.NTierExample.Api.Services.WeatherForecasts;

public interface IWeatherForecastService
{
    Task<List<WeatherForecast>> WeatherForecastGetNextAsync(int days, CancellationToken cancellationToken = default);
}

public class WeatherForecastService(IWeatherSummaryRepository weatherSummaryRepository) : IWeatherForecastService
{
    protected readonly IWeatherSummaryRepository _weatherSummaryRepository = weatherSummaryRepository;

    public async Task<List<WeatherForecast>> WeatherForecastGetNextAsync(int days, CancellationToken cancellationToken = default)
    {
        var ret = new List<WeatherForecast>();
        if (days == 0) return ret;

        var summaries = await _weatherSummaryRepository.WeatherSummaryGetAllAsync(cancellationToken);

        var res = Enumerable.Range(1, days)
            .Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = summaries.ElementAt(Random.Shared.Next(summaries.Count)).Description
            });

        if (res.Any())
            ret.AddRange(res);

        return ret;
    }
}
