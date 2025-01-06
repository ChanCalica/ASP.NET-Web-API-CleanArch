using MediatR;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Queries.GetAllRestaurants;
using Restaurants.Application.Restaurants.Queries.GetRestaurantById;

namespace Restaurants.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RestaurantsController(/*IRestaurantsService restaurantsService*/ IMediator mediator) : ControllerBase
{
    //private readonly IRestaurantsService _restaurantsService;

    //public RetaurantsController(IRestaurantsService restaurantsService)
    //{
    //    _restaurantsService = restaurantsService;
    //}

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        //var restaurants = await restaurantsService.GetAllRestaurants();

        var restaurants = await mediator.Send(new GetAllRestaurantsQuery());

        return Ok(restaurants);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        //var restaurant = await restaurantsService.GetRestaurant(Id);

        var restaurant = await mediator.Send(new GetRestaurantByIdQuery(id));
        //{
        //    Id = id
        //});

        if (restaurant is null)
            return NotFound();

        return Ok(restaurant);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRestaurant(/*[FromBody] CreateRestaurantDto createRestaurantDto*/ CreateRestaurantCommand command)
    {
        //if (!ModelState.IsValid)
        //{
        //    return BadRequest(ModelState);
        //}

        //int Id = await restaurantsService.CreateRestaurant(createRestaurantDto);

        int Id = await mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { Id }, null);
    }
}
