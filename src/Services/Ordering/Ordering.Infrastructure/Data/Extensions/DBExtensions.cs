using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Ordering.Infrastructure.Data.Extensions;

public static class DBExtensions
{
    public static async Task InitializeDataBaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context=scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        context.Database.MigrateAsync().GetAwaiter().GetResult();
        await SeedAsync(context);
    }

    private static async Task SeedAsync(ApplicationDbContext context)
    {
        await SeedCustomerAsync(context);
        await SeedProductsAsync(context);
        await SeedOrdersAndItemsAsync(context);

        
    }

    private static async Task SeedCustomerAsync(ApplicationDbContext context)
    {
        if (!await context.Customers.AnyAsync())
        {
            await context.Customers.AddRangeAsync(InitalData.Customers);
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedProductsAsync(ApplicationDbContext context)
    {
        if (!await context.Products.AnyAsync())
        {
            await context.Products.AddRangeAsync(InitalData.Products);
            await context.SaveChangesAsync();
        }
        
        
    }
    private static Dictionary<ProductId, Product> _productCache = InitalData.Products.ToDictionary(p => p.Id);

    private static async Task SeedOrdersAndItemsAsync(ApplicationDbContext context)
    {
        if (!await context.Orders.AnyAsync())
        {
            await context.Orders.AddRangeAsync(InitalData.OrdersWithItems);
            await context.SaveChangesAsync();
        }
        
        
    }
}