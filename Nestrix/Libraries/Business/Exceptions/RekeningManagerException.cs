namespace LogicLayer.Exceptions;

public class RekeningManagerException : Exception
{
    public RekeningManagerException(string message) : base(message)
    {
    }
    
    public RekeningManagerException(string message, Exception innerException) : base(message, innerException)
    {
    }
}