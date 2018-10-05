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

        private delegate bool LookupFunc(string name);
        public override bool Changed 
        {
            get;
            protected set;
        }


        public Spreadsheet() : base(s=> true, s => s, "default")
        {

        }
        public Spreadsheet(Func<string, bool> isValid, Func<string, string> normalize, string version) : base(isValid, normalize, version)
        {
            this.Version = version;
        }
        public Spreadsheet(string path, Func<string, bool> isValid, Func<string, string> normalize, string FileVersion) : base(isValid, normalize, FileVersion)
        {
            bool openfile = true;

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreWhitespace = true;
            try
            {
                using (XmlReader reader = XmlReader.Create(path, settings))
                {
                    while (openfile)
                    {

                        if (reader.Read())
                        {
                            if (reader.IsStartElement())
                            {
                                if (reader.Name == "spreadsheet")
                                {
                                    Version = reader.GetAttribute("version");
                                }
                                else if (reader.Name == "cell")
                                {
                                    string name = "";
                                    string contents = "";
                                    //if we can't read, throw exception
                                    if (!reader.Read())
                                        throw new SpreadsheetReadWriteException("Cell values don't exist");

                                    if (reader.IsStartElement())
                                    {
                                        if (reader.Name == "name")
                                        {
                                            reader.Read();
                                            name = reader.Value;
                                            reader.Read();
                                        }
                                        //if element isn't name, throw exception
                                        else
                                            throw new SpreadsheetReadWriteException("Cell not written correctly");
                                    }
                                    //if element doesn't exist, throw exception
                                    if (!reader.Read())
                                        throw new SpreadsheetReadWriteException("Cell values don't exist");
                                    if (reader.IsStartElement())
                                    {

                                        if (reader.Name == "contents")
                                        {
                                            reader.Read();
                                            contents = reader.Value;
                                            reader.Read();
                                        }
                                        //if element isn't contents, throw exception
                                        else
                                            throw new SpreadsheetReadWriteException("Cell not written in correct order");
                                    }
                                    SetContentsOfCell(name, contents);
                                }
                                else
                                    throw new SpreadsheetReadWriteException("Invalid element property");
                            }
                        }
                        else
                            openfile = false;
                    }
                    if (Version != FileVersion)
                        throw new SpreadsheetReadWriteException("Invalid Version Number");

                }
            }
            catch (Exception)
            {
                throw new SpreadsheetReadWriteException("Error reading or writing spreadsheet");
            }



            XMLSpreadsheet SS = new XMLSpreadsheet(version);
            SS = SS.ReadXML(path);
            Version = SS.version;

            foreach (XMLCell cell in SS.cells)
            {
                SetContentsOfCell(cell.name, cell.contents);
            }

        }

        public override string GetSavedVersion(string filename)
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreWhitespace = true;
            try
            {
                using (XmlReader reader = XmlReader.Create(filename, settings))
                {
                    if (reader.ReadToFollowing("spreadsheet"))
                    {
                        return reader.GetAttribute("version");
                    }
                    else
                        throw new SpreadsheetReadWriteException("XML Versioning not correct");
                }
            }
            catch
            {
                throw new SpreadsheetReadWriteException("Error reading or writing xml file");
            }
        }

        public List<Cell> ReadXML(string filename)
        {
            string version = "";
            List<Cell> cells = new List<XMLCell>();
            bool openfile = true;

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreWhitespace = true;
            try
            {
                using (XmlReader reader = XmlReader.Create(filename, settings))
                {
                    while (openfile)
                    {

                        if (reader.Read())
                        {
                            if (reader.IsStartElement())
                            {
                                if (reader.Name == "spreadsheet")
                                {
                                    version = reader.GetAttribute("version");
                                }
                                else if (reader.Name == "cell")
                                {
                                    string name = "";
                                    string contents = "";
                                    //if we can't read, throw exception
                                    if (!reader.Read())
                                        throw new SpreadsheetReadWriteException("Cell values don't exist");

                                    if (reader.IsStartElement())
                                    {
                                        if (reader.Name == "name")
                                        {
                                            reader.Read();
                                            name = reader.Value;
                                            reader.Read();
                                        }
                                        //if element isn't name, throw exception
                                        else
                                            throw new SpreadsheetReadWriteException("Cell not written correctly");
                                    }
                                    //if element doesn't exist, throw exception
                                    if (!reader.Read())
                                        throw new SpreadsheetReadWriteException("Cell values don't exist");
                                    if (reader.IsStartElement())
                                    {

                                        if (reader.Name == "contents")
                                        {
                                            reader.Read();
                                            contents = reader.Value;
                                            reader.Read();
                                        }
                                        //if element isn't contents, throw exception
                                        else
                                            throw new SpreadsheetReadWriteException("Cell not written in correct order");
                                    }
                                    XMLCell cell = new XMLCell(name, contents);
                                    cells.Add(cell);

                                }
                                else
                                    throw new SpreadsheetReadWriteException("Invalid element property");
                            }
                        }
                        else
                            openfile = false;
                    }
                    if (version != this.version)
                        throw new SpreadsheetReadWriteException("Invalid Version Number");
                    XMLSpreadsheet ReadSS = new XMLSpreadsheet(version, cells);
                    return ReadSS;

                }
            }
            catch (Exception)
            {
                throw new SpreadsheetReadWriteException("Error reading or writing spreadsheet");
            }
        }

        public override void Save(string filename)
        {

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = ("  ");
            IEnumerable<string> CellNames = GetNamesOfAllNonemptyCells();

            try
            {
                using (XmlWriter writer = XmlWriter.Create(filename, settings))
                {
                    // set <spreadsheet version ="version">
                    writer.WriteStartDocument();
                    writer.WriteStartElement("spreadsheet");
                    writer.WriteAttributeString("version", Version);

                    foreach (string name in CellNames)
                    {
                        object contents = GetCellContents(name);

                        if (contents.GetType() == typeof(string))
                        {
                            string StringContents = (string)contents;
                            WriteXML(writer, name, StringContents);
                        }
                        else if (contents.GetType() == typeof(double))
                        {
                            string DoubleContents = contents.ToString();
                            WriteXML(writer, name, DoubleContents);

                        }
                        else if (contents.GetType() == typeof(Formula))
                        {
                            string FormulaContents = "=" + contents.ToString();
                            WriteXML(writer, name, FormulaContents);

                        }
                    }
                    //ending spreadsheet element 
                    writer.WriteEndElement();
                }
            }
            catch (Exception)
            {
                throw new SpreadsheetReadWriteException("Error reading or writing xml file");
            }
            Changed = false;
        }

        private void WriteXML(XmlWriter writer, string name, string contents)
        {
                writer.WriteStartElement("cell");
                writer.WriteElementString("name", name);
                writer.WriteElementString("contents", contents);
                writer.WriteEndElement();
        }

        public override object GetCellValue(string name)
        {
            //check if valid name
            if (IsValidName(name))
            {
                //If the name is an exsiting cell
                HashSet<string> NamesOfCells = new HashSet<string>(GetNamesOfAllNonemptyCells());
                string NormalizedName = Normalize(name);
                if (NamesOfCells.Contains(NormalizedName))
                {
                    //return it's value
                    Cell cell = NonemptyCells[NormalizedName];
                    object value = cell.Value;
                    return value;
                }
                else
                {
                    return "";
                }
            }
            else
                throw new InvalidNameException();

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
                string NormalizedName = Normalize(name);
                if (NamesOfCells.Contains(name))
                {
                    //return it's contents
                    Cell cell = NonemptyCells[NormalizedName];
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

            double NumberValue = 0;

            string NormalizedName = Normalize(name);
            //If content is null, throw argument null exception
            if (content == null)
            {
                throw new ArgumentNullException();
            }

            //if name is null or invalid, throw invalid name exception
            else if (!IsValidName(name))
            {
                throw new InvalidNameException();
            }

            //if content parses as double, contents of cell becomes double
            else if(double.TryParse(content, out NumberValue ))
            {
                return SetCellContents(NormalizedName, NumberValue);
            }

            //if contents begins with =, try to make it a formula
            else if (content[0] == '=')
            {
                //if formula string cannot be parsed to formula, it will throw
                string FormulaString = content.Remove(0, 1);
                Formula formula = new Formula(FormulaString, Normalize, IsValid);

                //Function will throw if there is a circular dependency, otherwise it will set contents of cell 
                return SetCellContents(NormalizedName, formula);
            }

            //otherwise, contents of cell becomes content. if exception is not thrown, returns all the dependents of name
            return SetCellContents(NormalizedName, content);

        }

        protected override ISet<String> SetCellContents(String name, double number)
        {
            string type = "double";
            return SetCellContentsActual(name, number, type);
        }
        

        protected override ISet<String> SetCellContents(String name, String text)
        {
            string type = "string";
            return SetCellContentsActual(name, text, type);
        }


        protected override ISet<String> SetCellContents(String name, Formula formula)
        {
            string type = "formula";
            return SetCellContentsActual(name, formula, type);
        }


        protected override IEnumerable<String> GetDirectDependents(String name)
        {
            if (name.Equals(null))
                throw new ArgumentNullException();
            else if (!IsValidName(name))
                throw new InvalidNameException();   
            else
                return graph.GetDependents(Normalize(name));

        }
        private ISet<string> SetCellContentsActual(String name, object ObjectContents, string type)
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
            Changed = true;
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
            try
            {
                object FormulaValue = formula.Evaluate(s => CheckFormulaVariable(s));
                Cell cell = new Cell(formula, FormulaValue);
                NonemptyCells[name] = cell;

            }
            catch (ArgumentException)
            {
                Cell cell = new Cell(formula, new FormulaError());
                NonemptyCells[name] = cell;
            }

        }

        private double CheckFormulaVariable(string name)
        {
            object Value = GetCellValue(name);
            if (Value.GetType() == typeof(string))
                throw new ArgumentException();
            else if (Value.GetType() == typeof(FormulaError))
                throw new ArgumentException();
            else
                return (double)Value;
        }

        

        /// <summary>
        /// Helper method for validating names
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool IsValidName (string name)
        {
            String varPattern = @"^[a-zA-Z]+[0-9]+$";
            //if name is not null, is a valid variable according to spreadsheet, and passes the isvalid delegate function
            if (name != null && Regex.IsMatch(name, varPattern) && IsValid(name))
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
        private object CellValue;
        public object Contents
        {
            get
            {
                return CellContents;
            }
        }
        public object Value
        {
            get
            {
                return CellValue;
            }
        }
        public Cell(string contents)
        {
            CellContents = contents;
            CellValue = contents;
        }
        public Cell(double contents)
        {
            CellContents = contents;
            CellValue = contents;
        }
        public Cell(Formula contents, object value)
        {
            CellContents = contents;
            CellValue = value;
        }
        public void WriteXML(XmlWriter writer, string name)
        {
            writer.WriteStartElement("cell");
            writer.WriteElementString("name", name);
            writer.WriteElementString("contents", (string)CellContents);
            writer.WriteEndElement();

        }

    }

    public class XMLSpreadsheet
    {

        public string version{ get; private set; }

        public List<XMLCell> cells { get; private set; }

        public XMLSpreadsheet(string _version)
        {
            if (_version == null)
                throw new SpreadsheetReadWriteException("Setting null version");
            else 
                version = _version;

            cells = new List<XMLCell>();
        }
        public XMLSpreadsheet( string _version, List<XMLCell> _cells)
        {
            version = _version;
            cells = _cells;
        }

        public void AddCell(XMLCell cell)
        {
            cells.Add(cell);
        }

        public void WriteXML(string filename)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = ("  ");

            using (XmlWriter writer = XmlWriter.Create(filename, settings))
            {
                // set <spreadsheet version ="version">
                writer.WriteStartDocument();
                writer.WriteStartElement("spreadsheet");
                writer.WriteAttributeString("version", version);

                //writing all of the cells 
                foreach (XMLCell cell in cells)
                {
                    cell.WriteXML(writer);
                }

                //ending spreadsheet element 
                writer.WriteEndElement();
            }
        }
        public  XMLSpreadsheet ReadXML(string filename)
        {
            string version = "";
            List<XMLCell> cells = new List<XMLCell>();
            bool openfile = true;

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreWhitespace = true;
            try
            {
                using (XmlReader reader = XmlReader.Create(filename, settings))
                {
                    while (openfile)
                    {

                        if (reader.Read())
                        {
                            if (reader.IsStartElement())
                            {
                                if (reader.Name == "spreadsheet")
                                {
                                    version = reader.GetAttribute("version");
                                }
                                else if (reader.Name == "cell")
                                {
                                    string name = "";
                                    string contents = "";
                                    //if we can't read, throw exception
                                    if (!reader.Read())
                                        throw new SpreadsheetReadWriteException("Cell values don't exist");

                                    if (reader.IsStartElement())
                                    {
                                        if (reader.Name == "name")
                                        {
                                            reader.Read();
                                            name = reader.Value;
                                            reader.Read();
                                        }
                                        //if element isn't name, throw exception
                                        else
                                            throw new SpreadsheetReadWriteException("Cell not written correctly");
                                    }
                                    //if element doesn't exist, throw exception
                                    if (!reader.Read())
                                        throw new SpreadsheetReadWriteException("Cell values don't exist");
                                    if (reader.IsStartElement())
                                    {

                                        if (reader.Name == "contents")
                                        {
                                            reader.Read();
                                            contents = reader.Value;
                                            reader.Read();
                                        }
                                        //if element isn't contents, throw exception
                                        else
                                            throw new SpreadsheetReadWriteException("Cell not written in correct order");
                                    }
                                    XMLCell cell = new XMLCell(name, contents);
                                    cells.Add(cell);

                                }
                                else
                                    throw new SpreadsheetReadWriteException("Invalid element property");
                            }
                        }
                        else
                            openfile = false;
                    }
                    if (version != this.version)
                        throw new SpreadsheetReadWriteException("Invalid Version Number");
                    XMLSpreadsheet ReadSS = new XMLSpreadsheet(version, cells);
                    return ReadSS;

                }
            }
            catch (Exception)
            {
                throw new SpreadsheetReadWriteException("Error reading or writing spreadsheet");
            }
        }
    }
    public class XMLCell
    {
        public string name { get; private set; }
        public string contents { get; private set; }
        public XMLCell(string _name, string _contents)
        {
            name = _name;
            contents = _contents;
        }
        public void WriteXML(XmlWriter writer)
        {
            writer.WriteStartElement("cell");
            writer.WriteElementString("name", name);
            writer.WriteElementString("contents", contents);
            writer.WriteEndElement();

        }

    }


}
