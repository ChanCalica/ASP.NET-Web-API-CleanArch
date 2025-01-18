using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Repositories;

internal class DishesRepository(RestaurantsDbContext dbContext) : IDishesRepository
{
    public async Task<int> CreateAsync(Dish entity)
    {
        dbContext.Dishes.Add(entity);
        await dbContext.SaveChangesAsync();

        return entity.Id;
    }

    public async Task Delete(IEnumerable<Dish> entities)
    {
        dbContext.Dishes.RemoveRange(entities);

        await dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Dish>> GetAllDishes()
    {
        var dishes = await dbContext.Dishes.ToListAsync();

        return dishes;
    }

    public async Task<IEnumerable<Dish>> GetDishesByRestaurantId()
    {
        int restaurantId = 1;

        var dishes = await dbContext.Dishes.Where(d => d.RestaurantId.Equals(restaurantId)).ToListAsync();

        var dishes2 = await dbContext.Dishes.Select(d => new Dish
        {
            Id = d.Id,
            Name = d.Name,
            Description = d.Description,
            Price = d.Price,
            KiloCalories = d.KiloCalories,
            RestaurantId = d.RestaurantId
        }).ToListAsync();

        return dishes;
    }
}
