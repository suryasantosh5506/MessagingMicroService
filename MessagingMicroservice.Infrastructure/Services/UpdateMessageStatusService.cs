using MessagingMicroservice.Application.Interfaces;
using MessagingMicroservice.Domain.Enums;
using MessagingMicroservice.Domain.Interfaces;
using MessagingMicroservice.Domain.Models;

namespace MessagingMicroservice.Infrastructure.Services;

public class UpdateMessageStatusService:IUpdateMessageStatusService
{
    
    private readonly IOutboundMessageRepository _outboundMessageRepository;

    public UpdateMessageStatusService(IOutboundMessageRepository outboundMessageRepository)
    {
        _outboundMessageRepository = outboundMessageRepository;
    }
    
    public async Task UpdateMessageStatusAsync(UpdateMessageStatus request)
    {
        var message = await _outboundMessageRepository.GetMessageByProviderId(request.ProviderMessageId);
        if (message == null) return;
        var currentStatus=Enum.Parse<MessageStatus>(message.Status);
        bool isValid=MessageStatusTransition.IsValid(currentStatus,request.Status);
        if (!isValid) return;
        await _outboundMessageRepository.UpdateMessage(request);    
    }
}