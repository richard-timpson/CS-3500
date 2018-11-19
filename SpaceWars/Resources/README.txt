CS 3500 - Software Practice
Assignment 7: SpaceWars
Richard Timpson and Johnathan Smith
11/8/18

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
The game is mostly working. We just need to refactor the code a little bit and possibly add a small help menu. 
The error handling is working well. 

