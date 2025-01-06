using Microsoft.AspNetCore.Mvc;
using Restaurants.API.Models;
using Restaurants.API.Services;

namespace Restaurants.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IWeatherForecastService _weatherForecastService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,
            IWeatherForecastService weatherForecastService)
        {
            _logger = logger;
            _weatherForecastService = weatherForecastService;
        }

        //[HttpGet(Name = "GetWeatherForecast")]
        [HttpGet]
        [Route("{take}/example")]
        public IEnumerable<WeatherForecast> Get([FromQuery]int max, [FromRoute]int take)
        {
            var result = _weatherForecastService.Get();

            return result;
        }

        [HttpGet("currentDay")]
        public IActionResult GetCurrentDayForecast()
        {
            var result = _weatherForecastService.Get().First();

            //Response.StatusCode = 400;
            //return StatusCode(400, result); //ObjectResult
            return NotFound(result);
            //return result;
        }

        [HttpPost]
        public string Hello([FromBody] string name)
        {
            return $"Hello {name}";
        }

        [HttpPost("generate")]
        public IActionResult GetWeatherForecast([FromQuery] int count, [FromBody] TempRequest req)
        {
            if (count < 0 || req.Max < req.Min)
            {
                return BadRequest("Count has to be positive number, and max must be greater than the min value");
            }

            var result = _weatherForecastService.Get2(count, req.Min, req.Max);

            return Ok(result);
        }
    }
}