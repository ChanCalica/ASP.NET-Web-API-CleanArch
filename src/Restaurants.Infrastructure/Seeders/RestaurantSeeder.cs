using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Seeders;

internal class RestaurantSeeder(RestaurantsDbContext dbContext) : IRestaurantSeeder
{
    public async Task Seed()
    {
        if (dbContext.Database.GetPendingMigrations().Any())
        {
            await dbContext.Database.MigrateAsync();
        }

        if (await dbContext.Database.CanConnectAsync())
        {
            if (!dbContext.Restaurants.Any())
            {
                var restaurants = GetRestaurants();
                dbContext.Restaurants.AddRange(restaurants);
                await dbContext.SaveChangesAsync();
            }

            if (!dbContext.Roles.Any())
            {
                var roles = GetRoles();
                dbContext.Roles.AddRange(roles);
                await dbContext.SaveChangesAsync();
            }
        }
    }

    private IEnumerable<IdentityRole> GetRoles()
    {
        List<IdentityRole> roles =
            [
            new (UserRoles.User)
            {
                NormalizedName = UserRoles.User.ToUpper()
            },
            new (UserRoles.Owner)
            {
                NormalizedName = UserRoles.Owner.ToUpper()
            },
            new (UserRoles.Admin)
            {
                NormalizedName = UserRoles.Admin.ToUpper()
            }
            ];

        return roles;
    }

    private IEnumerable<Restaurant> GetRestaurants()
    {
        User owner = new User()
        {
            Email = "seed-user@test.com"
        };

        List<Restaurant> restaurants = [
            new(){
                Owner = owner,
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
                    PostalCode = "41-141"
                }
            },
            new Restaurant()
            {
                Owner = owner,
                Name = "McDonald",
                Category = "Fast Food",
                Description = "McDo",
                ContactEmail = "mcdo@gmail.com",
                HasDelivery = true,
                Address = new Address()
                {
                    City = "Cavite",
                    Street = "Street 1",
                    PostalCode = "12-345"
                }
            }
        ];

        return restaurants;
    }
}