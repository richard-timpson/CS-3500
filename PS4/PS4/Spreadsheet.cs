using System;
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
                        //read the next element
                        if (reader.Read())
                        {
                            if (reader.IsStartElement())
                            {
                                //if spreadsheet get version
                                if (reader.Name == "spreadsheet")
                                {
                                    Version = reader.GetAttribute("version");
                                }
                                //if cell get name and contents
                                else if (reader.Name == "cell")
                                {
                                    string name = "";
                                    string contents = "";
                                    reader.Read();

                                    if (reader.IsStartElement())
                                    {
                                        if (reader.Name == "name")
                                        {
                                            reader.Read();
                                            name = reader.Value;
                                            reader.Read();
                                            reader.ReadEndElement();
                                        }
                                        //if element isn't name, throw exception
                                        else
                                            throw new SpreadsheetReadWriteException("Cell not written correctly");
                                    }
                                    //if element doesn't exist, throw exception
                                    
                                    if (reader.IsStartElement())
                                    {

                                        if (reader.Name == "contents")
                                        {
                                            reader.Read();
                                            contents = reader.Value;
                                            reader.Read();
                                            reader.ReadEndElement();
                                        }
                                        //if element isn't contents, throw exception
                                        else
                                            throw new SpreadsheetReadWriteException("Cell not written in correct order");
                                    }
                                    //once we have the name and contents, set the contents of the cell
                                    SetContentsOfCell(name, contents);
                                }
                                else
                                    throw new SpreadsheetReadWriteException("Invalid element property");
                            }
                        }
                        else
                            openfile = false;
                    }
                    //if it's not a valid version number, throw exception
                    if (Version != FileVersion)
                        throw new SpreadsheetReadWriteException("Invalid Version Number");

                }
            }
            //catching the different SS exceptions and throwing them. otherwise, if it is a read write exception, throw that. 
            catch (CircularException E)
            {
                throw E;
            }
            catch(InvalidNameException E)
            {
                throw E;
            }
            catch(SpreadsheetReadWriteException E)
            {
                throw E;
            }
            catch (Exception)
            {
                throw new SpreadsheetReadWriteException("Error reading or writing spreadsheet");
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
                    //go to <spreadsheet>
                    if (reader.ReadToFollowing("spreadsheet"))
                    {
                        //return version
                        return reader.GetAttribute("version");
                    }
                    //if no <spreadsheet> throw exception
                    else
                        throw new SpreadsheetReadWriteException("XML Versioning not correct");
                }
            }
            catch
            {
                throw new SpreadsheetReadWriteException("Error reading or writing xml file");
            }
        }

        public override void Save(string filename)
        {

            if (Version == null)
                throw new SpreadsheetReadWriteException("Trying to save null version");
            //setting up writer
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = ("  ");
            //get the non empty cells 
            IEnumerable<string> CellNames = GetNamesOfAllNonemptyCells();

            try
            {
                using (XmlWriter writer = XmlWriter.Create(filename, settings))
                {
                    // set <spreadsheet version ="version">
                    writer.WriteStartDocument();
                    writer.WriteStartElement("spreadsheet");
                    writer.WriteAttributeString("version", Version);

                    //looping through all of the non empty cells
                    foreach (string name in CellNames)
                    {
                        //getting contents
                        object contents = GetCellContents(name);

                        //setting contents depending on the type
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
                    writer.WriteEndDocument();
                }
            }
            catch (Exception)
            {
                throw new SpreadsheetReadWriteException("Error reading or writing xml file");
            }
            Changed = false;
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
                //if cell is empty, return empty string
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
            HashSet<string> AllDependents;
            //if content is null, throw argument null exceptoin
            if (content == null)
                throw new ArgumentNullException();

            //if name is null or invalid,throw invalid name exception
            else if (!IsValidName(NormalizedName))
                throw new InvalidNameException();

            //if content parses as double, contents of cell becomes double, and cells recalculate
            else if (double.TryParse(content, out NumberValue))
            {
                //set content to double
                //return recalculate cells. 
                return SetCellContents(NormalizedName, NumberValue);
            }

            //if content starts with =, check for forumla
            else if (content[0] == '=')
            {
                //if content doesn't parse as formula, with throw, otherwise it will stay a formula
                string FormulaString = content.Remove(0, 1);
                Formula formula = new Formula(FormulaString, Normalize, IsValid);
                
                //getting the current contents of the cell
                object CurrentCellContents = GetCellContents(NormalizedName);

                // a list to keep track of the current dependees
                List<string> CurrentDependees = new List<string>();

                //if the cell already exsits as a formula
                if(CurrentCellContents.GetType() == typeof(Formula))
                {
                    //get the formula object
                    Formula CurrentcellFormula = (Formula)CurrentCellContents;
                    //set the current dependees as the dependees of the formula
                    CurrentDependees = new List<string>(CurrentcellFormula.GetVariables());
                }
                //a list for the replacement dependees of the new formula
                List<string> ReplaceDependees = new List<string>(formula.GetVariables());

                //replace the dependees with the new formula
                graph.ReplaceDependees(NormalizedName, ReplaceDependees);

                try
                {
                    //if circular exception is thrown, will catch
                    //otherwise will return recalculate values and return dependents
                    return SetCellContents(NormalizedName, formula);
                }
                //If exception is caught
                catch (CircularException E)
                {
                    //replace dependees with old variables
                    graph.ReplaceDependees(name, CurrentDependees);

                    //throw the exception again
                    throw new CircularException();
                }

            }
            //if it's not a formula, set the contents of the cell to the string, and relcalculate the cells
            return SetCellContents(NormalizedName, content);
        }
      
        private ISet<string> ReCalculateCells(HashSet<string> AllDependents)
        {
            //looping through all variables to recalculate
            foreach (string RecalcCellName in AllDependents)
            {
                //getting the value of the variables
                Cell RecalcCell = NonemptyCells[RecalcCellName];
                object RecalcObject = RecalcCell.Contents;
                //only change them if they are a formula. If they aren't, their value will already be set
                if (RecalcObject.GetType() == typeof(Formula))
                {
                    SetContentsToFormula(RecalcCellName, (Formula)RecalcObject);
                }

            }
            Changed = true;
            return AllDependents;

        }

        protected override ISet<String> SetCellContents(String name, double number)
        {
            HashSet<string> AllDependents = new HashSet<string>(GetCellsToRecalculate(name));
            SetContentsToDouble(name, number);
            return ReCalculateCells(AllDependents);

        }


        protected override ISet<String> SetCellContents(String name, String text)
        {
            HashSet<string> AllDependents = new HashSet<string>(GetCellsToRecalculate(name));
            SetContentsToString(name, text);
            return ReCalculateCells(AllDependents);
        }


        protected override ISet<String> SetCellContents(String name, Formula formula)
        {
            HashSet<string> AllDependents = new HashSet<string>(GetCellsToRecalculate(name));
            SetContentsToFormula(name, formula);
            return ReCalculateCells(AllDependents);
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
            catch (FormulaFormatException)
            {
                Cell cell = new Cell(formula, new FormulaError());
                NonemptyCells[name] = cell;
            }

        }
        /// <summary>
        /// helper method for the lookup delegate. Checking to see what the value is and only returning if it is a double. 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private double CheckFormulaVariable(string name)
        {
            object Value = GetCellValue(name);
            if (Value.GetType() == typeof(string))
                throw new FormulaFormatException("string in formula");
            else if (Value.GetType() == typeof(FormulaError))
                throw new FormulaFormatException("Formula error in formula");
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
        /// <summary>
        /// helper method for writing xml files. 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="name"></param>
        /// <param name="contents"></param>

        private void WriteXML(XmlWriter writer, string name, string contents)
        {
            writer.WriteStartElement("cell");
            writer.WriteElementString("name", name);
            writer.WriteElementString("contents", contents);
            writer.WriteEndElement();
        }


    }
    /// <summary>
    /// A basic version of a cell class, holding either string, number, or formula. 
    /// It has three different constructors, for the the different cell types. Will handle each type accordingly. 
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




    }

}
