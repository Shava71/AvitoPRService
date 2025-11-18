FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER root
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

RUN apt-get update && apt-get install -y \
    netcat-openbsd \
    postgresql-client \
    && rm -rf /var/lib/apt/lists/*

USER $APP_UID

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["AvitoPRService.Api/AvitoPRService.Api.csproj", "AvitoPRService.Api/"]
COPY ["AvitoPRService.Application/AvitoPRService.Application.csproj", "AvitoPRService.Application/"]
COPY ["AvitoPRService.Domain/AvitoPRService.Domain.csproj", "AvitoPRService.Domain/"]
COPY ["AvitoPRService.Infrastructure/AvitoPRService.Infrastructure.csproj", "AvitoPRService.Infrastructure/"]
RUN dotnet restore "AvitoPRService.Api/AvitoPRService.Api.csproj"
COPY . .
WORKDIR "/src/AvitoPRService.Api"
RUN dotnet build "AvitoPRService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "AvitoPRService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY AvitoPRService.Api/sql ./sql 
COPY --from=publish /app/publish .
COPY AvitoPRService.Api/wait-for-db.sh .
RUN dos2unix wait-for-db.sh && chmod +x wait-for-db.sh

ENTRYPOINT ["./wait-for-db.sh"]