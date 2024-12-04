FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/WebAPI/Nibbler.WebAPI/Nibbler.WebAPI.csproj", "WebAPI/Nibbler.WebAPI/"]
COPY ["src/Autenticacao/Nibbler.Autenticacao.App/Nibbler.Autenticacao.App.csproj", "Autenticacao/Nibbler.Autenticacao.App/"]
COPY ["src/BuildingBlocks/Nibbler.Core/Nibbler.Core.csproj", "BuildingBlocks/Nibbler.Core/"]
COPY ["src/BuildingBlocks/Nibbler.WebAPI.Core/Nibbler.WebAPI.Core.csproj", "BuildingBlocks/Nibbler.WebAPI.Core/"]
COPY ["src/Diario/Nibbler.Diario.App/Nibbler.Diario.App.csproj", "Diario/Nibbler.Diario.App/"]
COPY ["src/Diario/Nibbler.Diario.Infra/Nibbler.Diario.Infra.csproj", "Diario/Nibbler.Diario.Infra/"]
COPY ["src/Usuarios/Nibbler.Usuario.App/Nibbler.Usuario.App.csproj", "Usuarios/Nibbler.Usuario.App/"]
COPY ["src/Usuarios/Nibbler.Usuario.Infra/Nibbler.Usuario.Infra.csproj", "Usuarios/Nibbler.Usuario.Infra/"]

RUN dotnet restore "WebAPI/Nibbler.WebAPI/Nibbler.WebAPI.csproj"

COPY src/. .
WORKDIR "/src/WebAPI/Nibbler.WebAPI"
RUN dotnet build "Nibbler.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Nibbler.WebAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Nibbler.WebAPI.dll"]