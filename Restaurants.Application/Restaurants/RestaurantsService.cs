//using AutoMapper;
//using Microsoft.Extensions.Logging;
//using Restaurants.Application.Restaurants.Dtos;
//using Restaurants.Domain.Repositories;

//namespace Restaurants.Application.Restaurants;

//internal class RestaurantsService(IRestaurantRepository restaurantRepository, ILogger<RestaurantsService> logger, IMapper mapper) : IRestaurantsService
//{
//    public async Task<IEnumerable<RestaurantDto>> GetAllRestaurants()
//    {
//        logger.LogInformation("Getting all restaurants");

//        var restaurants = await restaurantRepository.GetAllAsync();

//        //var restaurantsDtos = restaurants.Select(RestaurantDto.FromEntity);

//        var restaurantsDtos = mapper.Map<IEnumerable<RestaurantDto>>(restaurants);

//        return restaurantsDtos!;
//    }

//    public async Task<RestaurantDto?> GetRestaurant(int Id)
//    {
//        logger.LogInformation($"Getting restaurant {Id}");

//        var restaurant = await restaurantRepository.GetByIdAsync(Id);

//        //var restaurantDto = RestaurantDto.FromEntity(restaurant);

//        var restaurantDto = mapper.Map<RestaurantDto>(restaurant);

//        return restaurantDto;
//    }

//    //public async Task<int> CreateRestaurant(CreateRestaurantDto createRestaurantDto)
//    //{
//    //    logger.LogInformation("Creating a new restaurant");

//    //    var restaurant = mapper.Map<Restaurant>(createRestaurantDto);

//    //    int id = await restaurantRepository.CreateAsync(restaurant);

//    //    return id;
//    //}
//}
