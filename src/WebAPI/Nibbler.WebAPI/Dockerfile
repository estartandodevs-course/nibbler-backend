FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["./WebAPI/Nibbler.WebAPI/Nibbler.WebAPI.csproj", "WebAPI/Nibbler.WebAPI/"]
COPY ["./Autenticacao/Nibbler.Autenticacao.App/Nibbler.Autenticacao.App.csproj", "Autenticacao/Nibbler.Autenticacao.App/"]

COPY ["./BuildingBlocks/Nibbler.Core/Nibbler.Core.csproj", "BuildingBlocks/Nibbler.Core/"]
COPY ["./BuildingBlocks/Nibbler.WebAPI.Core/Nibbler.WebAPI.Core.csproj", "BuildingBlocks/Nibbler.WebAPI.Core/"]

COPY ["./Tarefa/Nibbler.Tarefa.App/Nibbler.Tarefa.App.csproj", "Tarefa/Nibbler.Tarefa.App/"]
COPY ["./Tarefa/Nibbler.Tarefa.Domain/Nibbler.Tarefa.Domain.csproj", "Tarefa/Nibbler.Tarefa.Domain/"]
COPY ["./Tarefa/Nibbler.Tarefa.Infra/Nibbler.Tarefa.Infra.csproj", "Tarefa/Nibbler.Tarefa.Infra/"]

COPY ["./Diario/Nibbler.Diario.App/Nibbler.Diario.App.csproj", "Diario/Nibbler.Diario.App/"]
COPY ["./Diario/Nibbler.Diario.Domain/Nibbler.Diario.Domain.csproj", "Diario/Nibbler.Diario.Domain/"]
COPY ["./Diario/Nibbler.Diario.Infra/Nibbler.Diario.Infra.csproj", "Diario/Nibbler.Diario.Infra/"]

COPY ["./Usuarios/Nibbler.Usuario.App/Nibbler.Usuario.App.csproj", "Usuarios/Nibbler.Usuario.App/"]
COPY ["./Usuarios/Nibbler.Usuario.Domain/Nibbler.Usuario.Domain.csproj", "Usuarios/Nibbler.Usuario.Domain/"]
COPY ["./Usuarios/Nibbler.Usuario.Infra/Nibbler.Usuario.Infra.csproj", "Usuarios/Nibbler.Usuario.Infra/"]

RUN dotnet restore "WebAPI/Nibbler.WebAPI/Nibbler.WebAPI.csproj"

COPY src/. .
WORKDIR "WebAPI/Nibbler.WebAPI"
RUN dotnet build "Nibbler.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Nibbler.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Nibbler.WebAPI.dll"]