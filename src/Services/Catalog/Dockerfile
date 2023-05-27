# FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
# WORKDIR /App

# # Copy everything
# COPY . ./
# # Restore as distinct layers
# RUN dotnet restore
# # Build and publish a release
# RUN dotnet publish -c Release -o out

# # Build runtime image
# FROM mcr.microsoft.com/dotnet/aspnet:7.0
# WORKDIR /App
# COPY --from=build-env /App/out .
# ENTRYPOINT ["dotnet", "Catalog.API.dll"]

# Base env
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

# Build env
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ./Catalog.API.csproj ./Services/Catalog/Catalog.API/
RUN dotnet restore ./Services/Catalog/Catalog.API/Catalog.API.csproj
COPY . ./Services/Catalog/Catalog.API/
WORKDIR /src/Services/Catalog/Catalog.API/
RUN dotnet build Catalog.API.csproj -c Release -o /app/build


# Publish env
FROM build as publish
RUN dotnet publish -c release -o /app/publish --no-restore


# Final env
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish ./
ENTRYPOINT ["dotnet", "Catalog.API.dll"]