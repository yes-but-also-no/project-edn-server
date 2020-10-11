# server

This is the main repository for the server source code. This functions as a simple, bare bones server emulator.

## setup

To run the server locally:

- Install .NET Core sdk 3.1 https://dotnet.microsoft.com/download
- run `dotnet restore`
- adjust any port settings in Config/server.json
- cd GameServer/
- run `dotnet run`

### notes

The system comes pre-seeded with a single account, username: pinkett, password: password

The game requires two TCP ports, default 15180 for the webserver for creating accounts, and 15152 for gameplay

You will need to provide the game clients ".map" files under Config/Maps

You will also need to provide the game clients ".poo" files, unencrypted, under Config/Poo

The repository "project-edn-tools" has batch scripts to easily extract these files for fast setup
