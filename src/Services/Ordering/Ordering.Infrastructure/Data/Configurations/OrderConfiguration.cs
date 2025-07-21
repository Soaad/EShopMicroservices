using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Enums;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;

namespace Ordering.Infrastructure.Data.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);
        builder.Property(oi => oi.Id).HasConversion(orderId => orderId.Value, dbId => OrderId.Of(dbId));
        builder.HasOne<Customer>().WithMany().HasForeignKey(oi => oi.CustomerId).IsRequired();

        builder.HasMany<OrderItem>().WithOne().HasForeignKey(oi => oi.OrderId);
        builder.ComplexProperty(o => o.OrderName,
            orderBuilder => { orderBuilder.Property(n => n.Value).HasColumnName(nameof(Order.OrderName)); });

        builder.ComplexProperty(o => o.ShippigAddress, addBuilder =>
        {
            addBuilder.Property(n => n.FirstName).HasMaxLength(50).IsRequired();
            addBuilder.Property(n => n.LastName).HasMaxLength(50).IsRequired();
            addBuilder.Property(n => n.EmailAddress).IsRequired();
            addBuilder.Property(n => n.AddressLine).HasMaxLength(50).IsRequired();
            addBuilder.Property(n => n.Country).HasMaxLength(50);
            addBuilder.Property(n => n.State).HasMaxLength(50);
            addBuilder.Property(n => n.ZipCode).HasMaxLength(5).IsRequired();
        });

        builder.ComplexProperty(o => o.BillingAddress, billBuilder =>
        {
            billBuilder.Property(n => n.FirstName).HasMaxLength(50).IsRequired();
            billBuilder.Property(n => n.LastName).HasMaxLength(50).IsRequired();
            billBuilder.Property(n => n.EmailAddress).IsRequired();
            billBuilder.Property(n => n.AddressLine).HasMaxLength(50).IsRequired();
            billBuilder.Property(n => n.Country).HasMaxLength(50);
            billBuilder.Property(n => n.State).HasMaxLength(50);
            billBuilder.Property(n => n.ZipCode).HasMaxLength(5).IsRequired();
        });


        builder.ComplexProperty(o => o.Payment, paymentBuilder =>
        {
            paymentBuilder.Property(n => n.CardName).HasMaxLength(50);
            paymentBuilder.Property(n => n.CardNumber).HasMaxLength(24).IsRequired();
            paymentBuilder.Property(n => n.Expiration).HasMaxLength(10);
            paymentBuilder.Property(n => n.CVV).HasMaxLength(3).IsRequired();
            paymentBuilder.Property(n => n.PaymentMethod);

        });

        builder.Property(s => s.Status).HasDefaultValue(OrderStatus.Draft).HasConversion(s=>s.ToString(),dbStatus=>(OrderStatus)Enum.Parse(typeof(OrderStatus),dbStatus));
    }
}