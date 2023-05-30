namespace DataLayer.Exceptions;

public class GebruikerRepositoryException : Exception
{
    public GebruikerRepositoryException(string message) : base(message)
    {
    }
    
    public GebruikerRepositoryException(string message, Exception innerException) : base(message, innerException)
    {
    }
}