
using Catalog.API.Products.CreateProduct;

namespace Catalog.API.Products.GetProduct
{
    public record GetProductResponse(IEnumerable<Product>Products);
    public class GetProductEndpointL : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products", async (ISender sender) => {
                var result = await sender.Send(new GetProductQuery());
                var response = result.Adapt<GetProductResponse>();

                return Results.Ok(response);
            }).WithName("GetProducts")
            .Produces<GetProductResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithDescription("Get Products")
            .WithSummary("Get Products");
        }
    }
}
