using FIAP.CloudGames.Usuarios.Domain.Entities;

namespace FIAP.CloudGames.Usuarios.Domain.Interfaces.Repositories;
public interface IOwnedGameRepository
{
    Task AddAsync(OwnedGameEntity ownedGame);
    Task<List<OwnedGameEntity>> GetByUserIdAsync(int userId);
}




