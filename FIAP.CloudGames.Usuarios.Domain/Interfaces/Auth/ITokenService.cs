using FIAP.CloudGames.Usuarios.Domain.Entities;

namespace FIAP.CloudGames.Usuarios.Domain.Interfaces.Auth;
public interface ITokenService
{
    string Generate(UserEntity usuario);
    DateTime GetExpirationDate(string token);
}




