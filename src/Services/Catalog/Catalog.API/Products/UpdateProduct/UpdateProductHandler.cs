using Catalog.API.Products.CreateProduct;

namespace Catalog.API.Products.UpdateProduct
{
    public record UpdateProductCommand(Guid Id,string Name, List<string> Category, string Description, string ImageFile, decimal Price)
           : ICommand<UpdateProductCommandResult>;

    public record UpdateProductCommandResult(bool IsSuccess);
    internal class UpdateProductCommandHandler(IDocumentSession session,ILogger<UpdateProductCommandHandler> logger) : ICommandHandler<UpdateProductCommand, UpdateProductCommandResult>
    {
        public async Task<UpdateProductCommandResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation("UpdateProductCommandResult.handel is called with {@command}", command);
           var product=await session.LoadAsync<Product>(command.Id, cancellationToken);

            if (product is null) throw  new ProductNotFoundException();
            product.Name = command.Name;    
            product.Category = command.Category;    
            product.Description = command.Description;  
            product.ImageFile = command.ImageFile;  
            product.Price = command.Price;
            session.Update(product);
           await  session.SaveChangesAsync(cancellationToken); 
            
            return new UpdateProductCommandResult(true);    
        }
    }
}
