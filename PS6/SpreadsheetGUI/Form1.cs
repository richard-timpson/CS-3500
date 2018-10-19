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
        AbstractSpreadsheet spread = new Spreadsheet(s=>true, s=> s.ToUpper(), "ps6");
        private PrintDocument pd = new PrintDocument();
        string fileName = null;
        bool saved = false;

        public Form1(string filepath)
        {
            InitializeComponent();

            if (filepath != null)
            {
                spread = new Spreadsheet(filepath, s => true, s => s.ToUpper(), "ps6");
                this.Text = filepath;
                fileName = filepath;
                saved = true;
            }

            // This an example of registering a method so that it is notified when
            // an event happens.  The SelectionChanged event is declared with a
            // delegate that specifies that all methods that register with it must
            // take a SpreadsheetPanel as its parameter and return nothing.  So we
            // register the displaySelection method below.

            // This could also be done graphically in the designer, as has been
            // demonstrated in class.
            spreadsheetPanel1.SetSelection(0, 0);
            CellName.Text = "A1";
            DisplayPanelOnOpen(spreadsheetPanel1);
            spreadsheetPanel1.SelectionChanged += DisplayControlsOnSelection;
            pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
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
            foreach (string cell in spread.GetNamesOfAllNonemptyCells())
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
        /// Changes the display of the cells after the value of a cell is set. Uses similar logic
        /// If will simply find the cells that need to be changed and change them. 
        /// If an exception is thrown when setting the contents of a cell, the function will show a message. 
        /// </summary>
        /// <param name="ss"></param>
        private void DisplayPanelOnSet(SpreadsheetPanel ss)
        {
            string contents = CellContents.Text;
            HashSet<string> CellsToChange = new HashSet<string>();
            try
            {
                CellsToChange = new HashSet<string>(spread.SetContentsOfCell(CellName.Text, contents));
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

            ss.GetSelection(out col, out row);

            CellName.Text = "" + Convert.ToChar(col + 65) + (row + 1);
            object contents = spread.GetCellContents(CellName.Text);
            string StringContents;
            if (contents.GetType() == typeof(Formula))
            {
                StringContents = "=" + (string)contents.ToString();
            }
            else
            {
                StringContents = contents.ToString();
            }
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
        /// Opening
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openNewFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            string filePath = openNewFileDialog1.FileName;
            DemoApplicationContext.getAppContext().RunForm(new Form1(filePath));
        }

        private void openNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openNewFileDialog1.ShowDialog();
        }


        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            string filePath = openFileDialog1.FileName;
            spreadsheetPanel1.Clear();
            spread = new Spreadsheet(filePath, s => true, s => s.ToUpper(), "ps6");
            this.Text = filePath;
            fileName = filePath;
            saved = true;
            DisplayControlsOnSelection(spreadsheetPanel1);
            DisplayPanelOnOpen(spreadsheetPanel1);
            spreadsheetPanel1.SelectionChanged += DisplayControlsOnSelection;
            CellContents.Select();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (spread.Changed)
            {
                DialogResult dialog = MessageBox.Show("Opening will result in loss of your data since the last save. Are you sure you wish to open a file? ", "Open", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    openFileDialog1.ShowDialog();
                }
            }
            else
            {
                openFileDialog1.ShowDialog();
            }
            
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            string filePath = saveFileDialog1.FileName;
            spread.Save(filePath);
            saved = true;
            fileName = filePath;
            this.Text = filePath;
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CellContents.Focused)
            {
                CellContents.Copy();
            }
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CellContents.Focused)
            {
                CellContents.Cut();
            }
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CellContents.Focused)
            {
                CellContents.Paste();
            }
        }

        private void viewHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DemoApplicationContext.getAppContext().RunForm(new HelpMenu());

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (spread.Changed)
            {
                DialogResult dialog = MessageBox.Show("Closing will result in loss of your data since the last save. Are you sure you wish to exit? ", "Exit", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saved == false)
            {
                saveFileDialog1.ShowDialog();
            }
            else
            {
                spread.Save(fileName);
            }
        }

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

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            if (printDialog1.ShowDialog() == DialogResult.OK)
            {
                CaptureScreen();
                pd.Print();
            }
        }

        Bitmap memoryImage;

        private void CaptureScreen()
        {
            Graphics myGraphics = this.CreateGraphics();
            Size s = this.Size;
            memoryImage = new Bitmap(s.Width, s.Height, myGraphics);
            Graphics memoryGraphics = Graphics.FromImage(memoryImage);
            memoryGraphics.CopyFromScreen(this.Location.X + 8, this.Location.Y, 0, -90, s);
        }

        private void pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(memoryImage, 0, 0);
        }

        private void printPreviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CaptureScreen();
            printPreviewDialog1.Document = pd;
            printPreviewDialog1.ShowDialog();
        }

    }
}
