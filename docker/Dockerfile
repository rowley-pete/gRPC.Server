FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5241
EXPOSE 5242

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["gRPC.Server.Api/gRPC.Server.Api.csproj", "gRPC.Server.Api/"]

RUN dotnet restore "./gRPC.Server.Api/gRPC.Server.Api.csproj"

COPY . .
WORKDIR "/src/gRPC.Server.Api"
RUN dotnet build "./gRPC.Server.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./gRPC.Server.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "gRPC.Server.Api.dll"]