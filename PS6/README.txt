Richard Timpson and Jonathon Smith
PS6
CS 3500

Log 10/11/18 @ 3:40 pm. 

Jon has done most of the work to get the spreadsheet working as it is. So far we have the file menus working good, and the editing of cells working good. 

As of now this our ToDo list

1. Get the save working - Jon
2. Create help menu for TA's - Richard: Got the menu to open, need to add text inside of it. 
3. Error messages for exiting without saving - Jon
4. Error message for invalid formula - Richard: Finished.
5. Bug where you can't delete contents of cell - Richard 

For the added options, we are hoping to make it so that you can move cells with the arrow key, edit cells without having to click in contents box
save values on enter - Done 10/11/18 11:00 PM

select multiple cells for copy and paste, and possibly a undo/redo button. 

We also need to come up with a list of user tests to make sure functionality is working. 

We need to change PS4 so that opening file does not set Changed value to True. - Fixed 10/11/18 @ 11:07 PM

Currently, cells out of range return a formulaError.. needs to throw an invalid formula exception. - Fixed 10/11/18 @ 11:33 PM

Need to change the Open button to Open New.. and create an Open button that replaces the currently open window. - Done Jon 10/12/18 8:00 AM

Add error handeling for opening and saving file issues.
