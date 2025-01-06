using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Repositories;

internal class RestaurantsRepository(RestaurantsDbContext dbContext) : IRestaurantRepository
{
    //private readonly RestaurantsDbContext _dbContext;

    //internal RestaurantsRepository(RestaurantsDbContext dbContext)
    //{
    //    _dbContext = dbContext;
    //}

    public async Task<IEnumerable<Restaurant>> GetAllAsync()
    {
        var restaurants = await dbContext.Restaurants.ToListAsync();

        return restaurants;
    }

    public async Task<Restaurant?> GetByIdAsync(int Id)
    {
        var restaurant = await dbContext.Restaurants
            .Include(x => x.Dishes)
            .FirstOrDefaultAsync(r => r.Id.Equals(Id));

        return restaurant;
    }

    public async Task<int> CreateAsync(Restaurant restaurant)
    {
        dbContext.Restaurants.Add(restaurant);
        await dbContext.SaveChangesAsync();

        return restaurant.Id;
    }

    public async Task DeleteAsync(Restaurant restaurant)
    {
        dbContext.Remove(restaurant);
        await dbContext.SaveChangesAsync();
    }
}
