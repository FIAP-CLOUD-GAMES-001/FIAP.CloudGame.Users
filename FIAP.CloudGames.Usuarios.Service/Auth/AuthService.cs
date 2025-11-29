using FIAP.CloudGames.Usuarios.Domain.Exceptions;
using FIAP.CloudGames.Usuarios.Domain.Interfaces.Auth;
using FIAP.CloudGames.Usuarios.Domain.Interfaces.Repositories;
using FIAP.CloudGames.Usuarios.Domain.Interfaces.Services;
using FIAP.CloudGames.Usuarios.Domain.Requests.Auth;
using FIAP.CloudGames.Usuarios.Domain.Responses.Auth;

namespace FIAP.CloudGames.Usuarios.Service.Auth;

public class AuthService(IUserRepository repository, ITokenService tokens) : IAuthService
{
    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await repository.GetByEmailAsync(request.Email)
                   ?? throw new AuthenticationException("Credenciais inválidas.");

        var passwordOk = user.VerifyPassword(request.Password);

        if (!passwordOk)
            throw new AuthenticationException("Credenciais inválidas.");

        var token = tokens.Generate(user);
        var validTo = tokens.GetExpirationDate(token);

        return new AuthResponse(token, validTo);
    }
}




