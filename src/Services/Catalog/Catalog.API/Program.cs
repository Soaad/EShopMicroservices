

using Catalog.API.Products.CreateProduct;

var builder = WebApplication.CreateBuilder(args);

//add services to the container

builder.Services.AddCarter();

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(typeof(Program).Assembly);
});

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
//builder.Services.AddValidatorsFromAssemblyContaining<CreateProductCommandValidator>();
var serviceProvider = builder.Services.BuildServiceProvider();
var validator = serviceProvider.GetService<IValidator<CreateProductCommand>>();
if (validator == null)
{
    throw new Exception("CreateProductCommandValidator is not registered.");
}
builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();
var app = builder.Build();

//configure the http request pipline
app.MapCarter();

app.Run();
