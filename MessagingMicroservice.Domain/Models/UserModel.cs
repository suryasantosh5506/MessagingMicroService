using System.ComponentModel.DataAnnotations;

namespace MessagingMicroservice.Domain.Models;

public class UserModel
{
    [EmailAddress]
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
}