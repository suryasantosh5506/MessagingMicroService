using MessagingMicroService.Client.Interface;
using MessagingMicroService.Client.Models;
using System.Net.Http.Json;
using MessagingMicroservice.Client.Exceptions;

namespace MessagingMicroservice.Client.Services;

public class MessagingClient:IMessagingClient
{

    private readonly HttpClient _httpClient;

    public MessagingClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<SendMessageResponse?> SendMessageAsync(SendMessageRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/messages/send-message", request);
        if (!response.IsSuccessStatusCode)
        {
            var error=await response.Content.ReadAsStringAsync();
            throw new MessagingClientException(error,(int)response.StatusCode);
        }
        return await response.Content.ReadFromJsonAsync<SendMessageResponse?>();
    }
}