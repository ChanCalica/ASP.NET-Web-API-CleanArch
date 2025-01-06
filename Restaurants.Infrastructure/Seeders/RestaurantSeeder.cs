using Restaurants.Domain.Entities;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Seeders;

internal class RestaurantSeeder(RestaurantsDbContext dbContext) : IRestaurantSeeder
{
    public async Task Seed()
    {
        if (await dbContext.Database.CanConnectAsync())
        {
            if (!dbContext.Restaurants.Any())
            {
                var restaurants = GetRestaurants();
                dbContext.Restaurants.AddRange(restaurants);
                await dbContext.SaveChangesAsync();
            }
        }
    }

    private IEnumerable<Restaurant> GetRestaurants()
    {
        List<Restaurant> restaurants = [
            new(){
                Name = "KFC",
                Category = "Fast Food",
                Description = "KFC",
                ContactEmail = "kfc@gmail.com",
                HasDelivery = true,
                Dishes = [
                    new()
                    {
                        Name = "Chicken",
                        Description = "Fried Chicken",
                        Price = 10.30M
                    },
                    new()
                    {
                        Name = "Chicken",
                        Description = "Fried Chicken",
                        Price = 5.30M
                    }
                    ],
                Address = new()
                {
                    City = "Val",
                    Street = "Street 2",
                    PostalCode = "4114"
                }
            },
            new Restaurant()
            {
                Name = "McDonald",
                Category = "Fast Food",
                Description = "McDo",
                ContactEmail = "mcdo@gmail.com",
                HasDelivery = true,
                Address = new Address()
                {
                    City = "Cavite",
                    Street = "Street 1",
                    PostalCode = "1234"
                }
            }
        ];

        return restaurants;
    }
}
