
using Catalog.API.Products.CreateProduct;

namespace Catalog.API.Products.GetProduct
{
    public record GetProductResponse(IEnumerable<Product>Products);
    public record GetProductRequest(int? PageNumber=1,int? Pagesize=10);
    public class GetProductEndpointL : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products", async  ([AsParameters] GetProductRequest request, ISender sender) => {

                var query=request.Adapt<GetProductQuery>(); 
                var result = await sender.Send(query);
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
