using Microsoft.EntityFrameworkCore;
using Azure.Identity;

var builder = WebApplication.CreateBuilder(args);

var keyVaultUri = new Uri("https://kv-sales-api-neu-001.vault.azure.net/");

builder.Configuration.AddAzureKeyVault(keyVaultUri, new DefaultAzureCredential());

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.EnableRetryOnFailure()));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
    if (!db.Products.Any())
    {
        db.Products.AddRange(
            new Product { Name = "Laptop", Price = 999.99m, Stock = 10 },
            new Product { Name = "Mouse", Price = 29.99m, Stock = 50 },
            new Product { Name = "Keyboard", Price = 49.99m, Stock = 30 }
        );
        db.SaveChanges();
    }
}

app.MapGet("/", () => Results.Content("""
<!DOCTYPE html>
<html>
<head>
    <title>Sales API</title>
    <style>
        body { font-family: Arial, sans-serif; padding: 20px; }
        table { border-collapse: collapse; width: 100%; }
        th, td { border: 1px solid #ddd; padding: 8px; text-align: left; }
        th { background-color: #4CAF50; color: white; }
        tr:nth-child(even) { background-color: #f2f2f2; }
    </style>
</head>
<body>
    <h1>Products</h1>
    <table id="table">
        <tr><th>ID</th><th>Name</th><th>Price</th><th>Stock</th></tr>
    </table>
    <script>
        fetch('/products')
            .then(r => r.json())
            .then(data => {
                const table = document.getElementById('table');
                data.forEach(p => {
                    table.innerHTML += `<tr><td>${p.id}</td><td>${p.name}</td><td>${p.price}</td><td>${p.stock}</td></tr>`;
                });
            });
    </script>
</body>
</html>
""", "text/html"));

app.MapGet("/products", async (AppDbContext db) => await db.Products.ToListAsync());

app.MapGet("/products/{id}", async (int id, AppDbContext db) =>
{
    var product = await db.Products.FindAsync(id);
    return product is null ? Results.NotFound() : Results.Ok(product);
});

app.MapPost("/products", async (Product product, AppDbContext db) =>
{
    db.Products.Add(product);
    await db.SaveChangesAsync();
    return Results.Created($"/products/{product.Id}", product);
});

app.MapPut("/products/{id}", async (int id, Product updated, AppDbContext db) =>
{
    var product = await db.Products.FindAsync(id);
    if (product is null) return Results.NotFound();
    product.Name = updated.Name;
    product.Price = updated.Price;
    product.Stock = updated.Stock;
    return Results.Ok(product);
});

app.MapDelete("/products/{id}", async (int id, AppDbContext db) =>
{
    var product = await db.Products.FindAsync(id);
    if (product is null) return Results.NotFound();
    db.Products.Remove(product);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.Run();

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
}