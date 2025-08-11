using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WeatherApp.Application.Weather.Queries.GetWeatherForecasts;
using WeatherApp.Domain.Entities;

namespace WeatherApp.Web.IntegrationTests.Endpoints
{
    [TestFixture]
    public class ErrorSimulatorTests
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
        public async Task GetWeatherForecasts_ReturnsServiceUnavailable_OnException()
        {
            // Arrange
            var forecast = new WeatherForecast(25, 15, "Sunny", "Don't forget to bring a hat");
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

            // Act and Assert
            var response = await client.GetAsync("api/weather?Latitude=10&Longitude=20");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            response = await client.GetAsync("api/weather?Latitude=10&Longitude=20");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            response = await client.GetAsync("api/weather?Latitude=10&Longitude=20");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            response = await client.GetAsync("api/weather?Latitude=10&Longitude=20");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            response = await client.GetAsync("api/weather?Latitude=10&Longitude=20");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.ServiceUnavailable));
        }
    }
}
