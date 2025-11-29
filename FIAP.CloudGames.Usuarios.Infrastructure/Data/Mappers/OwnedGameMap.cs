using FIAP.CloudGames.Usuarios.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FIAP.CloudGames.Usuarios.Infrastructure.Data.Mappers;
public class OwnedGameMap : IEntityTypeConfiguration<OwnedGameEntity>
{
    public void Configure(EntityTypeBuilder<OwnedGameEntity> builder)
    {
        builder.ToTable("OwnedGames").HasKey(og => og.Id);

        builder.Property(og => og.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(og => og.UserId)
            .IsRequired();

        builder.Property(og => og.GameId)
            .IsRequired();

        builder.Property(og => og.PurchaseDate)
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(og => og.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasOne(og => og.User)
            .WithMany(u => u.OwnedGames)
            .HasForeignKey(og => og.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}




