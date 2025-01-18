using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistence;
using System.Linq.Expressions;

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

    public async Task<(IEnumerable<Restaurant>, int)> GetAllMatchingAsync(string? searchPhrase, int pageNumber, int pageSize,
        string? sortBy, SortDirection sortDirection)
    {
        var searchPhraseLower = searchPhrase?.ToLower();

        var baseQuery = dbContext.Restaurants
            .Where(r => searchPhraseLower == null ||
            (r.Name.ToLower().Contains(searchPhraseLower) || r.Description.ToLower().Contains(searchPhraseLower)));

        var totalCount = await baseQuery.CountAsync();

        if (sortBy != null)
        {
            var columnsSelector = new Dictionary<string, Expression<Func<Restaurant, object>>>
            {
                {nameof(Restaurant.Name), r => r.Name },
                {nameof(Restaurant.Description), r => r.Description },
                {nameof(Restaurant.Category), r => r.Category }
            };

            var selectedColumn = columnsSelector[sortBy];
            //baseQuery = baseQuery.OrderBy(r => r.Description);
            baseQuery = sortDirection == SortDirection.Ascending
                ? baseQuery.OrderBy(selectedColumn)
                : baseQuery.OrderByDescending(selectedColumn);
        }

        var restaurants = await baseQuery
            .Skip(pageSize * (pageNumber - 1))
            .Take(pageSize)
            .ToListAsync();

        return (restaurants, totalCount);
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
        //dbContext.Restaurants.Remove(restaurant);
        await dbContext.SaveChangesAsync();
    }

    public Task SaveChanges() =>
        dbContext.SaveChangesAsync();
}
