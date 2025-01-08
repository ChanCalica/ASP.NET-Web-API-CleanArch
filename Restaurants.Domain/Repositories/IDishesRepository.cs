﻿using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Repositories;

public interface IDishesRepository
{
    Task<int> CreateAsync(Dish entity);
    Task<IEnumerable<Dish>> GetDishesByRestaurantId();
}
