using Microsoft.EntityFrameworkCore;
using OptimisticConcurrencyExample.Models;
using OptimisticConcurrencyExample.Persistence;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<MealsDbContext>(c =>
{
    c.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    c.UseSeeding((context, b) =>
    {
        if (!context.Set<Meal>().Any())
        {
            context.Add(new Meal(){Name = "Tarte Flambée"});
            context.SaveChanges();
        }
    });
});
var app = builder.Build();

// Get all meals
app.MapGet("/", (MealsDbContext dbContext) => Results.Ok(dbContext.Set<Meal>()));

// Upsert meal
app.MapPost("/", async (MealsDbContext dbContext, Meal meal) =>
{
    dbContext.Set<Meal>().Update(meal);
    await dbContext.SaveChangesAsync();
    return Results.Created($"/{meal.Id}", meal);
});

// Get meal by id
app.MapGet("{id}", (MealsDbContext dbContext, Guid id) => Results.Ok(dbContext.Set<Meal>().Find(id)));


using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<MealsDbContext>();
dbContext.Database.EnsureDeleted();
dbContext.Database.EnsureCreated();

app.Run();