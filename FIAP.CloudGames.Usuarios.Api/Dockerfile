# Esta fase é usada durante a execução no VS no modo rápido (Padrão para a configuração de Depuração)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Esta fase é usada para compilar o projeto de serviço
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copia a solution e os arquivos .csproj
COPY ["FIAP.CloudGames.Usuarios.sln", "."]
COPY ["FIAP.CloudGames.Usuarios.Api/FIAP.CloudGames.Usuarios.Api.csproj", "FIAP.CloudGames.Usuarios.Api/"]
COPY ["FIAP.CloudGames.Usuarios.Domain/FIAP.CloudGames.Usuarios.Domain.csproj", "FIAP.CloudGames.Usuarios.Domain/"]
COPY ["FIAP.CloudGames.Usuarios.Infrastructure/FIAP.CloudGames.Usuarios.Infrastructure.csproj", "FIAP.CloudGames.Usuarios.Infrastructure/"]
COPY ["FIAP.CloudGames.Usuarios.Service/FIAP.CloudGames.Usuarios.Service.csproj", "FIAP.CloudGames.Usuarios.Service/"]

# Restaura as dependências
RUN dotnet restore "FIAP.CloudGames.Usuarios.sln"

# Copia todo o código fonte
COPY . .

# Compila o projeto
WORKDIR "/src/FIAP.CloudGames.Usuarios.Api"
RUN dotnet build "FIAP.CloudGames.Usuarios.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Esta fase é usada para publicar o projeto de serviço a ser copiado para a fase final
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "FIAP.CloudGames.Usuarios.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Esta fase é usada na produção
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FIAP.CloudGames.Usuarios.Api.dll"]




