
namespace Restaurants.API.Services
{
    public interface IWeatherForecastService
    {
        IEnumerable<WeatherForecast> Get();
        IEnumerable<WeatherForecast> Get2(int count, int minTemp, int maxTemp);
    }
}