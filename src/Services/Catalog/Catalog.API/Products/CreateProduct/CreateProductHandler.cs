using BuildingBlocks.CQRS;
using Catalog.API.Models;
 
namespace Catalog.API.Products.CreateProduct
{
    public record CreateProductCommand(string Name, List<string> Category, string Description, string ImageFile, decimal Price)
           :ICommand<CreateProductResult>;
    public record CreateProductResult(Guid Id);
    internal class CreateCommanProductHandler : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            //Create product entity from command object
            var product = new Product
            {
                Name = command.Name,
                ImageFile = command.ImageFile,
                Price = command.Price,
                Category = command.Category,
                Description = command.Description,

            };

            //Save to DB

            //Return CreateProductResult result
            return new CreateProductResult(Guid.NewGuid());
        }
    }
}                                                 
                                       