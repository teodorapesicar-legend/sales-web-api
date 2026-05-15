var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

var products = new List<Product>
{
    new Product(1, "Laptop", 999.99m, 10),
    new Product(2, "Mouse", 29.99m, 50),
    new Product(3, "Keyboard", 49.99m, 30)
};

app.MapGet("/products", () => products);

app.MapGet("/products/{id}", (int id) =>
{
    var product = products.FirstOrDefault(p => p.Id == id);
    return product is null ? Results.NotFound() : Results.Ok(product);
});

app.MapPost("/products", (Product product) =>
{
    products.Add(product);
    return Results.Created($"/products/{product.Id}", product);
});

app.MapPut("/products/{id}", (int id, Product updated) =>
{
    var product = products.FirstOrDefault(p => p.Id == id);
    if (product is null) return Results.NotFound();
    products.Remove(product);
    products.Add(updated);
    return Results.Ok(updated);
});

app.MapDelete("/products/{id}", (int id) =>
{
    var product = products.FirstOrDefault(p => p.Id == id);
    if (product is null) return Results.NotFound();
    products.Remove(product);
    return Results.NoContent();
});

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

app.Run();

record Product(int Id, string Name, decimal Price, int Stock);