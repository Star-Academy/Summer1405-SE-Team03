namespace MyWebApi.Exceptions;

public class InvalidDatabaseException : Exception
{
    public InvalidDatabaseException(string message) : base(message)
    {
    }
}