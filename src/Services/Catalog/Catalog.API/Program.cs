
using BuildingBlocks.Behaviors;
using Catalog.API.Products.CreateProduct;

var builder = WebApplication.CreateBuilder(args);

//add services to the container

var assembly = typeof(Program).Assembly;
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddCarter();

//var serviceProvider = builder.Services.BuildServiceProvider();
//var validator = serviceProvider.GetService<IValidator<CreateProductCommand>>();
//if (validator == null)
//{
//    throw new Exception("CreateProductCommandValidator is not registered.");
//}

builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();
var app = builder.Build();

//configure the http request pipline
app.MapCarter();

app.Run();
