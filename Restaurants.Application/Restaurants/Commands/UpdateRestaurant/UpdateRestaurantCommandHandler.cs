using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant;

public class UpdateRestaurantCommandHandler(ILogger<UpdateRestaurantCommandHandler> logger, IMapper mapper,
    IRestaurantRepository restaurantRepository) : IRequestHandler<UpdateRestaurantCommand>
{
    public async Task Handle(UpdateRestaurantCommand request, CancellationToken cancellationToken)
    {
        //logger.LogInformation($"Updating restaurant with id : {request.Id}");
        logger.LogInformation("Updating restaurant with id: {RestaurantId} with {@UpdatedRestaurant}", request.Id, request);

        var restaurant = await restaurantRepository.GetByIdAsync(request.Id);
        if (restaurant is null)
            throw new NotFoundException(nameof(Restaurant), request.Id.ToString());
        //return false;

        mapper.Map(request, restaurant);

        await restaurantRepository.SaveChanges();
    }
}
