using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.DeleteRestaurant;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Application.Restaurants.Queries.GetAllRestaurants;
using Restaurants.Application.Restaurants.Queries.GetRestaurantById;
using Restaurants.Domain.Constants;
using Restaurants.Infrastructure.Authorization;

namespace Restaurants.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class RestaurantsController(/*IRestaurantsService restaurantsService*/ IMediator mediator) : ControllerBase
{
    //private readonly IRestaurantsService _restaurantsService;

    //public RetaurantsController(IRestaurantsService restaurantsService)
    //{
    //    _restaurantsService = restaurantsService;
    //}

    [HttpGet]
    //[Authorize]
    [AllowAnonymous]
    //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<RestaurantDto>))]
    //public async Task<IActionResult> GetAll()
    public async Task<ActionResult<IEnumerable<RestaurantDto>>> GetAll()
    {
        //var restaurants = await restaurantsService.GetAllRestaurants();

        var restaurants = await mediator.Send(new GetAllRestaurantsQuery());

        return Ok(restaurants);
    }

    [HttpGet("{id}")]
    [Authorize(Policy = PolicyNames.HasNationality)]
    //public async Task<IActionResult> GetById([FromRoute] int id)
    public async Task<ActionResult<RestaurantDto?>> GetById([FromRoute] int id)
    {
        //var restaurant = await restaurantsService.GetRestaurant(Id);

        //var userId = User.Claims.FirstOrDefault(c => c.Type == "<id claim type>")!.Value;
        var restaurant = await mediator.Send(new GetRestaurantByIdQuery(id));
        //{
        //    Id = id
        //});

        //if (restaurant is null)
        //    return NotFound();

        return Ok(restaurant);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRestaurant([FromRoute] int id)
    {
        /*var isDeleted =*/
        await mediator.Send(new DeleteRestaurantCommand(id));

        //if (isDeleted)
        return NoContent();

        //return NotFound();
    }

    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateRestaurant([FromRoute] int id, UpdateRestaurantCommand command)
    {
        command.Id = id;
        /*     var isUpdated = */
        await mediator.Send(command);

        //if (isUpdated)
        return NoContent();

        //return NotFound();
    }

    [HttpPost]
    [Authorize(Roles = UserRoles.Owner)]
    public async Task<IActionResult> CreateRestaurant(/*[FromBody] CreateRestaurantDto createRestaurantDto*/ CreateRestaurantCommand command)
    {
        //if (!ModelState.IsValid)
        //{
        //    return BadRequest(ModelState);
        //}

        //int Id = await restaurantsService.CreateRestaurant(createRestaurantDto);

        //User.IsInRole();

        int Id = await mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { Id }, null);
    }
}
