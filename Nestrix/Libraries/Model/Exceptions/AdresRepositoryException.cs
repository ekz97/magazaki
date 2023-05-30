namespace DataLayer.Exceptions;

public class AdresRepositoryException : Exception
{
    public AdresRepositoryException(string message) : base(message)
    {
    }
    
    public AdresRepositoryException(string message, Exception innerException) : base(message, innerException)
    {
    }
}