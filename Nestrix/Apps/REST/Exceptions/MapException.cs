namespace RESTLayer.Exceptions;

public class MapException : Exception
{
    public MapException(string message) : base(message)
    {
        
    }
    
    public MapException(string message, Exception innerException) : base(message, innerException)
    {
        
    }
}