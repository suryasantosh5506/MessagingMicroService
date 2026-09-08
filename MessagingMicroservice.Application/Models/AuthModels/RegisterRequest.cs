using System.ComponentModel.DataAnnotations;

namespace MessagingMicroservice.Application.Models.AuthModels;

public class RegisterRequest
{
    [EmailAddress]
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}