



using Catalog.API.Data;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

//add services to the container

var assembly = typeof(Program).Assembly;
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));

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

if(builder.Environment.IsDevelopment())
{
    builder.Services.InitializeMartenWith<CatalogInitialData>();

}

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddHealthChecks().AddNpgSql( builder.Configuration.GetConnectionString("Database")!);
var app = builder.Build();
//app.UseExceptionHandler(exceptionHandlerApp =>
//{
//    exceptionHandlerApp.Run(async context =>
//    {
//        var execption = context.Features.Get<IExceptionHandlerFeature>()?.Error;

//        if (execption is null)
//            return;


//        var problemDetails = new ProblemDetails
//        {
//            Title = execption.Message,
//            Detail = execption.StackTrace,
//            Status = StatusCodes.Status500InternalServerError
//        };

//        var loger = context.RequestServices.GetRequiredService<ILogger<Program>>();
//        loger.LogError(execption, execption.Message);
//        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
//        context.Response.ContentType = "application/problem+json";
//        await context.Response.WriteAsJsonAsync(problemDetails);    
//    });
//});
//configure the http request pipline
app.MapCarter();
app.UseExceptionHandler(options => { });
app.UseHealthChecks("/health",new HealthCheckOptions
{
    ResponseWriter=UIResponseWriter.WriteHealthCheckUIResponse

});
app.Run();
