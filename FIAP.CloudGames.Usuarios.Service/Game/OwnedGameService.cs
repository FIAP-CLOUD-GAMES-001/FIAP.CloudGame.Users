using FIAP.CloudGames.Usuarios.Domain.Entities;
using FIAP.CloudGames.Usuarios.Domain.Exceptions;
using FIAP.CloudGames.Usuarios.Domain.Interfaces.Repositories;
using FIAP.CloudGames.Usuarios.Domain.Interfaces.Services;
using FIAP.CloudGames.Usuarios.Domain.Requests.Game;
using FIAP.CloudGames.Usuarios.Domain.Responses.Game;

namespace FIAP.CloudGames.Usuarios.Service.Game;
public class OwnedGameService(
    IOwnedGameRepository ownedGameRepository,
    IUserRepository userRepository) : IOwnedGameService
{
    public async Task<OwnedGameResponse> AddAsync(AddOwnedGameRequest request)
    {
        var user = await userRepository.GetByIdAsync(request.UserId)
            ?? throw new NotFoundException($"User with ID {request.UserId} not found.");

        // Nota: Em uma arquitetura de microserviços, a validação do GameId deveria ser feita
        // através de uma chamada ao microserviço de Games ou via mensageria
        // Por enquanto, apenas validamos o usuário

        var ownedGame = new OwnedGameEntity(request.UserId, request.GameId);
        await ownedGameRepository.AddAsync(ownedGame);
        return new OwnedGameResponse(ownedGame.Id, ownedGame.UserId, ownedGame.GameId, ownedGame.PurchaseDate);
    }

    public async Task<IEnumerable<OwnedGameResponse>> GetByUserIdAsync(int userId)
    {
        var ownedGames = await ownedGameRepository.GetByUserIdAsync(userId);
        return ownedGames.Select(og => new OwnedGameResponse(og.Id, og.UserId, og.GameId, og.PurchaseDate));
    }
}




