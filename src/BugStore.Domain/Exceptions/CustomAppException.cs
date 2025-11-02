namespace BugStore.Domain.Exceptions
{
    public class CustomAppException(string message, int statusCode = 400) : Exception(message)
    {
        public int StatusCode { get; set; } = statusCode;
    }
}
