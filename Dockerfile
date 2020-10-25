FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY GameServer/*.csproj ./GameServer/
# COPY NetCoreServer/*.csproj ./NetCoreServer/
COPY Data/*.csproj ./Data/
COPY *.sln ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/runtime:3.1
WORKDIR /app
COPY --from=build-env /app/out .
COPY Config /Config
COPY GameServer/Web/html ./html
# Copy latest db
#COPY GameServer/exteel.db .
ENTRYPOINT ["dotnet", "GameServer.dll"]
