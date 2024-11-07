
using Marten.Linq.QueryHandlers;

namespace Catalog.API.Products.GetProduct
{
    public record GetProductQuery(int? PageNumber= 1, int? Pagesize = 10):IQuery<GetProductResult>;
    public record GetProductResult(IEnumerable<Product>Products);
    internal class GetProductQueryHandler  (IDocumentSession session )
        : IQueryHandler<GetProductQuery, GetProductResult>
    {
       public async Task<GetProductResult> Handle(GetProductQuery query, CancellationToken cancellationToken)
        {
 
            var products= await session.Query<Product>().ToPagedListAsync(query.PageNumber?? 1,query.Pagesize ??10 ,cancellationToken);   
            return new GetProductResult(products);  
        }
    }
}
