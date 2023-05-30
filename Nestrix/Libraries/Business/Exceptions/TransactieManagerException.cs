namespace LogicLayer.Exceptions;

public class TransactieManagerException : Exception
{
    public TransactieManagerException(string message) : base(message)
    {
    }
    
    public TransactieManagerException(string message, Exception innerException) : base(message, innerException)
    {
    }
}