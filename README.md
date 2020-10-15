# server

This is the main repository for the server source code. This functions as a simple, bare bones server emulator.

### project state

This code is provided as-is, with no warranty. It is not being actively maintained. If you have any questions, please
reach out to me and I will do my best to answer them if I have time.

If you are interested in maintaining this project, please let me know.

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

## legal
Every effort has been made to comply with all laws and regulations. This project is an original creation, 
distributed free of charge. 

It contains no copyrighted files or code. It does not function without the game files, which are NOT included. 
In order for it to function, the user must legally acquire these files.

Donations are not accepted. 

The sole intent of this project is to provide players a chance to enjoy a long dead game from their childhood. 

If there are any legal concerns, please reach out to me on github and I will be happy to comply in any way required.
