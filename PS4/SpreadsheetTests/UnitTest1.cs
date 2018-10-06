﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Collections;
using SS;
using SpreadsheetUtilities;
namespace SpreadsheetTests
{
    [TestClass]
    public class ValidTests
    {
        [TestMethod]
        public void SingleSetandGetForContents()
        {
            //Setting number
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "5");
            double number = (double)(sheet.GetCellContents("a1"));

            double ValidNumber = 5;
            Assert.AreEqual(number, ValidNumber);

            //Setting string
            sheet.SetContentsOfCell("b1", "hello");
            string text = (string)sheet.GetCellContents("b1");

            Assert.AreEqual(text, "hello");

            //Setting Formula

            sheet.SetContentsOfCell("c1", "=a1 +5");

            Formula TestFormula = new Formula("a1 +5");


            Assert.AreEqual(sheet.GetCellContents("c1"), TestFormula);
        }

        [TestMethod()]
        public void SingleSetAndGetForValue()
        {
            //Setting number
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "5");
            double NumberContents = (double)(sheet.GetCellContents("a1"));

            double NumberValue = (double)(sheet.GetCellValue("a1"));
            double ValidNumber = 5;
            Assert.AreEqual(NumberContents, ValidNumber);
            Assert.AreEqual(NumberValue, ValidNumber);

            //Setting string
            sheet.SetContentsOfCell("b1", "hello");
            string StringContents = (string)sheet.GetCellContents("b1");
            string StringValue = (string)sheet.GetCellValue("b1");

            Assert.AreEqual(StringContents, "hello");
            Assert.AreEqual(StringValue, "hello");


            //Setting Formula

            sheet.SetContentsOfCell("c1", "=a1 +5");

            Formula FormulaContents = (Formula)sheet.GetCellContents("c1");

            double FormulaValue = (double)sheet.GetCellValue("c1");

