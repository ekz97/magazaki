namespace DataLayer.Exceptions;

public class RekeningRepositoryException : Exception
{
    public RekeningRepositoryException(string message) : base(message)
    {
    }
    
    public RekeningRepositoryException(string message, Exception innerException) : base(message, innerException)
    {
    }
}