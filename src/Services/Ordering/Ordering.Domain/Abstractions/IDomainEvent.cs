using MediatR;

namespace Ordering.Domain.Abstractions;

public interface IDomainEvent:INotification
{
    Guid EventId => Guid.NewGuid();
    public DateTime OccurredOn =>DateTime.Now;
    public String EventType => GetType().AssemblyQualifiedName;

}