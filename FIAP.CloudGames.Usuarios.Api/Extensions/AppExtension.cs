using FIAP.CloudGames.Usuarios.Api.Middlewares;
using FIAP.CloudGames.Usuarios.Domain.Entities;
using FIAP.CloudGames.Usuarios.Domain.Enums;
using FIAP.CloudGames.Usuarios.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace FIAP.CloudGames.Usuarios.Api.Extensions;

public static class AppExtension
{
    public static void UseProjectConfiguration(this WebApplication app)
    {
        app.UseCustomSwagger();
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.UseMiddleware<ForwardedPrefixMiddleware>();
        app.MapControllers();
        app.GenerateMigrations();
        app.MapHealthChecks("/health");
    }

    private static void UseCustomSwagger(this WebApplication app)
    {
        var pathBase = app.Configuration["Swagger:PathBase"] ?? string.Empty;
        
        app.UseSwagger(c =>
        {
            if (!string.IsNullOrEmpty(pathBase))
            {
                c.PreSerializeFilters.Add((swagger, httpReq) =>
                {
                    swagger.Servers = new List<OpenApiServer>
                    {
                        new OpenApiServer { Url = pathBase }
                    };
                });
            }
        });

        app.UseSwaggerUI(c =>
        {
            var swaggerUrl = string.IsNullOrEmpty(pathBase) 
                ? "/swagger/v1/swagger.json" 
                : $"{pathBase.TrimEnd('/')}/swagger/v1/swagger.json";
            
            c.SwaggerEndpoint(swaggerUrl, "FIAPCloudGames Users API v1");
            
            c.RoutePrefix = "swagger";
            
            c.SupportedSubmitMethods([
                SubmitMethod.Get,
                SubmitMethod.Post,
                SubmitMethod.Put,
                SubmitMethod.Delete,
                SubmitMethod.Patch
            ]);
        });
    }

    private static void GenerateMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
        dbContext.Database.Migrate();

        var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();

        var email = config["SeedAdmin:Email"] ?? Environment.GetEnvironmentVariable("SEED_ADMIN_EMAIL");
        var password = config["SeedAdmin:Password"] ?? Environment.GetEnvironmentVariable("SEED_ADMIN_PASSWORD");

        if (!string.IsNullOrEmpty(email) &&
            !string.IsNullOrEmpty(password) &&
            !dbContext.Users.Any(u => u.Email == email))
        {
            var admin = new UserEntity(
                name: "Administrator",
                email: email,
                plainPassword: password,
                role: ERole.Admin
            );

            dbContext.Users.Add(admin);
            dbContext.SaveChanges();
        }
    }
}




