namespace DataLayer.Exceptions;

public class BankRepositoryException : Exception
{
    public BankRepositoryException(string message) : base(message)
    {
    }
    
    public BankRepositoryException(string message, Exception innerException) : base(message, innerException)
    {
    }
}