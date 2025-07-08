using System.Reflection;

namespace Ordering.API;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {

      //  services.AddMediatR(cfg =>
        //{ cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());});
        return services;
    }

    public static WebApplication UseApiServices(this IApplicationBuilder app)
    {
       // app.MapCarter();
        return (WebApplication)app;
    }
}