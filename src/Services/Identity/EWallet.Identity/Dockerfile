FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# copy csproj and restore as distinct layers
COPY ["src/BuildingBlocks/EWallet.Common/EWallet.Common.csproj", "src/BuildingBlocks/EWallet.Common/"]
COPY ["src/BuildingBlocks/EWallet.Contracts/EWallet.Contracts.csproj", "src/BuildingBlocks/EWallet.Contracts/"]
COPY ["src/Services/Identity/EWallet.Identity/EWallet.Identity.csproj", "src/Services/Identity/EWallet.Identity/"]

RUN dotnet restore "src/Services/Identity/EWallet.Identity/EWallet.Identity.csproj"

# Copy everything and build app
COPY . .
RUN dotnet publish "src/Services/Identity/EWallet.Identity/EWallet.Identity.csproj" -c Release -o /app/publish

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "EWallet.Identity.dll"]
