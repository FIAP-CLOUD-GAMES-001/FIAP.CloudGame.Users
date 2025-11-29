using FIAP.CloudGames.Usuarios.Api.Extensions;
using FIAP.CloudGames.Usuarios.Api.Filters;
using FIAP.CloudGames.Usuarios.Domain.Enums;
using FIAP.CloudGames.Usuarios.Domain.Interfaces.Services;
using FIAP.CloudGames.Usuarios.Domain.Models;
using FIAP.CloudGames.Usuarios.Domain.Requests.User;
using FIAP.CloudGames.Usuarios.Domain.Responses.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace FIAP.CloudGames.Usuarios.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status401Unauthorized)]
[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status403Forbidden)]
[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status500InternalServerError)]
public class UserController(IUserService service) : ControllerBase
{
    [HttpPost("register")]
    [AllowAnonymous]
    [TypeFilter(typeof(ValidationFilter<RegisterUserRequest>))]
    [ProducesResponseType(typeof(ApiResponse<UserResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
    {
        var userCreated = await service.RegisterAsync(request);
        return this.ApiOk(userCreated, "User registered successfully.", HttpStatusCode.Created);
    }

    [HttpGet("users")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<List<UserResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await service.GetAllUsersAsync();
        return this.ApiOk(users, "Users retrieved successfully.");
    }

    [HttpPost("create-user-admin")]
    [Authorize(Roles = "Admin")]
    [TypeFilter(typeof(ValidationFilter<RegisterUserAdminRequest>))]
    [ProducesResponseType(typeof(ApiResponse<UserResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateUserFromAdmin([FromBody] RegisterUserAdminRequest request)
    {
        var created = await service.RegisterAdminAsync(request);
        return this.ApiOk(created, "Admin created successfully.", HttpStatusCode.Created);
    }

    [HttpPut("{id}/role")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<UserResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateRole(int id, [FromQuery] ERole role)
    {
        var updated = await service.UpdateUserRoleAsync(id, role);
        return this.ApiOk(updated, $"User role updated to {role}.");
    }

    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<UserResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMyProfile()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            return this.ApiFail("Invalid token.", null, HttpStatusCode.Unauthorized);

        var user = await service.GetByIdAsync(userId);

        return this.ApiOk(user, "Profile retrieved successfully.");
    }
}




