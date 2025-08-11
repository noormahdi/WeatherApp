using WeatherApp.Application.Interfaces;
using WeatherApp.Domain.Entities;

namespace WeatherApp.Application.WeatherForecasts.Queries.GetWeatherForecasts;

public record GetWeatherForecastQuery : IRequest<WeatherForecast>
{
    public double Latitude { get; init; }
    public double Longitude { get; init; }
};

public class GetWeatherForecastQueryHandler : IRequestHandler<GetWeatherForecastQuery, WeatherForecast>
{
    private readonly IWeatherService _weatherService;
    public GetWeatherForecastQueryHandler(IWeatherService weatherService)
    {
        _weatherService = weatherService ?? throw new ArgumentNullException(nameof(weatherService));
    }

    public async Task<WeatherForecast> Handle(GetWeatherForecastQuery request, CancellationToken cancellationToken)
    {
        var forecast = await _weatherService.GetWeatherAsync(request.Latitude, request.Longitude, cancellationToken);
        return forecast;
    }
}
