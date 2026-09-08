using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MessagingMicroservice.Application.Interfaces;
using MessagingMicroservice.Domain.Entities;
using MessagingMicroservice.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace MessagingMicroservice.Application.Services;

public class TokenService:ITokenService
{
    
    public readonly IConfiguration _configuration;
    
    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public string GenerateToken(UserEntity user)
    {
        List<Claim> claims = new()
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email)
        };
        
        var issuer=_configuration["Jwt:Issuer"];
        var audience=_configuration["Jwt:Audience"];
        var key=_configuration["Jwt:Key"]??"";
        var expiresIn=int.Parse(_configuration["Jwt:ExpirationMinutes"]??"0");

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            signingCredentials: signingCredentials,
            expires: DateTime.UtcNow.AddMinutes(expiresIn),
            claims: claims
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}