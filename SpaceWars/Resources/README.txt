CS 3500 - Software Practice
Assignment : SpaceWars
Richard Timpson and Johnathan Smith
12/6/18

Welcome to Space Wars! 
The game should function as outlined in the assignment specs. For our server implemntation, we just added one project
named server, and wrote all of our code to handle the networking call backs and the game physics. As of now, 
the server is fairly optimized. Our view seems to have performance issues, so when you run the game with >10 clients
it begins to lag. That's a problem for PS7 however, so we are ignoring it. We created a client class and a dictionary
that holds a list of clients using their ID as the key. The client class holds the command coming from the socket, 
as well as boolean flags for turning, firing, and thrusting, that are used to inform the logic for the game physics. 

As far as our model goes, we have updated the model from PS7 to hold vectors that allow us to perform the physics calcuations. 
We modified the data structures to be mostly dictionaries (with ID's as keys), so that we could access the various parts of the 
model in constant time instead of linear time (which was happening when they were lists). This increased our performance quite a bit. 

Our extra game feature implments multiple stars that rotate around the center of the world. This can be activated by specifying it in the settings.xml file
Set the "FancyGame" property to "Yes" or "yes", for the game mode to activate, otherwise it won't. It took us a while to figure out
the trig calculations for the orbit, but once we did, it was fairly simple to add the lines of code it to make it work. We were hoping
to get it working so that you could dynamically specify the number of stars to orbit, but we didn't have the time to implement it. 

Our unit tests are by no means robust, but we made sure to hit as much code coverage as we could to verify that all of our code was running without any errors. 
Obviously, anything that has to do with networking or asynchronous callbacks, we couldn't tests. The tests cover all of our code that has to do with the model
and it's calcuations. 

Other than that, the game logic and networking logic are pretty standar, and the game works well. We had a ton of fun implementing the game, 
and learned an immense amount. 

The following shows our logs that tracked our progress as we worked our way through, beginning with the start of PS7


We are just beginning the assignment. We have decided to outline the folder structurs as follows

Solution:  SpaceWars
	Projects:
		Resources
			Library
			Graphics
		NetworkController
			NetworkControllerClass
		GameController
			GameControllerClass
		GameModel
			World
			Ship
			Projectile
			Start
		View
			Form1
			Program

We decided to put all of the models as classes inside of the same project, to make things simpler. We may need to change this in the future. 
We are hoping to get the project outlined and everythign pipelined together to begin with, and then begin coding the main logic. 


*********************************
11/12/18

We have most of the networking code finished, and objects displaying. We are running into some issues 
with the gamecontroller talking with the view to draw the objects. 

As of now, the plan is to have the following organization

Create the world when the game is initialized, inside of the game controller
Every time a full message is received, the world will update it's objects, 
and return a deep copy of the objects to the view, through the WorldUpdated Event. 
When this event is triggered, the view will then draw the copy of the objects. 

Potential problems may be that the code takes too long to update the objects,copy them, and 
draw them in the time that the next message is processed. We may also have potential race conditions.

We also need to figure out how to send messages for fire and move. 
*********************************

11/14/18

As of now, the data structure for the objects is a list. 
We may want to switch to a hashset depending on the performance, 
especially for the projectiles. 

**********************************
11/15/18
We have the networking controller working well enough, that the ships are
being displayed properly. 

**********************************

11/17/18
Added an explosion class for creating explosions objects upon death. Runs on a frame counter that expires
after 130 frames. I made about 20 seperate flame and smoke images in photoshop, and they are run in sequence
for 5 frames each. I implemented a random number generator that randomly alters the size of the explosion image,
which helps simulate motion.

**********************************

11/18/18
The explosion animation, as it is now, is resource intensive when there are a dozen or so AI
clients. The framerate may drop slightly. I made a number of optimazations, including formating the pictures
upon initialization of the drawingPanel, but there is still a lot of overhead for the animation, because they
have to be resized on every frame. This is due to the random number generator choosing the size of the image 
drawn for the explosion. (This is critiical for the animation as it gives it the appearance that there is motion
and fluidity between frames). A possible solution to make it even faster would be to simulate the randomness of 
the size of the image by preloading a bunch of different sizes for each image, but ultimately we ran out of 
time to implement it.

Other than that, everything works perfectly.


**********************************

12/1

We have started the project and have most of the networking code finished. 
The server is at the point where we can connect multiple clients and get their name
We are working on creating a world and sending it back to the clients. 
As of now our plan is get the world created and sending back to clients with the correct json
strings by the end of the day. We are going to use a random number generator to create the 
random number positions of the ships when the client connects. 
Once we get the random positions we will have everything we need to start listening for moves,
and then we can start working on the physics. 

*****************************
12/5

We have most of the core functionality working. We can connect with multiple clients
and the game physics are working for the most part. As of now, this our to-do list

Unit test server code
Extra game mode
Error Handling/ clients disconnecting. 
Update settings file
Document code and finalize ReadMe. 

We are thinking of implementing a "brick breaker" extra game mode, but we may not have time to do it.
For now we are going to focus on unit tests and error handling. 



