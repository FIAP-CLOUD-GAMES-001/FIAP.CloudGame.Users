using FIAP.CloudGames.Usuarios.Domain.Entities;

namespace FIAP.CloudGames.Usuarios.Domain.Interfaces.Repositories;
public interface IUserRepository
{
    Task<bool> EmailExistsAsync(string email);
    Task<UserEntity?> GetByEmailAsync(string email);
    Task<UserEntity?> GetByIdAsync(int id);
    Task AddAsync(UserEntity user);
    Task<List<UserEntity>> ListAllAsync();
    Task UpdateAsync(UserEntity user);
}




