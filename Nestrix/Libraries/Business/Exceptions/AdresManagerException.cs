namespace LogicLayer.Exceptions;

public class AdresManagerException : Exception
{
    public AdresManagerException(string message) : base(message)
    {
    }
    
    public AdresManagerException(string message, Exception innerException) : base(message, innerException)
    {
    }
}