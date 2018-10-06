Richard Timpson
10/2/18
CS 3500 Software Practice
PS5


************
Beginning the assignment on Tuesday 10/2/18. I believe that the assignment should take about 12 hours to complete. 
Time beginning on the project for Tuesday 10/2/18: 3:30 pm

I've already read through the specifications for the assignment
I'm going to begin by going through each of the functions one at a time, writing them, and then testing them.
For PS4, I wrote all of the functions first and then tested them, and I don't think that was the right thing to do


Constructor:
Need to understand how constructors work with Abstract classes. As of now I don't understand the specs for the assignment 
in regard to the constructor. 

Figured out constructor. Need to think of test cases. 

Properties:
Working on the properties defined in the abstract class but I don't understand what their purpose is
Leaving all of the gets and sets as they were defined for now

GetSavedVersion:
Have the function to a point where I think it's working. Need to test it with the Save function. 
Going to wait till I have other functions written to write and test this, so that I can do it with actual values. 

GetCellValue:
Need to work on SetCellValue before I can test

SetContentsofCell:
This is most likely going to be a refactored version of the SetContentsActual I wrote for PS4
Need to figure out the IsValid function before I can correctly add values and tests

End of session 10/2/18 6:00 PM. Time spent 2:30 hours. Total Time: 2:30 hours]

*************


*************
Starting session 10/3/18 @ 9:30 Am. 
still need to work on the delegate functions declared in the Abstract spreadsheet class. 
Going to take a look at the example posted on XML as well. 

Was working on the contents fuction before I had to quit. 

Ended session 10/3/18 @10:30 AM. Time spent 1 Hours. Total Time: 3:30 hours
***************

*****************
Beginning session 10/3/18 @8:00 PM
Need to finish the contents function 
Finished set cell contents and get cell value, but haven't tested. Working on google doc to list all test cases

Paused Session 10/3/18 @ 8:30 PM
Started Session 10/3/18 @ 9:00 PM

Moved the old unit tests over so that they work with the new setcontents of cell function
Having a problem with the regex for new valid variables. Need to get it working

Ended Session 10/3/18 @ 10:00 pm Time Spent: 1:30 Hours. Total Time: 5 Hours
********************


********************
Started Session 10/4/18 @10:45 AM
Got the regex working, with some good tests. 

Have the setcontents of cell written and tested. 

Almost have the value of the formula working. 

Ended Session 10/4/18 @12:15 PM. Time spent 1:30. Total Time 6:30 hours. 
**********************



***********************
Started Session 10/4/8/18 @3:30 pm. 
Need to finish the values for formulas. 
Still not sure what to do about getvalue for emtpy cell. 
Need to work on the IsValid and Normalize delegates. 

Figured out how to handle different types of cell values for the getcellvalue function
Everything is tested well so far. 

Need to start working on the Save and GetSaved methods. 

Working on the xml reader and writer. 

Endend Session 10/4/18 @ 7:00 pm. Time spent 3:30 hours. Total Time 10 hours
**********************

*******************
Started Session 10/4/18 @8:30 pm

finished writing the XML classes, but wasn't able to test anything because
Visual studio is having issues on my laptop. going to finish tomorrow

ended Session 10/4/18 @9:30 pm. Time Spent 1 hour. Total Time 11 hours
************************

**************************
Started Session 3:00 PM
Finished writing read function for xml
Working on testing xml read and write

Finished the xml read and write. Realized that I needed to write the set contents of cell so that it recalculates all of the cells
Decided to take a late day and finish tomorrow

Ended Session 8:30 pm. Time Spent 5:30 hours. Total Time 16:30 hours
*************************

***********************
Started session 10/6/18 @ 9:45 Am
need to rewrite the set contents of cell to get it to recalculate all cells
Also need to make sure that all code is refactored, tested well, and is documented
Hoping to finish today in the next 2 hours. 


Pseudocode for setcontents of cell fuction

check if null or invalid, throw exception
if it is a double, change the contents (and value of the cell) to a double, and recalculate all other cells
If it is a string, change the contents(and value of the cell) to a string, and recalculate all other cells
If it a string starts with equal(make sure that you can parse as a formula). If changing formula would cause circular dependency, throw error. 
Otherwise, chang the contents to a formula, and recalculate the value of all other cells. 
