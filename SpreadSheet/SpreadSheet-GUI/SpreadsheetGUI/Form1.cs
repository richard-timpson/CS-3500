using SS;
using SpreadsheetUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadsheetGUI
{
    public partial class Form1 : Form
    {
        //the underlying storage for the spreadsheet
        AbstractSpreadsheet spread = new Spreadsheet(s=>true, s=> s.ToUpper(), "ps6");

        //An object that exists for printing the screen of the spreadsheet 
        private PrintDocument pd = new PrintDocument();

        // The name of the file that is displayed at the top of the GUI
        string fileName = null;

        //Keeping track of the state of the spreadsheet
        bool saved = false;

        public Form1(string filepath)
        {
            InitializeComponent();
            //make sure the file path is not null before intializing the spreadsheet
            if (filepath != null)
            {
                spread = new Spreadsheet(filepath, s => true, s => s.ToUpper(), "ps6");
                this.Text = filepath;
                fileName = filepath;
                saved = true;
            }

            //initializing the state of the spreadsheet by 
            // setting the selection to the first cell
            spreadsheetPanel1.SetSelection(0, 0);

            // Setting the textbox for the name to A1
            CellName.Text = "A1";

            // Displaying the cells
            DisplayPanelOnOpen(spreadsheetPanel1);

            //Adding the displayControlsOnSelection as a listener to the event handler for the panel
            spreadsheetPanel1.SelectionChanged += DisplayControlsOnSelection;

            // adding the function pd_PrintPage to the event handler pd.PrintPage, so that pd_PrintPage will be called
            // when the event is triggered. 
            pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);

            // Setting the cursor to the textbox for cell contents. 
            CellContents.Select();

        }

        // Every time the selection changes, this method is called with the
        // Spreadsheet as its parameter.

        /// <summary>
        /// This function will display the panels when the file is first opened. 
        /// </summary>
        /// <param name="ss"></param>
        private void DisplayPanelOnOpen(SpreadsheetPanel ss)
        {
            // Looping through all of the non empty cells
            foreach (string cell in spread.GetNamesOfAllNonemptyCells())
            {
                // setting the display of the cells to the empty cells. 
                int cellCol = cell[0];
                cellCol -= 65;
                string cellRowStr = cell.Substring(1);
                int.TryParse(cellRowStr, out int cellRow);
                cellRow -= 1;

                ss.SetValue(cellCol, cellRow, GetCellValueAsString(cell));
            }
        }

        /// <summary>
        /// Changes the display of the cells after the value of a cell is set. Uses similar logic
        /// If will simply find the cells that need to be changed and change them. 
        /// If an exception is thrown when setting the contents of a cell, the function will show a message. 
        /// </summary>
        /// <param name="ss"></param>
        private void DisplayPanelOnSet(SpreadsheetPanel ss)
        {
         
            string contents = CellContents.Text;
            HashSet<string> CellsToChange = new HashSet<string>();
            // try getting the cells to change. If there is an error when processing the cell changes, 
            // it will catch the exceptions and show a message. 
            try
            {
                // this is where the exeception should throw. Getting only the cells whose value should change when the content 
                // of the current cell is set. 
                CellsToChange = new HashSet<string>(spread.SetContentsOfCell(CellName.Text, contents));
                // If the exception doesn't throw, set the CellValue input box to be the value of the currently selected cell. 
                CellValue.Text = spread.GetCellValue(CellName.Text).ToString();
            }
            catch (FormulaFormatException E)
            {
                MessageBox.Show("Invalid Formula");
            }
            catch (InvalidNameException)
            {
                MessageBox.Show("Invalid Formula");
            }

            if (contents == "")
            {
                CellsToChange.Add(CellName.Text);
            }

            // Loop through CellsToChange if  it exists, and updating the displayed values of the cells. 
            foreach (string cell in CellsToChange)
            {
                int cellCol = cell[0];
                cellCol -= 65;
                string cellRowStr = cell.Substring(1);
                int.TryParse(cellRowStr, out int cellRow);
                cellRow -= 1;

                ss.SetValue(cellCol, cellRow, GetCellValueAsString(cell));
            }
        }


        /// <summary>
        /// This function will update the values that are displayed in the control forms at the top of the SS
        /// It is called every time a new cell is selected. 
        /// </summary>
        /// <param name="ss"></param>
        private void DisplayControlsOnSelection (SpreadsheetPanel ss)
        {
            int row, col;
            // getting the current cell. 
            ss.GetSelection(out col, out row);

            // setting the text value of the CellName input to be the current selection
            CellName.Text = "" + Convert.ToChar(col + 65) + (row + 1);

            // getting the contents of the current cell 
            object contents = spread.GetCellContents(CellName.Text);

            //StringContents exists as the actual display string. contents exists as just a placeholder. 
            string StringContents;
            // if it is a formula, convert the contents to a string and prepend it with a '='
            if (contents.GetType() == typeof(Formula))
            {
                StringContents = "=" + (string)contents.ToString();
            }
            // else just convert it to a string
            else
            {
                StringContents = contents.ToString();
            }
            // set the inputs at the top to hold the correct values. 
            CellContents.Text = StringContents;
            CellValue.Text = GetCellValueAsString(CellName.Text);
        }


        /// <summary>
        /// Helper method to display cell value
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        private string GetCellValueAsString(string cell)
        {

            object value = spread.GetCellValue(cell);

            if (value.GetType() == typeof(FormulaError))
            {
                return "Formula Error";
            }
            else
            {
                return value.ToString();
            }
        }

        /// <summary>
        /// This is the function that is called when the contents of a cell is changed by clicking the set button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CellContents_Click(object sender, EventArgs e)
        {
            DisplayPanelOnSet(spreadsheetPanel1);
        }

        /// <summary>
        /// This is the function that is called every time a key press happens inside of the cell contents
        /// It will check if the key pressed is enter. If it is, it will update the values of the cell. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CellContents_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                DisplayPanelOnSet(spreadsheetPanel1);

                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        /// <summary>
        /// Closing the file when the close button is clicked on the tool strip
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Opening a new spreadsheet in a new window when 'new' in toolbar is clicked. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            // Tell the application context to run the form on the same
            // thread as the other forms.
            DemoApplicationContext.getAppContext().RunForm(new Form1(null));
        }


        /// <summary>
        /// Opens a file dialog, that when selected, will open an existing file in a new window. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openNewFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            try
            {
                string filePath = openNewFileDialog1.FileName;
                DemoApplicationContext.getAppContext().RunForm(new Form1(filePath));
            }
            catch
            {
                MessageBox.Show("There was an error opening the file.  Please make sure that the filepath is correct, and that the file is a valid spreadsheet file.");
            }
        }


        /// <summary>
        /// Calls the open new dialog. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openNewFileDialog1.ShowDialog();
        }

        /// <summary>
        /// Creates a file dialog that allows an existing file to be opened in the same window 
        /// It will clear the contents of the existing window, and populate the window with the contents
        /// of the spreadsheet to be opened. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            try
            {

                // getting the filename from the file dialog
                string filePath = openFileDialog1.FileName;

                // emptying the spreadsheet
                spreadsheetPanel1.Clear();

                //creating a new spreadsheet from the specified file path
                spread = new Spreadsheet(filePath, s => true, s => s.ToUpper(), "ps6");

                this.Text = filePath;

                // updating the name of the spreadsheet window
                fileName = filePath;
                saved = true;

                // setting the displays of the input boxes at the top
                DisplayControlsOnSelection(spreadsheetPanel1);

                // setting the display of the panels
                DisplayPanelOnOpen(spreadsheetPanel1);

                // adding the DisplayControlsOnSelection to event handler for the spreadsheet panel
                spreadsheetPanel1.SelectionChanged += DisplayControlsOnSelection;

                // setting the cursor to the CellContents input box
                CellContents.Select();
            }
            catch
            {
                MessageBox.Show("There was an error opening the file.  Please make sure that the filepath is correct, and that the file is a valid spreadsheet file.");
            }
        }


        /// <summary>
        /// Calls the openFileDialog function to open an existing spreadsheet in the current window
        /// If the current file has not been saved, it will prompt the user to make sure they want to exit. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // checking to make sure that the user is not getting rid of unsaved work. 
                if (spread.Changed)
                {
                    DialogResult dialog = MessageBox.Show("Opening will result in loss of your data since the last save. Are you sure you wish to open a file? ", "Open", MessageBoxButtons.YesNo);
                    if (dialog == DialogResult.Yes)
                    {
                        // opening the file dialog
                        openFileDialog1.ShowDialog();
                    }
                }
                else
                {
                    // opening the file dialog. 
                    openFileDialog1.ShowDialog();
                }
            }
            catch
            {
                MessageBox.Show("There was an error opening the file. Please make sure that the filepath is correct, and that the file is a valid spreadsheet file.");
            }
            
        }
        /// <summary>
        /// Calls the saveFileDialog for creating a dialog to save the file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
        }

        /// <summary>
        /// Opens a file dialog that will save the specificed file from the save dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            try
            {
                // getting the filepath from the saveFileDialog
                string filePath = saveFileDialog1.FileName;
                // saving the contents to the spreadsheet object
                spread.Save(filePath);
                saved = true;

                // setting the name of the spreadsheet window to the file path
                fileName = filePath;
                this.Text = filePath;
            }
            catch
            {
                MessageBox.Show("There was an error while saving your file. Please check the filepath and try again.");
            }
        }

        /// <summary>
        /// Copying the CellContents textbox if it is selected. 
        /// Called on the click of the copy in the menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CellContents.Focused)
            {
                CellContents.Copy();
            }
        }

        /// <summary>
        /// Cutting the CellContents textbox if it is selected
        /// Called on the click of the cut in the menu. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CellContents.Focused)
            {
                CellContents.Cut();
            }
        }
        /// <summary>
        /// Pasting into the CellContents textbook if it is selected. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CellContents.Focused)
            {
                CellContents.Paste();
            }
        }

        /// <summary>
        /// Opening the help menu as a separate form. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void viewHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DemoApplicationContext.getAppContext().RunForm(new HelpMenu());

        }
        /// <summary>
        /// Function called on the close of the window. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Checking if there is content that has not been saved. If it has, show a warn box. 
            if (spread.Changed)
            {
                DialogResult dialog = MessageBox.Show("Closing will result in loss of your data since the last save. Are you sure you wish to exit? ", "Exit", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }

        }
        /// <summary>
        /// Saving the file on the click of the saved menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // If the spreadsheet has not been saved already, open the save dialog
            if (saved == false)
            {
               
                saveFileDialog1.ShowDialog();
            }
            // If it has been saved already, just save the file, and don't show the dialog. 
            else
            {
                try
                {
                    spread.Save(fileName);
                }
                catch
                {
                    MessageBox.Show("There was an error while saving your file. Please check the filepath and try again.");
                }
            }
        }
        /// <summary>
        /// Fuction that is used for selected the cells on an arrow press. 
        /// It will move the selection based on what key was pressed. 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            spreadsheetPanel1.GetSelection(out int col, out int row);
            if (keyData == Keys.Down)
            {
                spreadsheetPanel1.SetSelection(col, row + 1);
                DisplayControlsOnSelection(spreadsheetPanel1);
                return true;
            }
            if (keyData == Keys.Up)
            {
                spreadsheetPanel1.SetSelection(col, row - 1);
                DisplayControlsOnSelection(spreadsheetPanel1);
                return true;
            }
            if (keyData == Keys.Left)
            {
                spreadsheetPanel1.SetSelection(col - 1, row);
                DisplayControlsOnSelection(spreadsheetPanel1);
                return true;
            }
            if (keyData == Keys.Right)
            {
                spreadsheetPanel1.SetSelection(col + 1, row);
                DisplayControlsOnSelection(spreadsheetPanel1);
                return true;
            }
            else
            {
                return base.ProcessCmdKey(ref msg, keyData);
            }
        }
        /// <summary>
        /// Fuction that is called when the print key is clicked. 
        /// It will capture the screen and use the PrintDocument object to save the screenshot
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // If the result of the dialog is ok
            if (printDialog1.ShowDialog() == DialogResult.OK)
            {
                // Capture the screen and print it. 
                CaptureScreen();
                pd.Print();
            }
        }

        // A bit map for storing the image. 
        Bitmap memoryImage;

        /// <summary>
        /// Fuction for capturing the screen of the spreadsheet window
        /// </summary>
        private void CaptureScreen()
        {
            Graphics myGraphics = this.CreateGraphics();
            Size s = this.Size;
            memoryImage = new Bitmap(s.Width, s.Height, myGraphics);
            Graphics memoryGraphics = Graphics.FromImage(memoryImage);
            memoryGraphics.CopyFromScreen(this.Location.X + 8, this.Location.Y, 0, -90, s);
        }

        /// <summary>
        /// Fuctino for actually printing the page. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(memoryImage, 0, 0);
        }

        /// <summary>
        /// Function for handling the print preview menu item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void printPreviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Capturing the screen
            CaptureScreen();

            // Dialog for holding the document from the print screen
            printPreviewDialog1.Document = pd;

            // showing the dialog. 
            printPreviewDialog1.ShowDialog();
        }

    }
}
