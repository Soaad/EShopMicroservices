var builder = WebApplication.CreateBuilder(args);

//Add services to the container

var app = builder.Build();

//Configure http request pipline

app.Run();
