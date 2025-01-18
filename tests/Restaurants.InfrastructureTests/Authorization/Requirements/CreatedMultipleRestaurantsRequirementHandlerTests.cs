using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.Users;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Xunit;

namespace Restaurants.Infrastructure.Authorization.Requirements.Tests;

public class CreatedMultipleRestaurantsRequirementHandlerTests
{
    [Fact()]
    public async Task HandleRequirementAsync_UserHasNotCreatedMultipleRestaurants_ShouldFail()
    {
        // Arrange
        //var loggerMock = new Mock<ILogger<CreatedMultipleRestaurantsRequirementHandler>>();
        var currentUser = new CurrentUser("1", "test@test.com", [], null, null);
        var userContextMock = new Mock<IUserContext>();
        userContextMock.Setup(s => s.GetCurrentUser()).Returns(currentUser);

        var restaurants = new List<Restaurant>()
        {
            new()
            {
                OwnerId = currentUser.Id
            },
            new()
            {
                OwnerId = "2"
            }
        };

        var restaurantsRepositoryMock = new Mock<IRestaurantRepository>();
        restaurantsRepositoryMock.Setup(s => s.GetAllAsync()).ReturnsAsync(restaurants);

        var requirement = new CreatedMultipleRestaurantsRequirement(2);
        var handler = new CreatedMultipleRestaurantsRequirementHandler(/*loggerMock.Object,*/ restaurantsRepositoryMock.Object, userContextMock.Object);
        var context = new AuthorizationHandlerContext([requirement], null, null);

        // Act
        await handler.HandleAsync(context);

        // Assert
        context.HasSucceeded.Should().BeFalse();
        context.HasFailed.Should().BeTrue();
    }

    [Fact()]
    public async Task HandleRequirementAsync_UserHasCreatedMultipleRestaurants_ShouldSucceed()
    {
        // Arrange
        //var loggerMock = new Mock<ILogger<CreatedMultipleRestaurantsRequirementHandler>>();
        var currentUser = new CurrentUser("1", "test@test.com", [], null, null);
        var userContextMock = new Mock<IUserContext>();
        userContextMock.Setup(s => s.GetCurrentUser()).Returns(currentUser);

        var restaurants = new List<Restaurant>()
        {
            new()
            {
                OwnerId = currentUser.Id
            },
            new()
            {
                OwnerId = currentUser.Id
            },
            new()
            {
                OwnerId = "2"
            }
        };

        var restaurantsRepositoryMock = new Mock<IRestaurantRepository>();
        restaurantsRepositoryMock.Setup(s => s.GetAllAsync()).ReturnsAsync(restaurants);

        var requirement = new CreatedMultipleRestaurantsRequirement(2);
        var handler = new CreatedMultipleRestaurantsRequirementHandler(/*loggerMock.Object,*/ restaurantsRepositoryMock.Object, userContextMock.Object);
        var context = new AuthorizationHandlerContext([requirement], null, null);

        // Act
        await handler.HandleAsync(context);

        // Assert
        context.HasSucceeded.Should().BeTrue();
    }
}