namespace MessagingMicroservice.Application.Interfaces;

public interface IValidateWebhookRequest<T>
{
    Task<bool> ValidateAsync(T request);
}