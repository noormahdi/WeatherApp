namespace WeatherApp.Domain.Entities;
public record WeatherForecast(
    double TemperatureC,
    double WindSpeedKmph,
    string Condition,
    string Recommendation
);
