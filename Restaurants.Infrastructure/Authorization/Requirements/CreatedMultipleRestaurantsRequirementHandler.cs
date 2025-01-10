using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Repositories;

namespace Restaurants.Infrastructure.Authorization.Requirements;

public class CreatedMultipleRestaurantsRequirementHandler(ILogger<CreatedMultipleRestaurantsRequirementHandler> logger,
    IRestaurantRepository restaurantRepository,
    IUserContext userContext) : AuthorizationHandler<CreatedMultipleRestaurantsRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, CreatedMultipleRestaurantsRequirement requirement)
    {
        var currentUser = userContext.GetCurrentUser();

        var restaurants = await restaurantRepository.GetAllAsync();

        var userRestaurantsCreated = restaurants.Count(r => r.OwnerId == currentUser!.Id);

        if (userRestaurantsCreated >= requirement.MinimumRestaurantsCreated)
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }
    }
}
