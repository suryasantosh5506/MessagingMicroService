namespace MessagingMicroservice.Application.Interfaces;

public interface IMessageProviderFactory
{
    IMessageProvider GetPrimaryMessageProvider();
    IMessageProvider GetFallBackMessageProvider();
}