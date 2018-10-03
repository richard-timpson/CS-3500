﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

using SpreadsheetUtilities;

namespace SS
{
    public class Spreadsheet : AbstractSpreadsheet
    {
        //making the main storage for the spreadsheet a dictionary with the name as the key, and a cell class as the value
        private Dictionary<string, Cell> NonemptyCells = new Dictionary<string, Cell>();

        //A dependency graph to keep track of cells
        private DependencyGraph graph = new DependencyGraph();

        public override bool Changed 
        {
            get;
            protected set;
        }

        public Func<string, bool> IsValid
        {
            get;
            protected set;
        }

        public Func<string, bool> Normalize
        {
            get;
            protected set;
        }

        public string Version
        {
            get;
            set;
        }

        public Spreadsheet() : base(s=> true, s => s, "default")
        {

        }
        public Spreadsheet(Func<string, bool> isValid, Func<string, string> normalize, string version) : base(isValid, normalize, version)
        {

        }
        public Spreadsheet(string path, Func<string, bool> isValid, Func<string, string> normalize, string version) : base(isValid, normalize, version)
        {

        }

        public override string GetSavedVersion(string filename)
        {
            try
            {
                using (XmlReader reader = XmlReader.Create(filename))
                {
                    while (reader.ReadToFollowing("spreadsheet"))
                    {
                        string version = reader.GetAttribute("version");
                        return version;
                    }
                    throw new SpreadsheetReadWriteException("Version not saved");
                }
            }
            catch (XmlException e)
            {
                throw new SpreadsheetReadWriteException(e.Message);
            }
        }

        public override void Save(string filename)
        {
            
            throw new NotImplementedException();
        }

        public override object GetCellValue(string name)
        {
            //check if valid name
            if (IsValidName(name))
            {
                //If the name is an exsiting cell
                HashSet<string> NamesOfCells = new HashSet<string>(GetNamesOfAllNonemptyCells());
                if (NamesOfCells.Contains(name))
                {
                    //return it's contents
                    Cell cell = NonemptyCells[name];
                    object value = cell.Value;
                    return value;
                }
            }
            else
                throw new InvalidNameException();


            throw new NotImplementedException();
        }


        public override IEnumerable<String> GetNamesOfAllNonemptyCells()
        {
            List<string> NonemptyCellNames = new List<string>();

            //looping through the keys of the nonempty cells dictionary
            foreach (KeyValuePair<string, Cell> cell in NonemptyCells)
            {
                NonemptyCellNames.Add(cell.Key);
            }
            return NonemptyCellNames;
        }


        public override object GetCellContents(string name)
        {
            //check if valid name
            if (IsValidName(name))
            {
                //If the name is an exsiting cell
                HashSet<string> NamesOfCells = new HashSet<string>(GetNamesOfAllNonemptyCells());
                if (NamesOfCells.Contains(name))
                {
                    //return it's contents
                    Cell cell = NonemptyCells[name];
                    object Contents = cell.Contents;
                    return Contents;
                }
                else
                {
                    //return the empty string
                    return "";
                }
            }
            else
                throw new InvalidNameException();
        }

        public override ISet<String> SetContentsOfCell(String name, String content)
        {
            throw new NotImplementedException();
        }

        protected override ISet<String> SetCellContents(String name, double number)
        {
            string type = "double";
            return SetCellContentsActual(name, number, type);
        }
        

        protected override ISet<String> SetCellContents(String name, String text)
        {
            if (text == null)
                throw new ArgumentNullException();
            else
            {
                string type = "string";
                return SetCellContentsActual(name, text, type);
            }
        }


        protected override ISet<String> SetCellContents(String name, Formula formula)
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

        /// <summary>
        /// This function exists as helper for SetContents to set based on the type
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ObjectContents"></param>
        /// <param name="type"></param>
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
                if (contents != "")
                    SetContentsToString(name, contents);
            }
            else if (type == "formula")
            {
                Formula contents = (Formula)ObjectContents;
                SetContentsToFormula(name, contents);
            }
        }

        /// <summary>
        /// Helper method for SetCell contents
        /// </summary>
        /// <param name="name"></param>
        /// <param name="number"></param>
        private void SetContentsToDouble(string name, double number)
        {
            Cell cell = new Cell(number);
            NonemptyCells[name] = cell;
        }

        /// <summary>
        /// Helper method for SetCell contents
        /// </summary>
        /// <param name="name"></param>
        /// <param name="number"></param>
        private void SetContentsToString(string name, string text)
        {
            Cell cell = new Cell(text);
            NonemptyCells[name] = cell;
        }

        /// <summary>
        /// Helper method for SetCell contents
        /// </summary>
        /// <param name="name"></param>
        /// <param name="number"></param>
        private void SetContentsToFormula(string name, Formula formula)
        {
            Cell cell = new Cell(formula);
            NonemptyCells[name] = cell;
        }

        /// <summary>
        /// Helper method for validating names
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool IsValidName(string name)
        {
            String varPattern = @"^[a-zA-Z_](?:[a-zA-Z_]|\d)*$";
            if (name != null && Regex.IsMatch(name, varPattern))
                return true;
            else
                throw new InvalidNameException();
        }
        private bool IsValidCellVariable (string name)
        {
            String varPattern = @"^[a-zA-Z](?:\d)*$";
            if (Regex.IsMatch(name, varPattern))
                return true;
            else
                return false;
        }

    }
    /// <summary>
    /// A basic version of a cell class, holding either string, number, or formula. 
    /// </summary>
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
