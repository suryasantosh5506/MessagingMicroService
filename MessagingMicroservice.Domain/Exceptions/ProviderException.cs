namespace MessagingMicroservice.Domain.Exceptions;

public class ProviderException:Exception
{
    public bool CanFallback { get; }

    public ProviderException(string message,bool canFallback) : base(message)
    {
        CanFallback = canFallback;
    }
}