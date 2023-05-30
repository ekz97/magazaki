namespace DataLayer.Exceptions;

public class TransactieRepositoryException : Exception
{
    public TransactieRepositoryException(string message) : base(message)
    {
    }
    
    public TransactieRepositoryException(string message, Exception innerException) : base(message, innerException)
    {
    }
}