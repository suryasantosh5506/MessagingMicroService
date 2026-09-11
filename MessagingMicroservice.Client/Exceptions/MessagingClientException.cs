namespace MessagingMicroservice.Client.Exceptions;

public class MessagingClientException:Exception
{
    public int statusCode { get; }

    public MessagingClientException(string message, int statusCode) : base(message)
    {
        this.statusCode = statusCode;
    }
}