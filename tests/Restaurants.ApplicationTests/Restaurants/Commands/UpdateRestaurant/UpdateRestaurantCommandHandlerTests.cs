using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;
using Xunit;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant.Tests;

public class UpdateRestaurantCommandHandlerTests
{
    private readonly Mock<ILogger<UpdateRestaurantCommandHandler>> _loggerMock;
    private readonly Mock<IRestaurantRepository> _restaurantRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IRestaurantAuthorizationService> _restaurantAuthorizationService;

    private readonly UpdateRestaurantCommandHandler _handler;

    public UpdateRestaurantCommandHandlerTests()
    {
        _loggerMock = new Mock<ILogger<UpdateRestaurantCommandHandler>>();
        _mapperMock = new Mock<IMapper>();
        _restaurantRepositoryMock = new Mock<IRestaurantRepository>();
        _restaurantAuthorizationService = new Mock<IRestaurantAuthorizationService>();
        _handler = new UpdateRestaurantCommandHandler(_loggerMock.Object,
            _mapperMock.Object, _restaurantRepositoryMock.Object, _restaurantAuthorizationService.Object);
    }

    [Fact()]
    public async Task Handle_WithValidRequest_ShouldUpdateRestaurant()
    {
        // Arrange
        var restaurantId = 1;
        var command = new UpdateRestaurantCommand()
        {
            Id = restaurantId,
            Name = "Test Name",
            Description = "Test Description",
            HasDelivery = true
        };

        var restaurant = new Restaurant()
        {
            Id = restaurantId,
            Name = "Test",
            Description = "Test",
        };

        _restaurantRepositoryMock.Setup(s => s.GetByIdAsync(restaurantId)).ReturnsAsync(restaurant);

        _restaurantAuthorizationService.Setup(s => s.Authorize(restaurant, Domain.Constants.ResourceOperation.Update)).Returns(true);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _restaurantRepositoryMock.Verify(v => v.SaveChanges(), Times.Once);

        _mapperMock.Verify(v => v.Map(command, restaurant), Times.Once);
    }

    [Fact]
    public async Task Handle_WithNonExistingRestaurant_ShouldThrowNotFoundException()
    {
        // Arrange
        var restaurantId = 2;

        var command = new UpdateRestaurantCommand() { Id = restaurantId };

        _restaurantRepositoryMock.Setup(s => s.GetByIdAsync(restaurantId)).ReturnsAsync((Restaurant?)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>().WithMessage($"Restaurant with id: {restaurantId} doesn't exist");
    }

    [Fact]
    public async Task Handle_WithUnauthorizedUser_ShouldThrowForbidException()
    {
        // Arrange
        var restaurantId = 3;
        var command = new UpdateRestaurantCommand
        {
            Id = restaurantId
        };

        var existingRestaurant = new Restaurant
        {
            Id = restaurantId
        };

        _restaurantRepositoryMock.Setup(s => s.GetByIdAsync(restaurantId)).ReturnsAsync(existingRestaurant);

        _restaurantAuthorizationService.Setup(s => s.Authorize(existingRestaurant, ResourceOperation.Update)).Returns(false);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Asset

        await act.Should().ThrowAsync<ForbidException>();
    }
}