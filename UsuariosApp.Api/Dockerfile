FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["UsuariosApp.Api/UsuariosApp.Api.csproj", "UsuariosApp.Api/"]
RUN dotnet restore "./UsuariosApp.Api/./UsuariosApp.Api.csproj"
COPY . .
WORKDIR "/src/UsuariosApp.Api"
RUN dotnet build "./UsuariosApp.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./UsuariosApp.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UsuariosApp.Api.dll", "--environment", "Development"]

