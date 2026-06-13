FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY ["src/BuildingBlocks/EWallet.Common/EWallet.Common.csproj", "src/BuildingBlocks/EWallet.Common/"]
COPY ["src/Gateways/EWallet.Gateways.WebAPI/EWallet.Gateways.WebAPI.csproj", "src/Gateways/EWallet.Gateways.WebAPI/"]

RUN dotnet restore "src/Gateways/EWallet.Gateways.WebAPI/EWallet.Gateways.WebAPI.csproj"

# Copy everything else and build
COPY . .
RUN dotnet publish "src/Gateways/EWallet.Gateways.WebAPI/EWallet.Gateways.WebAPI.csproj" -c Release -o /app/publish

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "EWallet.Gateways.WebAPI.dll"]
