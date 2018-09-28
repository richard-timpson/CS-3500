using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using SpreadsheetUtilities;

namespace SS
{
    public class Spreadsheet : AbstractSpreadsheet
    {
        public Spreadsheet()
        {

        }

        private Dictionary<string, Cell> NonemptyCells = new Dictionary<string, Cell>();
        private DependencyGraph graph = new DependencyGraph();
        public override IEnumerable<String> GetNamesOfAllNonemptyCells()
        {
            List<string> NonemptyCellNames = new List<string>();

            foreach (KeyValuePair<string, Cell> cell in NonemptyCells)
            {
                NonemptyCellNames.Add(cell.Key);
            }
            return NonemptyCellNames;
        }


        public override object GetCellContents(string name)
        {
            if (IsValidName(name))
            {
                Cell cell = NonemptyCells[name];
                object Contents = cell.Contents;
                return Contents;
            }
            else
                throw new InvalidNameException();
        }


        public override ISet<String> SetCellContents(String name, double number)
        {
            string type = "double";
            return SetCellContentsActual(name, number, type);
        }
        

        public override ISet<String> SetCellContents(String name, String text)
        {
            if (text == null)
                throw new ArgumentNullException();
            else
            {
                string type = "string";
                return SetCellContentsActual(name, text, type);
            }
        }


        public override ISet<String> SetCellContents(String name, Formula formula)
        {
            if (formula == null)
                throw new ArgumentNullException();
            else
            {
                string type = "formula";
                return SetCellContentsActual(name, formula, type);
            }
        
    }


        protected override IEnumerable<String> GetDirectDependents(String name)
        {
            if (name.Equals(null))
                throw new ArgumentNullException();
            else if (!IsValidName(name))
            {
                throw new InvalidNameException();   
            }
            else
                return graph.GetDependents(name);

        }
        private ISet<string> SetCellContentsActual(String name, object ObjectContents, string type)
        {
            // If valid name 
            if (IsValidName(name))
            {
                //If cell already exsits (is not empty), we need to set the cell and get rid of dependee's. 
                //Dependencies should stay the same.
                if (IsNonemptyCell(name))
                {
                    //if it's a formula, replace the dependents and dependees with the new variables
                    if (type == "formula")
                    {

                    }
                    //if it's not a formula, replace the dependents and dependess with empty sets
                    //set the value of the cell. 
                    SetContentByType(name, ObjectContents, type);
                }
                else
                {
                    //set the cell contents based on the type given
                    SetContentByType(name, ObjectContents, type);
                }
            }
            //return ISet including all dependents, both direct and indirect, of named cell. 
            ISet<string> AllDependents = new HashSet<string>(GetCellsToRecalculate(name));

            return AllDependents;
        }

        private void SetContentByType(string name, object ObjectContents, string type)
        {
            if (type == "double")
            {
                double contents = (double)ObjectContents;
                SetContentsToDouble(name, contents);
            }
            else if (type == "string")
            {
                string contents = (string)ObjectContents;
                Cell cell = new Cell(contents);
                NonemptyCells[name] = cell;
            }
            else if (type == "formula")
            {

                //string contents = Convert.ToString(ObjectContents);
                Cell cell = new Cell((Formula)ObjectContents);
                NonemptyCells[name] = cell;
            }
        }

        private void SetContentsToDouble(string name, double number)
        {
            Cell cell = new Cell(number);
            NonemptyCells[name] = cell;
        }
        private void SetContentsToString(string name, string text)
        {
            Cell cell = new Cell(text);
            NonemptyCells[name] = cell;
        }
        private void SetContentsToFormula(string name, Formula formula)
        {
            Cell cell = new Cell(formula);
            NonemptyCells[name] = cell;
        }
        private bool IsValidName(string name)
        {
            String varPattern = @"^[a-zA-Z_](?:[a-zA-Z_]|\d)*$";
            if (name != null && Regex.IsMatch(name, varPattern))
                return true;
            else
                throw new InvalidNameException();
        }
        private bool IsNonemptyCell(string name)
        {
            IEnumerable<string> NonemptyCellNames = GetNamesOfAllNonemptyCells();
            foreach (string s in NonemptyCellNames)
            {
                if (name == s)
                    return true;
            }
            return false;
        }

    }
    class Cell
    {
        private object CellContents;
        public object Contents
        {
            get
            {
                return CellContents;
            }
        }
        public Cell(string contents)
        {
            CellContents = contents;
        }
        public Cell(double contents)
        {
            CellContents = contents;
        }
        public Cell(Formula contents)
        {
            CellContents = contents;
        }
    }


}
