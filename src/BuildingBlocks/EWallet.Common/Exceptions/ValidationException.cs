namespace EWallet.Common.Exceptions;

public class ValidationException : System.Exception
{
    public ValidationException() : base()
    {
    }

    public ValidationException(string message) : base(message)
    {
    }

    public ValidationException(string message, System.Exception innerException) : base(message, innerException)
    {
    }
}