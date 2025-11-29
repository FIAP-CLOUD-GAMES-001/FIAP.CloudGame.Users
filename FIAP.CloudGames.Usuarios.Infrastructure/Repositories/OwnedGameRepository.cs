using FIAP.CloudGames.Usuarios.Domain.Entities;
using FIAP.CloudGames.Usuarios.Domain.Interfaces.Repositories;
using FIAP.CloudGames.Usuarios.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FIAP.CloudGames.Usuarios.Infrastructure.Repositories;
public class OwnedGameRepository(DataContext context) : IOwnedGameRepository
{
    public async Task AddAsync(OwnedGameEntity ownedGame)
    {
        await context.OwnedGames.AddAsync(ownedGame);
        await context.SaveChangesAsync();
    }

    public async Task<List<OwnedGameEntity>> GetByUserIdAsync(int userId)
    {
        return await context.OwnedGames
            .Where(og => og.UserId == userId)
            .AsNoTracking()
            .ToListAsync();
    }
}




