# Use imagem oficial do .NET 9 para build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia os arquivos do projeto e restaura dependências
COPY ["MotoManager.Worker.Rental/MotoManager.Worker.MotocycleCreate.csproj", "MotoManager.Worker.Rental/"]
COPY ["MotoManager.Domain/MotoManager.Domain.csproj", "MotoManager.Domain/"]
COPY ["MotoManager.IoC/MotoManager.IoC.csproj", "MotoManager.IoC/"]
COPY ["MotoManager.Shared/MotoManager.Shared.csproj", "MotoManager.Shared/"]
RUN dotnet restore "MotoManager.Worker.Rental/MotoManager.Worker.MotocycleCreate.csproj"

# Copia todo o código e faz o build
COPY . .
WORKDIR "/src/MotoManager.Worker.Rental"
RUN dotnet publish "MotoManager.Worker.MotocycleCreate.csproj" -c Release -o /app/publish

# Imagem final para execução
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MotoManager.Worker.MotocycleCreate.dll"]