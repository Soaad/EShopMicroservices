var builder = WebApplication.CreateBuilder(args);

//add services to the container

builder.Services.AddCarter();

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(typeof(Program).Assembly);
});

builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();
var app = builder.Build();

//configure the http request pipline
app.MapCarter();

app.Run();
