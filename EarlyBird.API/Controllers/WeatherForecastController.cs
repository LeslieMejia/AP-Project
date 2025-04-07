using Microsoft.AspNetCore.Mvc;
using EarlyBirdAPI.Model;  // This ensures the controller knows about the WeatherForecast class
namespace EarlyBirdAPI.Controllers;


////using EarlyBirdAPI.Model.Entities;  // To access the models like User, Job, etc.
//using EarlyBirdAPI.Model.Repositories.Interfaces;  // To access interfaces like IUserRepository
//using EarlyBirdAPI.Model.Repositories;  // To access services like UserRepository


[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            //Date = DateTime.FromDateTime(DateTime.Now.AddDays(index))
            Date = DateTime.Now.AddDays(index), 
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}
