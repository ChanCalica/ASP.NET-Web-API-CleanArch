using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Repositories;

public interface IRestaurantRepository
{
    Task<IEnumerable<Restaurant>> GetAllAsync();
    Task<(IEnumerable<Restaurant>, int)> GetAllMatchingAsync(string? searchPhrase,
        int pageNumber, int pageSize, string? sortBy, SortDirection sortDirection);
    Task<Restaurant?> GetByIdAsync(int Id);
    Task<int> CreateAsync(Restaurant entity);
    Task DeleteAsync(Restaurant restaurant);
    Task SaveChanges();
}
