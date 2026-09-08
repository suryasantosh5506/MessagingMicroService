using MessagingMicroservice.Application.Interfaces;
using MessagingMicroservice.Application.Models.ConsumerModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MessagingMicroservice.Controllers;

[ApiController]
[Route("api/messages")]
[Authorize]
public class MessagesController:ControllerBase
{
    private readonly ISendMessageService _sendMessageService;

    public MessagesController(ISendMessageService sendMessageService)
    {
        _sendMessageService = sendMessageService;
    }

    [HttpPost("send_message")]
    public async Task<IActionResult> SendMessageAsync([FromBody] SendMessageRequest request)
    {   
        await _sendMessageService.SendMessageAsync(request);
        return Ok();
    }
}