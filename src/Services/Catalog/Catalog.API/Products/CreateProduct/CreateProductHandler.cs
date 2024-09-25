


namespace Catalog.API.Products.CreateProduct
{
    public record CreateProductCommand(string Name, List<string> Category, string Description, string ImageFile, decimal Price)
           :ICommand<CreateProductResult>;
    public record CreateProductResult(Guid Id);

    public class CreateProductCommandValidator:AbstractValidator<CreateProductCommand>  
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.ImageFile).NotEmpty().WithMessage("ImageFile is required");
            RuleFor(x => x.Category).NotEmpty().WithMessage("Category is required");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be > 0");


        }

    }
    internal class CreateCommanProductHandler(IDocumentSession session,IValidator<CreateProductCommand> validator) 
                 : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            var res=await validator.ValidateAsync(command,cancellationToken);  
            
           var errors= res.Errors.Select(x=>x.ErrorMessage).ToList();
            if (errors.Any())
            {
                throw new ValidationException(errors.FirstOrDefault());
            }
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
            session.Store(product);
            session.SaveChangesAsync(cancellationToken);  
            //Return CreateProductResult result
            return new CreateProductResult(product.Id);
        }
    }
}                                                 
                                       