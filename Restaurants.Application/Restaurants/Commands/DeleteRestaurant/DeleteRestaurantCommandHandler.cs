using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;
namespace Restaurants.Application.Restaurants.Commands.DeleteRestaurant;

public class DeleteRestaurantCommandHandler(ILogger<DeleteRestaurantCommandHandler> logger,
    IRestaurantRepository restaurantRepository) : IRequestHandler<DeleteRestaurantCommand>
{
    public async Task Handle(DeleteRestaurantCommand request, CancellationToken cancellationToken)
    {
        //logger.LogInformation($"Deleting restaurant wuth id : {request.Id}");
        logger.LogInformation("Deleting restaurant wuth id : {RestaurantId}", request.Id);

        var restaurant = await restaurantRepository.GetByIdAsync(request.Id);

        if (restaurant is null)
            throw new NotFoundException(nameof(Restaurant), request.Id.ToString());
        //throw new NotFoundException($"Restaurant with {request.Id} doens't exist");
        //return false;

        await restaurantRepository.DeleteAsync(restaurant);
    }
}