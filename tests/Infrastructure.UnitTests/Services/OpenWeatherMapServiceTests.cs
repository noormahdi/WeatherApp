using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using WeatherApp.Infrastructure.Services;

namespace Infrastructure.UnitTests.Services;

public class OpenWeatherMapServiceTests
{
    private static HttpClient CreateMockHttpClient(HttpResponseMessage response)
    {
        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        return new HttpClient(handlerMock.Object);
    }

    private static IConfiguration CreateMockConfig(string apiKey)
    {
        var config = new Mock<IConfiguration>();
        config.Setup(c => c["OpenWeatherMap:ApiKey"]).Returns(apiKey);
        return config.Object;
    }

    [Test]
    public async Task GetWeatherAsync_ReturnsWeatherForecast_WhenApiResponseIsValid()
    {
        // Arrange
        var apiKey = "test-api-key";
        var latitude = 10.0;
        var longitude = 20.0;
        var cancellationToken = CancellationToken.None;

        var apiResponse = new
        {
            Weather = new[] { new { Main = "Clear", Description = "clear sky" } },
            Main = new { Temp = 25.0 },
            Wind = new { Speed = 2.0 }
        };

        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(apiResponse)
        };

        var httpClient = CreateMockHttpClient(response);
        var config = CreateMockConfig(apiKey);

        var service = new OpenWeatherMapService(httpClient, config);

        // Act
        var result = await service.GetWeatherAsync(latitude, longitude, cancellationToken);

        // Assert
        Assert.NotNull(result);
        Assert.That(result.TemperatureC, Is.EqualTo(25.0));
        Assert.That(result.WindSpeedKmph, Is.EqualTo(2.0 * 3.6));
        Assert.That(result.Condition, Is.EqualTo("Sunny"));
        Assert.That(result.Recommendation, Is.EqualTo("Don't forget to bring a hat"));
    }
}
