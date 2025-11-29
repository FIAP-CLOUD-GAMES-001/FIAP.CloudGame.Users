using FIAP.CloudGames.Usuarios.Domain.Enums;

namespace FIAP.CloudGames.Usuarios.Domain.Responses.User;
public record UserResponse(int Id, string Name, string Email, ERole Role = ERole.User);




