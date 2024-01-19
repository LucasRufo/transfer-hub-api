#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/TransferHub.API/TransferHub.API.csproj", "src/TransferHub.API/"]
COPY ["src/TransferHub.Domain/TransferHub.Domain.csproj", "src/TransferHub.Domain/"]
COPY ["src/TransferHub.Infrastructure/TransferHub.Infrastructure.csproj", "src/TransferHub.Infrastructure/"]
RUN dotnet restore "./src/TransferHub.API/./TransferHub.API.csproj"
COPY . .
WORKDIR "/src/src/TransferHub.API"
RUN dotnet build "./TransferHub.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TransferHub.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TransferHub.API.dll"]