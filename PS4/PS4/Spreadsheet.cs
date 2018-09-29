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
                HashSet<string> NamesOfCells = new HashSet<string>(GetNamesOfAllNonemptyCells());
                if (NamesOfCells.Contains(name))
                {
                    Cell cell = NonemptyCells[name];
                    object Contents = cell.Contents;
                    return Contents;
                }
                else
                {
                    return "";
                }
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
                // Get current cells variables, if it is a formula
                object CurrentCellContents = GetCellContents(name);
                List<string> CurrentDependees = new List<string>();

                if (CurrentCellContents.GetType() == typeof(Formula))
                {
                    Formula CurrentCellFormula = (Formula)CurrentCellContents;
                    CurrentDependees = new List<string>(CurrentCellFormula.GetVariables());
                }

                //Get new formula's variables. 
                List<string> ReplaceDependees = new List<string>();
                if (type == "formula")
                {
                    Formula formula = (Formula)ObjectContents;
                    ReplaceDependees = new List<string>(formula.GetVariables());
                }

                //replace the dependees 
                graph.ReplaceDependees(name, ReplaceDependees);

                //Try getting the cells to recalculate
                try
                {
                    //Checking for circular dependency
                    ISet<string> AllDependents = new HashSet<string>(GetCellsToRecalculate(name));

                    //If exception wasn't caught, set the content to new object, and return AllDependents
                    SetContentByType(name, ObjectContents, type);
                    return AllDependents;
                }
                //If exception is caught
                catch(CircularException E)
                {
                    //replace dependees with old variables
                    graph.ReplaceDependees(name, CurrentDependees);

                    //throw the exception again
                    throw new CircularException();
                }
            }
            throw new InvalidNameException();
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
                SetContentsToString(name, contents);
            }
            else if (type == "formula")
            {
                Formula contents = (Formula)ObjectContents;
                SetContentsToFormula(name, contents);
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
