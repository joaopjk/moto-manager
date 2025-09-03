# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia os arquivos dos projetos necessários e restaura dependências
COPY ["MotoManager.Worker.ShowYear/MotoManager.Worker.ShowYear.csproj", "MotoManager.Worker.ShowYear/"]
COPY ["MotoManager.Domain/MotoManager.Domain.csproj", "MotoManager.Domain/"]
COPY ["MotoManager.Infrastructure/MotoManager.Infrastructure.csproj", "MotoManager.Infrastructure/"]
COPY ["MotoManager.Shared/MotoManager.Shared.csproj", "MotoManager.Shared/"]
RUN dotnet restore "MotoManager.Worker.ShowYear/MotoManager.Worker.ShowYear.csproj"

# Copia todo o código e faz o build
COPY . .
WORKDIR "/src/MotoManager.Worker.ShowYear"
RUN dotnet publish "MotoManager.Worker.ShowYear.csproj" -c Release -o /app/publish

# Etapa final para execução
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MotoManager.Worker.ShowYear.dll"]