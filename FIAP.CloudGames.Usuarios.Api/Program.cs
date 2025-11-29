using FIAP.CloudGames.Usuarios.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);
BuilderExtension.AddProjectServices(builder);

var app = builder.Build();
AppExtension.UseProjectConfiguration(app);

await app.RunAsync();
