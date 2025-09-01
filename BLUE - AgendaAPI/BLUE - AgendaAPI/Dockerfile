# Dockerfile otimizado para estrutura do Visual Studio

# Esta fase é usada durante a execução no VS no modo rápido
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Esta fase é usada para compilar o projeto de serviço
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copie os arquivos de projeto - ajustando os caminhos relativos
COPY ["BLUE - AgendaAPI/Agenda.API.csproj", "BLUE - AgendaAPI/"]
COPY ["Agenda.Application/Agenda.Application.csproj", "Agenda.Application/"]
COPY ["Agenda.Domain/Agenda.Domain.csproj", "Agenda.Domain/"]
COPY ["Agenda.Infrastructure/Agenda.Infrastructure.csproj", "Agenda.Infrastructure/"]

# Restaure as dependências
RUN dotnet restore "./BLUE - AgendaAPI/Agenda.API.csproj"

# Copie todo o código fonte
COPY . .
WORKDIR "/src/BLUE - AgendaAPI"

# Build da aplicação
RUN dotnet build "./Agenda.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Esta fase é usada para publicar o projeto
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Agenda.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Esta fase é usada na produção
FROM base AS final
WORKDIR /app

# Copie os arquivos publicados
COPY --from=publish /app/publish .

# Configure variáveis de ambiente padrão
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "Agenda.API.dll"]