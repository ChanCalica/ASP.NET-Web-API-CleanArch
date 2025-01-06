using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Repositories;

public interface IRestaurantRepository
{
    Task<IEnumerable<Restaurant>> GetAllAsync();
    Task<Restaurant?> GetByIdAsync(int Id);
    Task<int> CreateAsync(Restaurant entity);
    Task DeleteAsync(Restaurant restaurant);
    Task SaveChanges();
}
