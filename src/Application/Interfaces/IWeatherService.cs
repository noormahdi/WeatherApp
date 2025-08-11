using WeatherApp.Domain.Entities;

namespace WeatherApp.Application.Interfaces;
public interface IWeatherService
{
    Task<WeatherForecast> GetWeatherAsync(double latitude, double longitude, CancellationToken cancellationToken);
}
