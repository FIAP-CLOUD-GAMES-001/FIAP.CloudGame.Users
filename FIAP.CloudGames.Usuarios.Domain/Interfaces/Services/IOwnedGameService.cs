using FIAP.CloudGames.Usuarios.Domain.Requests.Game;
using FIAP.CloudGames.Usuarios.Domain.Responses.Game;

namespace FIAP.CloudGames.Usuarios.Domain.Interfaces.Services;
public interface IOwnedGameService
{
    Task<OwnedGameResponse> AddAsync(AddOwnedGameRequest request);
    Task<IEnumerable<OwnedGameResponse>> GetByUserIdAsync(int userId);
}




