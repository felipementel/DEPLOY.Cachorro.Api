#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
LABEL org.opencontainers.image.authors="felipementel@hotmail.com"
LABEL org.opencontainers.image.source=https://github.com/felipementel/DEPLOY.Cachorro.Api
LABEL org.opencontainers.image.description="API criada por Felipe Augusto, canal DEPLOY"
LABEL org.opencontainers.image.licenses=MIT

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["DEPLOY.Cachorro.Api/DEPLOY.Cachorro.Api.csproj", "DEPLOY.Cachorro.Api/"]
COPY ["DEPLOY.Cachorro.Domain/DEPLOY.Cachorro.Domain.csproj", "DEPLOY.Cachorro.Domain/"]
COPY ["DEPLOY.Cachorro.Repository/DEPLOY.Cachorro.Repository.csproj", "DEPLOY.Cachorro.Repository/"]
RUN dotnet restore "DEPLOY.Cachorro.Api/DEPLOY.Cachorro.Api.csproj"
COPY . .
WORKDIR "/src/DEPLOY.Cachorro.Api"
RUN dotnet build "DEPLOY.Cachorro.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DEPLOY.Cachorro.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DEPLOY.Cachorro.Api.dll"]