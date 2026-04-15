namespace Catalog.Endpoints;

public static class ProductEndpoint
{
    public static void MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/products").WithTags("Products");

        //GET /products
        group.MapGet("/", async (ProductService service) =>
        {
            var products = await service.GetProductsAsync();
            return Results.Ok(products);
        })
        .WithName("GetAllProducts")
        .Produces<List<Product>>(StatusCodes.Status200OK);


        //GET /products/{id}
        group.MapGet("/{id:int}", async (int id, ProductService service) =>
        {
            var product = await service.GetProductByIdAsync(id);
            return product is not null ? Results.Ok(product) : Results.NotFound();
        })
        .WithName("GetProductById")
        .Produces<Product>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        //POST /products
        group.MapPost("/", async (Product inputProduct, ProductService service) =>
        {
            await service.CreateProductAsync(inputProduct);
            return Results.CreatedAtRoute("GetProductById", new { id = inputProduct.Id }, inputProduct);
        })
        .WithName("CreateProduct")
        .Produces<Product>(StatusCodes.Status201Created);

        //PUT /products/{id}
        group.MapPut("/{id:int}", async (int id, Product inputProduct, ProductService service) =>
        {
            var existingProduct = await service.GetProductByIdAsync(id);
            if (existingProduct is null) return Results.NotFound();

            await service.UpdateProductAsync(existingProduct, inputProduct);
            return Results.NoContent();
        })
        .WithName("UpdateProduct")
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status204NoContent);

        //DELETE /products/{id}
        group.MapDelete("/{id:int}", async (int id, ProductService service) =>
        {
            var existingProduct = await service.GetProductByIdAsync(id);
            if (existingProduct is null) return Results.NotFound();
            await service.DeleteProductAsync(existingProduct);
            return Results.NoContent();
        })
        .WithName("DeleteProduct")
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status204NoContent);
    }
}
