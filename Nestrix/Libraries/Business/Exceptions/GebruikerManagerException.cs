namespace LogicLayer.Exceptions;

public class GebruikerManagerException : Exception
{
    public GebruikerManagerException(string message) : base(message)
    {
    }
    
    public GebruikerManagerException(string message, Exception innerException) : base(message, innerException)
    {
    }
}