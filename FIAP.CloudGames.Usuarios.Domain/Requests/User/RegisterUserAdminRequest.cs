using FIAP.CloudGames.Usuarios.Domain.Enums;

namespace FIAP.CloudGames.Usuarios.Domain.Requests.User;
public record RegisterUserAdminRequest(string Name, string Email, string Password, ERole Role = ERole.User);




