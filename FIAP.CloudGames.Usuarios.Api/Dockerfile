# Esta fase é usada para compilar o projeto de serviço
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
USER root
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

# Publica o projeto
RUN dotnet publish "FIAP.CloudGames.Usuarios.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Esta fase é usada na produção
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final
USER root

# Configurando timezone para America/Sao_Paulo
RUN apk add --no-cache tzdata \
    && cp /usr/share/zoneinfo/America/Sao_Paulo /etc/localtime \
    && echo "America/Sao_Paulo" > /etc/timezone \
    && apk del tzdata

ENV TZ=America/Sao_Paulo

EXPOSE 8080
EXPOSE 8081

WORKDIR /app
COPY --from=build /app/publish .
USER $APP_UID
ENTRYPOINT ["dotnet", "FIAP.CloudGames.Usuarios.Api.dll"]




