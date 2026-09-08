using MessagingMicroservice.Application.Interfaces;

namespace MessagingMicroservice.Tests.Fakes;

public class FakeMessageProviderFactory:IMessageProviderFactory
{
    
    private readonly IMessageProvider _primaryProvider;
    private readonly IMessageProvider _fallBackProvider;
    
    public FakeMessageProviderFactory(IMessageProvider primaryProvider,IMessageProvider fallBackProvider)
    {
        _primaryProvider = primaryProvider;
        _fallBackProvider = fallBackProvider;
    }
    
    public IMessageProvider GetPrimaryMessageProvider()
    {
        return _primaryProvider;
    }

    public IMessageProvider GetFallBackMessageProvider()
    {
        return _fallBackProvider;
    }
}