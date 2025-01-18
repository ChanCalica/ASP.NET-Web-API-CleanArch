using MediatR;

namespace Restaurants.Application.Restaurants.Commands.DeleteRestaurant;

public class DeleteRestaurantCommand(int id) : IRequest // put IRequest if no response
{
    public int Id { get; } = id;
}