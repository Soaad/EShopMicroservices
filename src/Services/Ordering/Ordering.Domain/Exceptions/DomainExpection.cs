namespace Ordering.Domain.Exceptions;

public class DomainExpection:Exception
{
    public DomainExpection(string message):base ($"Domain Exception: \"{message}\" throws from domain layer ")
    {
        
    }
    
}