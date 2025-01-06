using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant;

public class UpdateRestaurantCommandHandler(ILogger<UpdateRestaurantCommandHandler> logger, IMapper mapper,
    IRestaurantRepository restaurantRepository) : IRequestHandler<UpdateRestaurantCommand, bool>
{
    public async Task<bool> Handle(UpdateRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Updating restaurant wuth id : {request.Id}");

        var restaurant = await restaurantRepository.GetByIdAsync(request.Id);
        if (restaurant is null)
            return false;

        mapper.Map(request, restaurant);

        await restaurantRepository.SaveChanges();

        return true;
    }
}
