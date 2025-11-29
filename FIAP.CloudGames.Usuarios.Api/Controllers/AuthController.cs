using FIAP.CloudGames.Usuarios.Api.Extensions;
using FIAP.CloudGames.Usuarios.Domain.Interfaces.Services;
using FIAP.CloudGames.Usuarios.Domain.Models;
using FIAP.CloudGames.Usuarios.Domain.Requests.Auth;
using FIAP.CloudGames.Usuarios.Domain.Responses.Auth;
using Microsoft.AspNetCore.Mvc;

namespace FIAP.CloudGames.Usuarios.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[ProducesResponseType(typeof(ApiResponse<AuthResponse>), StatusCodes.Status200OK)]
[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status401Unauthorized)]
[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status500InternalServerError)]
public class AuthController(IAuthService service) : ControllerBase
{
    /// <summary>
    /// Authenticates a user based on the provided login request and returns the result.
    /// </summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await service.LoginAsync(request);
        return this.ApiOk(user, "Login successful.");
    }
}




