using SS;
using SpreadsheetUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadsheetGUI
{
    public partial class Form1 : Form
    {
        AbstractSpreadsheet spread = new Spreadsheet();

        public Form1(string filepath)
        {
            if (filepath != null)
            {
                spread = new Spreadsheet(filepath, s => true, s => s.ToUpper(), "ps6");

            }

            InitializeComponent();

            // This an example of registering a method so that it is notified when
            // an event happens.  The SelectionChanged event is declared with a
            // delegate that specifies that all methods that register with it must
            // take a SpreadsheetPanel as its parameter and return nothing.  So we
            // register the displaySelection method below.

            // This could also be done graphically in the designer, as has been
            // demonstrated in class.

            spreadsheetPanel1.SetSelection(0, 0);
            CellName.Text = "A1";
            spreadsheetPanel1.SelectionChanged += displaySelection;
            
            CellContents.Focus();

        }

        // Every time the selection changes, this method is called with the
        // Spreadsheet as its parameter.

        private void displaySelection(SpreadsheetPanel ss)
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

            ss.SetValue(col, row, GetCellValueAsString(CellName.Text));

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


        private void displayRecalculated(SpreadsheetPanel ss)
        {

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

        private void CellContents_TextChanged(object sender, EventArgs e)
        {
            string contents = CellContents.Text;
            spread.SetContentsOfCell(CellName.Text, contents);
            displaySelection(spreadsheetPanel1);
        }

        private void closeToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Close();
        }

        // Deals with the New menu
        private void newToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            // Tell the application context to run the form on the same
            // thread as the other forms.
            DemoApplicationContext.getAppContext().RunForm(new Form1(null));
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            string filePath = openFileDialog1.FileName;
            DemoApplicationContext.getAppContext().RunForm(new Form1(filePath));
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void CellContents_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                string contents = CellContents.Text;
                spread.SetContentsOfCell(CellName.Text, contents);
                
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
    }
}
