using FIAP.CloudGames.Usuarios.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FIAP.CloudGames.Usuarios.Infrastructure.Data;
/// <summary>
/// Geração dos migrations e scripts de banco de dados
/// </summary>
/// dotnet ef migrations add InitialCreate --project ../FIAP.CloudGames.Usuarios.Infrastructure --startup-project ../FIAP.CloudGames.Usuarios.Api
/// dotnet ef migrations script -o ./Scripts/script_base.sql --context DataContext --startup-project ../FIAP.CloudGames.Usuarios.Api
/// dotnet ef database update --context DataContext --startup-project ../FIAP.CloudGames.Usuarios.Api
/// <param name="options"></param>
public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<UserEntity> Users { get; set; } = null!;
    public DbSet<OwnedGameEntity> OwnedGames { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
    }
}




