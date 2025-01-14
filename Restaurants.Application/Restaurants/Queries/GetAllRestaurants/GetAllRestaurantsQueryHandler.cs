using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Common;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants;

public class GetAllRestaurantsQueryHandler(ILogger<GetAllRestaurantsQueryHandler> logger,
    IMapper mapper,
    IRestaurantRepository restaurantRepository) : IRequestHandler<GetAllRestaurantsQuery, PagedResult<RestaurantDto>>
{
    public async Task<PagedResult<RestaurantDto>> Handle(GetAllRestaurantsQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting all restaurants");

        var (restaurants, totalCount) = await restaurantRepository.GetAllMatchingAsync(request.SearchPhrase,
            request.PageNumber, request.PageSize, request.SortBy, request.SortDirection);

        var restaurantsDtos = mapper.Map<IEnumerable<RestaurantDto>>(restaurants);

        var results = new PagedResult<RestaurantDto>(restaurantsDtos, totalCount, request.PageSize, request.PageNumber);

        return results;
    }
}
