namespace FIAP.CloudGames.Usuarios.Domain.Entities;
public abstract class BaseEntity
{
    public int Id { get; protected set; }
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
}




