namespace Ordering.Domain.ValueObjects;

public record OrderName
{
    public string Value { get; }
    private const int DefaultLength = 50;

    private OrderName(string value)=>Value=value;
    
    public static OrderName Of(string value)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(value);
        ArgumentOutOfRangeException.ThrowIfNotEqual(value.Length,DefaultLength);
        return new OrderName(value);
    }
    
}