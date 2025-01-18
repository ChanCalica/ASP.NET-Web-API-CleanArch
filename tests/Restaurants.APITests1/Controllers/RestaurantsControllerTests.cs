using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Restaurants.APITests1;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Restaurants.API.Controllers.Tests;

public class RestaurantsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly Mock<IRestaurantRepository> _restaurantsRepositoryMock = new();

    public RestaurantsControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                services.Replace(ServiceDescriptor.Scoped(typeof(IRestaurantRepository), _ => _restaurantsRepositoryMock.Object));
            });
        });
    }

    [Fact()]
    public async Task GetAll_ForValidRequest_Returns200Ok()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var results = await client.GetAsync("/api/Restaurants?pageNumber=1&pageSize=10");

        // Assert
        results.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact()]
    public async Task GetAll_ForInvalidRequest_Returns400BadRequest()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var results = await client.GetAsync("/api/Restaurants");

        // Assert
        results.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact()]
    public async Task GetAll_ForNonExistingId_ShouldReturn404NotFound()
    {
        // Arrange
        var id = 1123;

        _restaurantsRepositoryMock.Setup(m => m.GetByIdAsync(id)).ReturnsAsync((Restaurant?)null);

        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync($"/api/Restaurants/{id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact()]
    public async Task GetAll_ForExistingId_ShouldReturn200Ok()
    {
        // Arrange
        var id = 1123;

        var restaurant = new Restaurant
        {
            Id = id,
            Name = "Test Name",
            Description = "Test Desc"
        };

        _restaurantsRepositoryMock.Setup(m => m.GetByIdAsync(id)).ReturnsAsync(restaurant);

        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync($"/api/Restaurants/{id}");
        var restaurantDto = await response.Content.ReadFromJsonAsync<RestaurantDto>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        restaurantDto.Should().NotBeNull();
        restaurantDto.Name.Should().Be("Test Name");
        restaurantDto.Description.Should().Be("Test Desc");
    }
}