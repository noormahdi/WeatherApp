using System.Net;
using System.Net.Http.Json;
using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WeatherApp.Application.Weather.Queries.GetWeatherForecasts;
using WeatherApp.Domain.Entities;

namespace WeatherApp.Web.IntegrationTests.Endpoints
{
    [TestFixture]
    public class WeatherEndpointTests
    {
        private WebApplicationFactory<Program> _factory;

        [SetUp]
        public void SetUp()
        {
            _factory = new WebApplicationFactory<Program>();
        }

        [TearDown]
        public void TearDown()
        {
            _factory?.Dispose();
        }

        [Test]
        public async Task GetWeatherForecasts_ReturnsOk_WithForecast()
        {
            // Arrange
            var forecast = new WeatherForecast(25,15,"Sunny","Don't forget to bring a hat");

            var senderMock = new Mock<ISender>();
            senderMock
                .Setup(s => s.Send(It.IsAny<GetWeatherForecastQuery>(), default))
                .ReturnsAsync(forecast);

            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddSingleton(senderMock.Object);
                });
            }).CreateClient();

            // Act
            var response = await client.GetAsync("api/weather?Latitude=10&Longitude=20");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var result = await response.Content.ReadFromJsonAsync<WeatherForecast>();
            Assert.That(result, Is.Not.Null);
            Assert.That(result.TemperatureC, Is.EqualTo(forecast.TemperatureC));
            Assert.That(result.Recommendation, Is.EqualTo(forecast.Recommendation));
        }
    }
}
