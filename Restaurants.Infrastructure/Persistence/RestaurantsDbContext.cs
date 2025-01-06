using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;

namespace Restaurants.Infrastructure.Persistence;

//internal class RestaurantsDbContext(DbContextOptions<RestaurantsDbContext> options) : DbContext(options)
internal class RestaurantsDbContext(DbContextOptions<RestaurantsDbContext> options) : DbContext(options)
{

    //public RestaurantsDbContext(DbContextOptions<RestaurantsDbContext> options) : base(options)
    //{

    //}

    // add / to removed warning
    internal /*required*/ DbSet<Restaurant> Restaurants { get; set; }
    internal /*required*/ DbSet<Dish> Dishes { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    // add-migration Init
    //    // Remove-Migration
    //    // update-database
    //    optionsBuilder.UseSqlServer("");
    //}

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    base.OnConfiguring(optionsBuilder);
    //}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Restaurant>()
            .OwnsOne(r => r.Address);

        modelBuilder.Entity<Restaurant>()
            .HasMany(r => r.Dishes)
            .WithOne()
            .HasForeignKey(d => d.RestaurantId);
    }
}