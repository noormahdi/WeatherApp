using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Application.Services;
public static class WeatherRecommendationService
{
    public static string GetRecommendation(string condition, double temp)
    {
        if(temp > 25)
        {
            return "It's a great day for a swim";
        }

        if(temp < 15 && (condition == "Rainy" || condition == "Snowing"))
        {
            return "Don't forget to bring a coat";
        }

        if(condition == "Rainy")
        {
            return "Don't forget the umberalla";
        }

        if (condition == "Sunny")
        {
            return "Don't forget to bring a hat";
        }

        return "Dress comfortably";
    }

    public static string MapCondition(string? weather)
    {
        return weather?.ToLower() switch
        {
            "clear" => "Sunny",
            "rain" or "drizzle" => "Rainy",
            "snow" => "Snowing",
            "wind" => "Windy",
            _ => "Sunny"
        };
    }
}
