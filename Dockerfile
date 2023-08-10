FROM mcr.microsoft.com/dotnet/sdk:6.0 AS builder
WORKDIR /App

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish ./GrapheneTemplate/ -c Release -o out -nowarn:all

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /App
COPY --from=builder /App/out .
RUN ls -la
ENTRYPOINT ["dotnet", "GrapheneTemplate.dll"]