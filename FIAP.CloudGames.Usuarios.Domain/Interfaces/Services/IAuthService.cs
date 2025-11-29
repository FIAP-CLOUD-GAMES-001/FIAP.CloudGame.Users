using FIAP.CloudGames.Usuarios.Domain.Requests.Auth;
using FIAP.CloudGames.Usuarios.Domain.Responses.Auth;

namespace FIAP.CloudGames.Usuarios.Domain.Interfaces.Services;
public interface IAuthService
{
    Task<AuthResponse> LoginAsync(LoginRequest request);
}




