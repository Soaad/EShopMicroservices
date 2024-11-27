

var builder = WebApplication.CreateBuilder(args);

//Add services to container
builder.Services.AddCarter();
var assembly=typeof(Program).Assembly;  
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
    opts.Schema.For<ShoppingCart>().Identity(x=>x.UserName);    
 }).UseLightweightSessions();

builder.Services.AddScoped<IBasketRepository,BasketRepository>();   
var app = builder.Build();


//Configure Http Request Pipline

app.MapCarter();

app.Run();
