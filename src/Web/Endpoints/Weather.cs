using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WeatherApp.Application.WeatherForecasts.Queries.GetWeatherForecasts;
using WeatherApp.Domain.Entities;

namespace WeatherApp.Web.Endpoints;

public class Weather : EndpointGroupBase
{    
    public override void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.RequireAuthorization();
        groupBuilder.MapGet(GetWeatherForecasts);
    }

    [ProducesResponseType(typeof(WeatherForecast), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<Ok<WeatherForecast>> GetWeatherForecasts(ISender sender, [AsParameters] GetWeatherForecastQuery query)
    {        
        var forecast = await sender.Send(query);
        return TypedResults.Ok(forecast);
    }

}
