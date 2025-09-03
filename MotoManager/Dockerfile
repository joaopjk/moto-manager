# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia os arquivos dos projetos necessários e restaura dependências
COPY ["MotoManager/MotoManager.Api.csproj", "MotoManager/"]
COPY ["MotoManager.IoC/MotoManager.IoC.csproj", "MotoManager.IoC/"]
RUN dotnet restore "MotoManager/MotoManager.Api.csproj"

# Copia todo o código e faz o build
COPY . .
WORKDIR "/src/MotoManager"
RUN dotnet publish "MotoManager.Api.csproj" -c Release -o /app/publish

# Etapa final para execução
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["dotnet", "MotoManager.Api.dll"]