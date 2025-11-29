using FIAP.CloudGames.Usuarios.Domain.Enums;
using FIAP.CloudGames.Usuarios.Domain.Requests.User;
using FIAP.CloudGames.Usuarios.Domain.Responses.User;

namespace FIAP.CloudGames.Usuarios.Domain.Interfaces.Services;
public interface IUserService
{
    Task<UserResponse> GetByIdAsync(int id);
    Task<UserResponse> RegisterAsync(RegisterUserRequest request);
    Task<UserResponse> RegisterAdminAsync(RegisterUserAdminRequest request);
    Task<List<UserResponse>> GetAllUsersAsync();
    Task<UserResponse> UpdateUserRoleAsync(int userId, ERole newRole);
}




