using MediatR;
using Restaurants.Application.Restaurants.Dtos;

namespace Restaurants.Application.Restaurants.Queries.GetRestaurantById;

public class GetRestaurantByIdQuery(int id/*, string userId*/) : IRequest<RestaurantDto>
{

    //public GetRestaurantByIdQuery(int id) => Id = id;

    //public string UserId { get; init; } = userId;
    public int Id { get; } = id;

    //public int Id { get; set; } // if have body in controller

    //public int Id { get; init; } // if no constructor
}
