using HouseholdApp.Client.Models;
using HouseholdApp.Client.Services;
using HouseholdApp.Components;
using HouseholdApp.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Default")));

// Prerendering runs these components on the server before WebAssembly takes
// over, so the same services the Client project registers must exist here too.
builder.Services.AddScoped(sp =>
{
    var navigationManager = sp.GetRequiredService<NavigationManager>();
    return new HttpClient { BaseAddress = new Uri(navigationManager.BaseUri) };
});
builder.Services.AddScoped<TransactionApiClient>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(HouseholdApp.Client._Imports).Assembly);

var api = app.MapGroup("/api/transactions");

api.MapGet("/", async (AppDbContext db) =>
    await db.Transactions
        .OrderBy(t => t.Date)
        .ThenBy(t => t.Id)
        .ToListAsync());

api.MapPost("/", async (TransactionItem input, AppDbContext db) =>
{
    var transaction = new TransactionItem
    {
        Date = input.Date,
        Category = input.Category,
        Amount = input.Amount,
        Type = input.Type
    };
    db.Transactions.Add(transaction);
    await db.SaveChangesAsync();
    return Results.Created($"/api/transactions/{transaction.Id}", transaction);
});

api.MapGet("/{id:int}", async (int id, AppDbContext db) =>
{
    var transaction = await db.Transactions.FindAsync(id);
    return transaction is null ? Results.NotFound() : Results.Ok(transaction);
});

api.MapPut("/{id:int}", async (int id, TransactionItem input, AppDbContext db) =>
{
    var transaction = await db.Transactions.FindAsync(id);
    if (transaction is null)
    {
        return Results.NotFound();
    }

    transaction.Date = input.Date;
    transaction.Category = input.Category;
    transaction.Amount = input.Amount;
    transaction.Type = input.Type;
    await db.SaveChangesAsync();
    return Results.NoContent();
});

api.MapDelete("/{id:int}", async (int id, AppDbContext db) =>
{
    var transaction = await db.Transactions.FindAsync(id);
    if (transaction is null)
    {
        return Results.NotFound();
    }

    db.Transactions.Remove(transaction);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.Run();
