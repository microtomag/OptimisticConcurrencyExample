using Microsoft.EntityFrameworkCore;
using OptimisticConcurrencyExample.Models;

namespace OptimisticConcurrencyExample.Persistence;

public class MealsDbContext(DbContextOptions options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Meal>(meals =>
        {
            meals.ToTable("Meals");
            meals.Property(meal => meal.ConcurrencyToken)
                .IsConcurrencyToken()
                .ValueGeneratedOnAddOrUpdate();
        });
    }
}