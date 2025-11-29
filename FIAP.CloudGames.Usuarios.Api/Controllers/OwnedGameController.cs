using FIAP.CloudGames.Usuarios.Api.Extensions;
using FIAP.CloudGames.Usuarios.Api.Filters;
using FIAP.CloudGames.Usuarios.Domain.Interfaces.Services;
using FIAP.CloudGames.Usuarios.Domain.Models;
using FIAP.CloudGames.Usuarios.Domain.Requests.Game;
using FIAP.CloudGames.Usuarios.Domain.Responses.Game;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FIAP.CloudGames.Usuarios.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status401Unauthorized)]
[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status403Forbidden)]
[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status500InternalServerError)]
public class OwnedGameController(IOwnedGameService ownedGameService) : ControllerBase
{
    [HttpPost]
    [ServiceFilter(typeof(OwnedGameAccessFilter))]
    [TypeFilter(typeof(ValidationFilter<AddOwnedGameRequest>))]
    [ProducesResponseType(typeof(ApiResponse<OwnedGameResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> Add([FromBody] AddOwnedGameRequest request)
    {
        var ownedGame = await ownedGameService.AddAsync(request);
        return this.ApiOk(ownedGame, "Game added to library successfully.", HttpStatusCode.Created);
    }

    [HttpGet("user/{userId}")]
    [ServiceFilter(typeof(OwnedGameAccessFilter))]
    [ProducesResponseType(typeof(ApiResponse<List<OwnedGameResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByUserId(int userId)
    {
        var ownedGames = await ownedGameService.GetByUserIdAsync(userId);
        return this.ApiOk(ownedGames, "Library retrieved successfully.");
    }
}




