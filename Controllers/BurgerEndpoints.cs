using Microsoft.EntityFrameworkCore;
using API2.Data;
using API2.Data.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
namespace API2.Controllers;

public static class BurgerEndpoints
{
    public static void MapBurgerEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Burger").WithTags(nameof(Burger));

        group.MapGet("/", async (PoEjercicioBurger2Context db) =>
        {
            return await db.Burgers.ToListAsync();
        })
        .WithName("GetAllBurgers")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Burger>, NotFound>> (int burgerid, PoEjercicioBurger2Context db) =>
        {
            return await db.Burgers.AsNoTracking()
                .FirstOrDefaultAsync(model => model.BurgerId == burgerid)
                is Burger model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetBurgerById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int burgerid, Burger burger, PoEjercicioBurger2Context db) =>
        {
            var affected = await db.Burgers
                .Where(model => model.BurgerId == burgerid)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.BurgerId, burger.BurgerId)
                    .SetProperty(m => m.BurgerName, burger.BurgerName)
                    .SetProperty(m => m.WithCheese, burger.WithCheese)
                    .SetProperty(m => m.Price, burger.Price)
                    .SetProperty(m => m.PriceCeiling, burger.PriceCeiling)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateBurger")
        .WithOpenApi();

        group.MapPost("/", async (Burger burger, PoEjercicioBurger2Context db) =>
        {
            db.Burgers.Add(burger);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Burger/{burger.BurgerId}",burger);
        })
        .WithName("CreateBurger")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int burgerid, PoEjercicioBurger2Context db) =>
        {
            var affected = await db.Burgers
                .Where(model => model.BurgerId == burgerid)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteBurger")
        .WithOpenApi();
    }
}
