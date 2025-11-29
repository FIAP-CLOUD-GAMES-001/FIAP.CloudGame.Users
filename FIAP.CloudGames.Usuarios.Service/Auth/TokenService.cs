using FIAP.CloudGames.Usuarios.Domain.Entities;
using FIAP.CloudGames.Usuarios.Domain.Interfaces.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FIAP.CloudGames.Usuarios.Service.Auth;
/// <summary>
/// Vamos utilizar Enum para definir os roles de usuário nesse inicio, mas o ideal é utilizar uma tabela de roles no banco de dados.
/// </summary>
public class TokenService(IConfiguration configuration) : ITokenService
{
    public string Generate(UserEntity user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var issuer = configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("JWT Issuer not configured");
        var audience = configuration["Jwt:Audience"] ?? throw new InvalidOperationException("JWT Audience not configured");
        var keyString = configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not configured");
        
        var key = Encoding.UTF8.GetBytes(keyString);
        var creds = new SigningCredentials(
                        new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds);

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        
        // Log para debug (remover em produção se necessário)
        Console.WriteLine($"[TokenService] Token gerado - Issuer: {issuer}, Audience: {audience}");
        
        return tokenString;
    }

    public DateTime GetExpirationDate(string token)
    {
        var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
        return jwtToken.ValidTo;
    }
}



