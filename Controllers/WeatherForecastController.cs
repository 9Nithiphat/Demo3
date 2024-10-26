using Demo2.biz;
using Microsoft.AspNetCore.Mvc;
using System.Configuration;

namespace Demo2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly PackageService _packageService;
        private readonly IConfiguration _configuration;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, PackageService packageService, IConfiguration configuration)
        {
            _logger = logger;
            _packageService = packageService;
            _configuration = configuration;
        }
        

        [HttpGet(Name = "GetWeatherForecast")]
        public string GetTest()
        {
            _logger.LogInformation("GetTest called.");
            return _configuration["packageDefault"];
        }
        //public IEnumerable<WeatherForecast> Get()
        //{
        //    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //    {
        //        Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
        //        TemperatureC = Random.Shared.Next(-20, 55),
        //        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        //    })
        //    .ToArray();
        //}
    }
}
