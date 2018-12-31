# Multiplayer SpaceWars Game
This is a multiplayer spacewars game built as the final project of my CS 3500: Software Practice I course at the University of Utah. It was built using C# and the .NET framework, utilizing the windows form and console applications. It features both client and server applications, which when run together, allow the play of the game. Built using Sockets and TCP/IP, the server application will allow as many connections as reason permits. The client provides an input to specify which IP Address the server is running on. 

## Getting Started
These intructions will get you a copy of the project running on your local machine. 

### Prerequisites
It is recommended that you have Visual Studio to look at and run the project. If not, the application can still be run, but because it was built using the Windows Form's Application, it will only work on a Windows machine. 

### Running the Game using Visual Studio

1. Clone the repository and open up the Space Wars Solution
2. Set the startup projects as 'View' and 'Server'
3. Build the solution

### Running the Game without Visual Studio
The project can be run by finding the correct executables for both server and client applications. The steps for both are show below

1. Clone repository and navigated to executables
2. Path to View Executable: /SpaceWarsExecutables/client/view.exe
3. Path to Server Exectuable: /SpaceWarsExecutables/server/server.exe
4. Double click on both, and the applications will run. 

### Game Logistics: Client
Once the client is running, you can specify an IP address to connect and the name you want you your ship to be. If there is a valid SpaceWars server running at the specified IP Address, the game will connect, and you can play as normal. Use the arrow keys to move the spaceship, and the spacebar to fire. The game simply allows multiple clients to connect to the same server, and will count the score as the game is played. To play client on the same computer that runs the server, use 'localhost' as the port. 

### Game Logistics: Server
The server is a bit more complicated. In the SpaceWarsExecutables/server folder you'll find a settings.xml file. In this file, you can specify what settings the applicatoin should run. You can edit them to be whatever you like, as long as it is valid XML, and you use only the tags that are specified. The most important thing to keep track of is the FancyGame mode. If you set that to "Yes" or 'Yes', the game will spawn four stars that orbit each other. If it is set to anything else, the game will spawn the stars as specified in the settings. 

Once the space wars application is up and running, it can connect to as many clients are as reasonable (it's recommended to not run more than 10 for gameplay purposes). The clients can connect to the IP address of the system that is running the server, and allow for multiplayer gameplay. 

### Final Thoughts and Acknowledgements. 

The game was not developed to be particularly robust by any means, so it's very likely that you'll run into bugs as you test it. We attempted to handle as many exceptions as we could think of in the time frame for the project, but as it always goes in software engineering, it's certainly not perfect :) 