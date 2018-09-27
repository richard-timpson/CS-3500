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
            if (name == null || IsValidName(name))
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
            // If valid name 
            if (IsValidName(name))
            {
                //If cell already exsits (is not empty), we need to set the cell and update dependencies
                if (IsNonemptyCell(name))
                {
                    IEnumerable<string> RecalculateCells = GetCellsToRecalculate(name);
                    foreach (string s in RecalculateCells)
                    {

                    }
                    //get all the dependee's of cell
                    //remove all of the dependee's of cell
                }
                //If cell does not already exist
                else
                {
                    // make a new cell 
                    Cell NewCell = new Cell(number);
                    //Add it to list of cells. 
                    NonemptyCells.Add(name, NewCell);
                }
            }
            //return ISet including all dependents, both direct and indirect, of named cell. 
            HashSet<string> AllDependents = new HashSet<string>();
            HashSet<string> CurrentDependents = GetDirectDependents(name);

            //get all dependents of Cell
        }


        public override ISet<String> SetCellContents(String name, String text)
        {
            throw new NotImplementedException();
        }


        public override ISet<String> SetCellContents(String name, Formula formula)
        {
            throw new NotImplementedException();
        }


        protected override IEnumerable<String> GetDirectDependents(String name)
        {
            if (IsValidName(name))
                return graph.GetDependents(name);
            else if (name == null)
                throw new ArgumentNullException();
            else
                throw new InvalidNameException();
        }
        private bool IsValidName(string name)
        {
            String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            if (!Regex.IsMatch(name, varPattern))
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
            CellContents = contents.ToString();
        }
        public Cell(Formula contents)
        {
            CellContents = contents;
        }
    }


}
