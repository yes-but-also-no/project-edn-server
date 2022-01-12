FROM mcr.microsoft.com/dotnet/sdk:3.1-focal
RUN apt-get update && \
    apt-get install -y \
        rename \
    && \
    apt-get clean
WORKDIR /app
COPY . ./
WORKDIR /app/Config/Poo
RUN rename 'y/A-Z/a-z/' *
WORKDIR /app/Config/Spawns
RUN rename 'y/A-Z/a-z/' *
WORKDIR /app/Config/Maps
RUN rename 'y/A-Z/a-z/' *
WORKDIR /app/GameServer
ENTRYPOINT ["dotnet", "run"]
