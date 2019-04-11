FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-alpine-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-alpine AS build
WORKDIR /src
COPY ["pumpk1n_backend/pumpk1n_backend.csproj", "pumpk1n_backend/"]
RUN dotnet restore "pumpk1n_backend/pumpk1n_backend.csproj"
COPY . .
WORKDIR "/src/pumpk1n_backend"
RUN dotnet build "pumpk1n_backend.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "pumpk1n_backend.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "pumpk1n_backend.dll"]