            Assert.AreEqual(sheet.GetCellContents("c1"), FormulaContents);
            Assert.AreEqual(FormulaValue, 10);
        }

        [TestMethod()]
        public void SetReturnsWithSingleCell()
        {
            Spreadsheet sheet = new Spreadsheet();

            HashSet<string> Dependents = (HashSet<string>)sheet.SetContentsOfCell("a1", "5");
            Assert.IsTrue(Dependents.Count == 1);
        }

        [TestMethod()]
        public void SetReturnsWithMultipleCells()
        {
            Spreadsheet sheet = new Spreadsheet();


            sheet.SetContentsOfCell("a1", "5");
            sheet.SetContentsOfCell("a2", "=a1+2");
            sheet.SetContentsOfCell("a3", "=a2+2");
            sheet.SetContentsOfCell("a4", "=a3+2");
            sheet.SetContentsOfCell("a5", "=a4+2");

            HashSet<string> Dependents = (HashSet<string>)sheet.SetContentsOfCell("a1", "5");
            int count = 5;
            Assert.AreEqual(count, Dependents.Count);
            bool CorrectVariable = false;
            foreach (string s in Dependents)
            {
                if (s == "a3")
                    CorrectVariable = true;
            }
            Assert.IsTrue(CorrectVariable);
        }


        [TestMethod()]
        public void SetForExistingCell()
        {
            Spreadsheet sheet = new Spreadsheet();


            sheet.SetContentsOfCell("a1", "5");
            sheet.SetContentsOfCell("a2", "=a1+5");
            sheet.SetContentsOfCell("a3", "5");
            HashSet<string> Dependents = new HashSet<string>(sheet.SetContentsOfCell("a2", "=a3+5"));

            Assert.IsTrue(Dependents.Count == 1);
            Assert.IsTrue(Dependents.Contains("a2"));
        }

        [TestMethod()]
        public void SetForExistingCell1()
        {
            Spreadsheet sheet = new Spreadsheet();



            sheet.SetContentsOfCell("a1", "5");
            sheet.SetContentsOfCell("a2", "=a1+5");
            sheet.SetContentsOfCell("a3", "=a2+5");
            sheet.SetContentsOfCell("a4", "=a3+5");

            HashSet<string> Dependents = new HashSet<string>(sheet.SetContentsOfCell("a1", "5"));

            HashSet<string> CorrectDependents = new HashSet<string>();
            CorrectDependents.Add("a1");
            CorrectDependents.Add("a2");
            CorrectDependents.Add("a3");
            CorrectDependents.Add("a4");

            Assert.IsTrue(Dependents.SetEquals(CorrectDependents));

        }

        [TestMethod()]
        public void SetandGetValidNamesForAllTypes()
        {
            Spreadsheet sheet = new Spreadsheet();

            Formula formula = new Formula("a1+5", s => s, s => true);

            sheet.SetContentsOfCell("a1", "5");
            sheet.SetContentsOfCell("a1111111", "=a1+5");
            sheet.SetContentsOfCell("abcdefghijklmnopqrstuvwzyz0123456789", "hello");
            sheet.SetContentsOfCell("aaaaaaaaaa1", "=a1+5");
     


            double d1 = (double)sheet.GetCellContents("a1");
            Formula f1 = (Formula)sheet.GetCellContents("a1111111");
            string s2 = (string)sheet.GetCellContents("abcdefghijklmnopqrstuvwzyz0123456789");
            Formula f2 = (Formula)sheet.GetCellContents("aaaaaaaaaa1");
            

            Assert.AreEqual(d1, 5);

            Assert.AreEqual(s2, "hello");

            Assert.AreEqual(f1, new Formula("a1+5"));
            Assert.AreEqual(f2, new Formula("a1+5"));
        }

        [TestMethod()]
        public void GetContentsForEmptyCell()
        {
            Spreadsheet sheet = new Spreadsheet();

            string EmptyString = (string)sheet.GetCellContents("a1");

            Assert.AreEqual("", EmptyString);
        }

        [TestMethod()]
        public void GetNamesForOneCell ()
        {
            Spreadsheet sheet = new Spreadsheet();

            sheet.SetContentsOfCell("a1", "5");

            IEnumerator<string> names = sheet.GetNamesOfAllNonemptyCells().GetEnumerator();

            Assert.IsTrue(names.MoveNext());

            Assert.AreEqual("a1", names.Current);

            Assert.IsFalse(names.MoveNext());
        }


        [TestMethod()]
        public void GetNamesForMultipleCells()
        {
            Spreadsheet sheet = new Spreadsheet();


            sheet.SetContentsOfCell("a1", "5");
            sheet.SetContentsOfCell("a2", "5");
            sheet.SetContentsOfCell("a3", "hello");
            sheet.SetContentsOfCell("a4", "=a1+5");
            sheet.SetContentsOfCell("a5", "5");
            sheet.SetContentsOfCell("a6", "hello");


            IEnumerator<string> names = sheet.GetNamesOfAllNonemptyCells().GetEnumerator();

            Assert.IsTrue(names.MoveNext());
            Assert.AreEqual("a1", names.Current);

            Assert.IsTrue(names.MoveNext());
            Assert.AreEqual("a2", names.Current);

            Assert.IsTrue(names.MoveNext());
            Assert.AreEqual("a3", names.Current);


            //hello
            Assert.IsTrue(names.MoveNext());
            Assert.AreEqual("a4", names.Current);

            Assert.IsTrue(names.MoveNext());
            Assert.AreEqual("a5", names.Current);

            Assert.IsTrue(names.MoveNext());
            Assert.AreEqual("a6", names.Current);

            Assert.IsFalse(names.MoveNext());

        }

        /*******************
         * Constructor Tests
         *******************/



        [TestMethod()]
        public void SimpleNormalize()
        {
            Spreadsheet sheet = new Spreadsheet(s => true, s => s.ToUpper(), "Version 1");
            sheet.SetContentsOfCell("a1", "5");
            IEnumerator<string> CellNames = sheet.GetNamesOfAllNonemptyCells().GetEnumerator();

            Assert.IsTrue(CellNames.MoveNext());
            Assert.AreEqual("A1", CellNames.Current);

        }

        [TestMethod()]
        public void SimpleIsValid()
        {
            Spreadsheet sheet = new Spreadsheet(s => true, s => s, "Version 1");
            sheet.SetContentsOfCell("a1", "5");
            sheet.SetContentsOfCell("a2", "=a1+5");

            double Value = (double)sheet.GetCellValue("a2");
            double CorrectValue = 10;

            Assert.AreEqual(CorrectValue, Value);
        }

        [TestMethod()]
        public void SimpleVersion()
        {
            Spreadsheet sheet = new Spreadsheet(s => true, s => s, "1.0");
            Assert.AreEqual("1.0", sheet.Version);
        }

        [TestMethod()]
        public void SimpleSave()
        {
            Spreadsheet sheet = new Spreadsheet(s => true, s => s, "1.0");

            sheet.SetContentsOfCell("a1", "5");
            sheet.SetContentsOfCell("a2", "=a1 +5");
            sheet.SetContentsOfCell("a3", "=a2 +5");

            sheet.Save("test.xml");
        }
        [TestMethod()]
        public void SimpleSaveAndRead()
        {
            Spreadsheet sheet = new Spreadsheet(s => true, s => s, "1.0");

            sheet.SetContentsOfCell("a1", "5");
            sheet.SetContentsOfCell("a2", "=a1 +5");
            sheet.SetContentsOfCell("a3", "=a2 +5");

            sheet.Save("test.xml");

            Spreadsheet NewSheet = new Spreadsheet("test.xml", s => true, s => s, "1.0");

            Assert.AreEqual((double)5, NewSheet.GetCellContents("a1"));
            Assert.AreEqual(new Formula("a1+5"), NewSheet.GetCellContents("a2"));
            Assert.AreEqual(new Formula("a2 +5"), NewSheet.GetCellContents("a3"));

        }

        [TestMethod()]
        public void SimpleRead()
        {
            Spreadsheet sheet = new Spreadsheet("../../XMLSimple.xml", s => true, s => s, "1.0");
            Assert.AreEqual((double)5, sheet.GetCellContents("a1"));
            Assert.AreEqual(new Formula("a1+5"), sheet.GetCellContents("a2"));
            Assert.AreEqual(new Formula("a2 +5"), sheet.GetCellContents("a3"));
        }

        [TestMethod()]
        public void SimpleChangedTrue()
        {
            Spreadsheet sheet = new Spreadsheet();

            sheet.SetContentsOfCell("a1", "5");

            Assert.IsTrue(sheet.Changed);
        }

        [TestMethod()]
        public void SimpleChangedFalse()
        {
            Spreadsheet sheet = new Spreadsheet();

            Assert.IsFalse(sheet.Changed);
        }

        [TestMethod()]
        public void ChangedTrueOnSave()
        {
            Spreadsheet sheet = new Spreadsheet(s => true, s => s, "1.0");

            sheet.SetContentsOfCell("a1", "5");
            sheet.SetContentsOfCell("a2", "=a1 +5");
            sheet.SetContentsOfCell("a3", "=a2 +5");

            sheet.Save("test.xml");

            Assert.IsFalse(sheet.Changed);

            sheet.SetContentsOfCell("a1", "5");

            Assert.IsTrue(sheet.Changed);
            
        }

        [TestMethod()]
        public void GetSavedVersion()
        {
            Spreadsheet sheet = new Spreadsheet();
            string version = sheet.GetSavedVersion("../../XMLSimple.xml");

        }
    }
    [TestClass]
    public class InvalidTests
    {

        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetWithSingleNumber()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("1", "5");
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetWithSingleLetter()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a", "5");
        }



        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetWithNumberAtFirst()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("1a", "5");
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetWithRandomSymbol()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("%456", "5");
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetWithRandom()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a123%67", "5");
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetNullName()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell(null, "5");
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetNullContents()
        {
            Spreadsheet sheet = new Spreadsheet();
            string test = null;
            sheet.SetContentsOfCell("a1", test);
        }

        [TestMethod()]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void SetInvalidFormula()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "=((a1+4)");
        }

        [TestMethod()]
        [ExpectedException(typeof(SpreadsheetUtilities.FormulaFormatException))]
        public void SetInvalidFormula1()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "=");
        }

        [TestMethod()]
        public void SetFormulaErrorEmptyCell()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "=a2 +5");
            object Error = sheet.GetCellValue("a1");

            Assert.IsInstanceOfType(Error, typeof(FormulaError));
        }
        [TestMethod()]
        public void SetFormulaErrorDivideBy()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a2", "5");
            sheet.SetContentsOfCell("a1", "=(a2/(5-5))");

            object Error = sheet.GetCellValue("a1");

            Assert.IsInstanceOfType(Error, typeof(FormulaError));
        }

        [TestMethod()]
        public void SetFormulaErrorString()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "hello");
            sheet.SetContentsOfCell("a2", "=a1 +5");

            object Error = sheet.GetCellValue("a2");

            Assert.IsInstanceOfType(Error, typeof(FormulaError));
        }
        [TestMethod()]
        public void SetReturnsWithMultipleWrong()
        {
            Spreadsheet sheet = new Spreadsheet();


            sheet.SetContentsOfCell("a1", "5");
            sheet.SetContentsOfCell("a2", "=a1+2");
            sheet.SetContentsOfCell("a3", "=a2+2");
            sheet.SetContentsOfCell("a4", "=a3+2");
            sheet.SetContentsOfCell("a5", "=a6+2");

            HashSet<string> Dependents = (HashSet<string>)sheet.SetContentsOfCell("a1", "5");
            int count = 4;
            Assert.AreEqual(count, Dependents.Count);
            bool CorrectVariable = true;
            foreach (string s in Dependents)
            {
                if (s == "a5")
                    CorrectVariable = false;
            }
            Assert.IsTrue(CorrectVariable);
        }

        [TestMethod()]
        [ExpectedException(typeof(CircularException))]
        public void SetCircularDependency()
        {
            Spreadsheet sheet = new Spreadsheet();

            sheet.SetContentsOfCell("a1", "5");

            sheet.SetContentsOfCell("a2", "=a3 +2");
            sheet.SetContentsOfCell("a3", "=a2 + 2");
        }

        [TestMethod()]
        public void SetCircularDependecyDoesntChangeOnNewCell()
        {
            Spreadsheet sheet = new Spreadsheet();

            sheet.SetContentsOfCell("a1", "5");

            sheet.SetContentsOfCell("a2", "=a3 +2");
            try
            {
                sheet.SetContentsOfCell("a3", "=a2 + 2");
            }
            catch (CircularException e)
            {
                HashSet<string> NamesOfCells = new HashSet<string>(sheet.GetNamesOfAllNonemptyCells());
                int count = NamesOfCells.Count;
                Assert.AreEqual(count, 2);
            }
        }

        [TestMethod()]
        public void SetCircularDependecyDoesntChangeOnExistingCell()
        {
            Spreadsheet sheet = new Spreadsheet();

            sheet.SetContentsOfCell("a1", "5");

            sheet.SetContentsOfCell("a2", "=a1 +5");

            sheet.SetContentsOfCell("a3", "=a2 +5");

            sheet.SetContentsOfCell("a4", "=a3 +5");

            sheet.SetContentsOfCell("a5", "=a4 +5");

            HashSet<string> DentsBeforeCircExcep = new HashSet<string>(sheet.SetContentsOfCell("a1", "5"));

            try
            {
                sheet.SetContentsOfCell("a3", "=a1");
            }
            catch (CircularException E)
            {
                HashSet<string> DentsAfterCircExcep = new HashSet<string>(sheet.SetContentsOfCell("a1", "5"));
                Assert.IsTrue(DentsBeforeCircExcep.SetEquals(DentsAfterCircExcep));
            }
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetContentsWithSingleNumber()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.GetCellContents("1");
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetValueWithSingleNumber()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.GetCellValue("1");
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetContentsWithNumberAtFirst()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.GetCellContents("1a");
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetValueWithNumberAtFirst()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.GetCellValue("1a");
        }




        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetContentsWithRandomSymbol()
        
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.GetCellContents("%456");
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetValueWithRandomSymbol()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.GetCellValue("%456");
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetContentsWithRandom()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.GetCellContents("a123%67");
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetValueWithRandom()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.GetCellValue("a123%67");
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetContentsNullName()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.GetCellContents(null);
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetValueNullName()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.GetCellValue(null);
        }


        /*****************
         * Constructor
         * ***********/

        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void IsValidFalse()
        {
            Spreadsheet sheet = new Spreadsheet(s => false, s => s, "1.0");
            sheet.SetContentsOfCell("a1", "5");
            sheet.SetContentsOfCell("a2", "=a1 + 5");
        }
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void IsValidFalseForFomula()
        {
            Spreadsheet sheet = new Spreadsheet(s =>
            {
                if (s == "a1" || s =="a2")
                    return true;
                else
                    return false;
            },
            s => s, 
            "1.0"
            );

            sheet.SetContentsOfCell("a1", "5");
            sheet.SetContentsOfCell("a2", "=a3 +5");
        }

        [TestMethod()]
        [ExpectedException(typeof(CircularException))]
        public void XmlCircularDependency()
        {
            Spreadsheet sheet = new Spreadsheet("../../XMLCircularDependency.xml", s=>true, s=> s, "1.0");

        }

        [TestMethod()]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void XmlInvalidVersion()
        {
            Spreadsheet sheet = new Spreadsheet("../../XMLInvalidVersion.xml", s => true, s => s, "1.1");

        }

        [TestMethod()]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void XmlInvalidFilePath()
        {
            Spreadsheet sheet = new Spreadsheet("../../XMLInvalVersion.xml", s => true, s => s, "1.0");

        }

        [TestMethod()]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void XmlNullFilePath()
        {
            Spreadsheet sheet = new Spreadsheet(null, s => true, s => s, "1.1");

        }

        [TestMethod()]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void XmlNullVersion()
        {
            Spreadsheet sheet = new Spreadsheet("../../XMLInvalVersion.xml", s => true, s => s, null);

        }

        [TestMethod()]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void XmlInvalidElement()
        {
            Spreadsheet sheet = new Spreadsheet("../../XMLInvalidElement.xml", s => true, s => s, "1.0");

        }

        [TestMethod()]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void XmlInvalidElementOrder()
        {
            Spreadsheet sheet = new Spreadsheet("../../XMLInvalidElementOrder.xml", s => true, s => s, "1.0");
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void XmlInvalidFormula()
        {
            Spreadsheet sheet = new Spreadsheet("../../XMLInvalidFormula.xml", s => true, s => s, "1.0");
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void XmlInvalidVariable()
        {
            Spreadsheet sheet = new Spreadsheet("../../XMLInvalidVariable.xml", s => true, s => s, "1.0");
        }

        [TestMethod()]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void XmlNoClosingElementTag()
        {
            Spreadsheet sheet = new Spreadsheet("../../XMLNoClosingElementTag.xml", s => true, s => s, "1.0");
        }

        [TestMethod()]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void XmlSetNullVersion()
        {
            Spreadsheet sheet = new Spreadsheet(s => true, s => s, null);

            sheet.SetContentsOfCell("a1", "5");
            sheet.SetContentsOfCell("a2", "=a1 +5");
            sheet.SetContentsOfCell("a3", "=a2 +5");

            sheet.Save("test.xml");

        }


        [TestMethod()]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void XmlSetNullFilePath()
        {
            Spreadsheet sheet = new Spreadsheet(s => true, s => s, null);

            sheet.SetContentsOfCell("a1", "5");
            sheet.SetContentsOfCell("a2", "=a1 +5");
            sheet.SetContentsOfCell("a3", "=a2 +5");

            sheet.Save(null);

        }

    }
}
