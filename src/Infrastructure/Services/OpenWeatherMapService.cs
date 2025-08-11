using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using WeatherApp.Application.Interfaces;
using WeatherApp.Application.Services;
using WeatherApp.Domain.Entities;

namespace WeatherApp.Infrastructure.Services;
public class OpenWeatherMapService : IWeatherService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    public OpenWeatherMapService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _config = config;
    }

    public async Task<WeatherForecast> GetWeatherAsync(double latitude, double longitude, CancellationToken cancellationToken)
    {
        var apiKey = _config["OpenWeatherMap:ApiKey"];
        if (string.IsNullOrWhiteSpace(apiKey))
            throw new InvalidOperationException("API key is missing.");

        var url = $"https://api.openweathermap.org/data/2.5/weather?lat={latitude}&lon={longitude}&appid={apiKey}&units=metric";

        var data = await _httpClient.GetFromJsonAsync<OpenWeatherResponse>(url)
                   ?? throw new InvalidOperationException("Weather API returned no data.");

        var condition = WeatherRecommendationService.MapCondition(data.Weather.FirstOrDefault()?.Main);
        var recommendation = WeatherRecommendationService.GetRecommendation(condition, data.Main.Temp);

        return new WeatherForecast(
            TemperatureC: data.Main.Temp,
            WindSpeedKmph: data.Wind.Speed * 3.6,
            Condition: condition,
            Recommendation: recommendation
        );
    }

    private record OpenWeatherResponse(WeatherInfo[] Weather, MainInfo Main, WindInfo Wind);
    private record WeatherInfo(string Main, string Description);
    private record MainInfo(double Temp);
    private record WindInfo(double Speed);
}
