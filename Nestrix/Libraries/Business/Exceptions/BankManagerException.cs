namespace LogicLayer.Exceptions;

public class BankManagerException : Exception
{
    public BankManagerException(string message) : base(message)
    {
    }
    
    public BankManagerException(string message, Exception innerException) : base(message, innerException)
    {
    }
}