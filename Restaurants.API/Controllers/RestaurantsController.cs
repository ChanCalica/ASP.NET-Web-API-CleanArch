using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Restaurants;
using Restaurants.Application.Restaurants.Dtos;

namespace Restaurants.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RestaurantsController(IRestaurantsService restaurantsService) : ControllerBase
{
    //private readonly IRestaurantsService _restaurantsService;

    //public RetaurantsController(IRestaurantsService restaurantsService)
    //{
    //    _restaurantsService = restaurantsService;
    //}

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var restaurants = await restaurantsService.GetAllRestaurants();

        return Ok(restaurants);
    }

    [HttpGet("{Id}")]
    public async Task<IActionResult> GetById([FromRoute] int Id)
    {
        var restaurant = await restaurantsService.GetRestaurant(Id);

        if (restaurant is null)
            return NotFound();

        return Ok(restaurant);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRestaurant([FromBody] CreateRestaurantDto createRestaurantDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        int Id = await restaurantsService.CreateRestaurant(createRestaurantDto);

        return CreatedAtAction(nameof(GetById), new { Id }, null);
    }
}
