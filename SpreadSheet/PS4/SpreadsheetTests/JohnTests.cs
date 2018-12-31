// These tests are for private use only
// Redistributing this file is strictly against SoC policy.

using SS;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using SpreadsheetUtilities;

namespace GradingTests
{


    /// <summary>
    ///This is a test class for SpreadsheetTest and is intended
    ///to contain all SpreadsheetTest Unit Tests
    ///</summary>
    [TestClass()]
    public class JohnTests
    {

        // EMPTY SPREADSHEETS
        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestEmptyGetNull()
        {
            Spreadsheet s = new Spreadsheet();
            s.GetCellContents(null);
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestEmptyGetContents()
        {
            Spreadsheet s = new Spreadsheet();
            s.GetCellContents("1AA");
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestInvalidVarDouble()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("1", "1.0");
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestInvalidName()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("1A", "1.0");
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestInvalidName2()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A3A", "1.0");
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestInvalidName3()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("AAB33_", "1.0");
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestInvalidName4()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("*AB", "1.0");
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestInvalidName5()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A", "1.0");
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestInvalidName6()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A2", "1.0");
            s.GetCellValue("a");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestInvalidDeleteOfDepend()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "2.0");
            s.SetContentsOfCell("A2", "=A1-1.0");
            s.SetContentsOfCell("A3", "2.0");
            s.SetContentsOfCell("A4", "=A2+A3");
            s.SetContentsOfCell("A5", "=A3+A1");
            Assert.AreEqual(4.0, s.GetCellValue("A5"));
            s.SetContentsOfCell("A5", "=A1");
            Assert.AreEqual(2.0, s.GetCellValue("A5"));
            s.SetContentsOfCell("A3", "5.0");
            s.SetContentsOfCell("A5", "=A3+A1");
            s.SetContentsOfCell("A3", "");

            Assert.IsInstanceOfType(s.GetCellContents("A5"), typeof(FormulaError));

        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestIsValidDelegate()
        {
            Spreadsheet s = new Spreadsheet(str => false, str => str.ToUpper(), "default");
            s.SetContentsOfCell("A", "1.0");
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullContent()
        {
            Spreadsheet s = new Spreadsheet(str => true, str => str.ToUpper(), "default");
            s.SetContentsOfCell("A", null);
        }

        [TestMethod()]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestNullFileName()
        {
            Spreadsheet s = new Spreadsheet(null, str => true, str => str.ToUpper(), "default");
        }

        [TestMethod()]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestFileNotFound()
        {
            Spreadsheet s = new Spreadsheet("test4", str => true, str => str.ToUpper(), "default");
        }

        [TestMethod()]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestSaveNoName()
        {
            Spreadsheet s = new Spreadsheet(str => true, str => str.ToUpper(), "default");
            s.SetContentsOfCell("A2", "1.0");
            s.Save(null);
        }

        [TestMethod()]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestGetSavedVersionNull()
        {
            Spreadsheet s = new Spreadsheet(str => true, str => str.ToUpper(), "default");
            s.SetContentsOfCell("A2", "1.0");
            s.Save("test1");
            s.GetSavedVersion(null);
        }

        [TestMethod()]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestGetSavedVersionIncorrectFileName()
        {
            Spreadsheet s = new Spreadsheet(str => true, str => str.ToUpper(), "default");
            s.SetContentsOfCell("A2", "1.0");
            s.Save("test1");
            s.GetSavedVersion("ImTired");
        }

        [TestMethod()]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestLoadSavedVersionIncorrectFileName()
        {
            Spreadsheet s = new Spreadsheet(str => true, str => str.ToUpper(), "default");
            s.SetContentsOfCell("A2", "1.0");
            s.Save("test1");

            Spreadsheet s2 = new Spreadsheet("invalid", str => true, str => str.ToUpper(), "default");
        }

        [TestMethod()]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestGetSavedVersionIncorrectVersionInput()
        {
            Spreadsheet s = new Spreadsheet(str => true, str => str.ToUpper(), "default");
            s.SetContentsOfCell("A2", "1.0");
            s.Save("test1");
            Spreadsheet s1 = new Spreadsheet("test1", str => true, str => str.ToUpper(), "wrongVersion");
        }

        [TestMethod()]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestGetSavedVersionNotFound()
        {
            Spreadsheet s = new Spreadsheet(str => true, str => str.ToUpper(), null);
            s.SetContentsOfCell("A2", "1.0");
            s.Save("test1");
            s.GetSavedVersion("test1");
        }

        [TestMethod()]
        [ExpectedException(typeof(CircularException))]
        public void TestCircularException()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "1.0");
            s.SetContentsOfCell("A2", "4.0");
            s.SetContentsOfCell("A3", "5.0");
            s.SetContentsOfCell("A4", "=A2+A3");
            s.SetContentsOfCell("A2", "=A4");
        }

        [TestMethod()]
        [ExpectedException(typeof(CircularException))]
        public void TestCircularException2()
        {
            Spreadsheet s = new Spreadsheet(str => true, str => str.ToUpper(), "default");
            s.SetContentsOfCell("a6", "=A6");
        }

        [TestMethod()]
        [ExpectedException(typeof(CircularException))]
        public void TestCircularException3()
        {
            Spreadsheet s = new Spreadsheet(str => true, str => str.ToUpper(), "default");
            s.SetContentsOfCell("A6", "=a6");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestFormulaFormatException()
        {
            Spreadsheet s = new Spreadsheet();
            Formula f = new Formula("A2+A3");
            s.SetContentsOfCell("A1", "2.0");
            s.SetContentsOfCell("A2", "=A1");
            s.SetContentsOfCell("A1", "3.0");
            s.SetContentsOfCell("A1", "5.0");
            s.SetContentsOfCell("A3", "6.0");
            s.SetContentsOfCell("A2", "=A1 + A3");
            s.SetContentsOfCell("A1", "Jim");

            Assert.AreEqual("Jim", s.GetCellValue("A1"));
            Assert.AreEqual("Jim", s.GetCellValue("A2"));
        }

        [TestMethod()]
        public void TestGetEmpty()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            Assert.AreEqual("", s.GetCellContents("A2"));
        }

        [TestMethod()]
        public void TestSetEmptyContents()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A2", "");
            Assert.AreEqual("", s.GetCellContents("A2"));
        }

        [TestMethod()]
        public void TestSetCellContents()
        {
            Spreadsheet s = new Spreadsheet();
            Formula f = new Formula("A2");
            s.SetContentsOfCell("A2", "1.5");
            s.SetContentsOfCell("A3", "Jim");
            s.SetContentsOfCell("A4", "=A2");
            Assert.AreEqual(1.5, s.GetCellContents("A2"));
            Assert.AreEqual("Jim", s.GetCellContents("A3"));
            Assert.AreEqual(f, s.GetCellContents("A4"));
        }

        [TestMethod()]
        public void TestRemoveCellContents()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A2", "1.5");
            s.SetContentsOfCell("A3", "Jim");
            s.SetContentsOfCell("A4", "=A2");
            s.SetContentsOfCell("A3", "");
            s.SetContentsOfCell("A4", "");

            HashSet<string> test = new HashSet<string>();
            test.Add("A2");
            Assert.IsTrue(test.SetEquals(s.GetNamesOfAllNonemptyCells()));
        }

        [TestMethod()]
        public void TestNormalize()
        {
            Spreadsheet s = new Spreadsheet(str => true, str => str.ToUpper(), "default");
            Formula f = new Formula("A2");
            s.SetContentsOfCell("a2", "1.5");
            s.SetContentsOfCell("a3", "Jim");
            s.SetContentsOfCell("a4", "=a2");
            Assert.AreEqual(1.5, s.GetCellContents("A2"));
            Assert.AreEqual("Jim", s.GetCellContents("A3"));
            Assert.AreEqual(f, s.GetCellContents("A4"));
            Assert.AreEqual(1.5, s.GetCellContents("a2"));
        }

        [TestMethod()]
        public void TestSetCellContentsNameConvention()
        {
            Spreadsheet s = new Spreadsheet();
            Formula f = new Formula("AA22");
            s.SetContentsOfCell("AA22", "1.5");
            s.SetContentsOfCell("AB3", "Jim");
            s.SetContentsOfCell("A4", "=AA22");
            Assert.AreEqual(1.5, s.GetCellContents("AA22"));
            Assert.AreEqual("Jim", s.GetCellContents("AB3"));
            Assert.AreEqual(f, s.GetCellContents("A4"));
        }

        [TestMethod()]
        public void TestSetCellContentsGet()
        {
            Spreadsheet s = new Spreadsheet();
            Formula f = new Formula("A2+A3");
            s.SetContentsOfCell("A1", "2.0");
            s.SetContentsOfCell("A2", "=A1-1.0");
            s.SetContentsOfCell("A3", "2.0");
            s.SetContentsOfCell("A4", "=A2+A3");
            s.SetContentsOfCell("A3", "5.0");
            Assert.AreEqual(5.0, s.GetCellContents("A3"));
            Assert.AreEqual(f, s.GetCellContents("A4"));
            Assert.AreEqual(5.0, s.GetCellValue("A3"));
            Assert.AreEqual(6.0, s.GetCellValue("A4"));
        }

        [TestMethod()]
        public void TestSetCellContentReplaceForm()
        {
            Spreadsheet s = new Spreadsheet();
            Formula f = new Formula("A2+A3");
            s.SetContentsOfCell("A1", "2.0");
            s.SetContentsOfCell("A2", "=A1-1.0");
            s.SetContentsOfCell("A3", "2.0");
            s.SetContentsOfCell("A4", "=A2+A3");
            s.SetContentsOfCell("A5", "=A3+A1");
            Assert.AreEqual(4.0, s.GetCellValue("A5"));
            s.SetContentsOfCell("A5", "=A1");
            Assert.AreEqual(2.0, s.GetCellValue("A5"));
            s.SetContentsOfCell("A3", "5.0");
            s.SetContentsOfCell("A5", "=A3+A1");
            Assert.AreEqual(7.0, s.GetCellValue("A5"));
            Assert.AreEqual(5.0, s.GetCellContents("A3"));
            Assert.AreEqual(f, s.GetCellContents("A4"));
            Assert.AreEqual(5.0, s.GetCellValue("A3"));
            Assert.AreEqual(6.0, s.GetCellValue("A4"));
            s.SetContentsOfCell("A5", "");

            HashSet<string> test = new HashSet<string>();
            test.Add("A1");
            test.Add("A2");
            test.Add("A3");
            test.Add("A4");
            Assert.IsTrue(test.SetEquals(s.GetNamesOfAllNonemptyCells()));

        }

        [TestMethod()]
        public void TestGetNamesOfAllNonempty()
        {
            Spreadsheet s = new Spreadsheet();
            Formula f = new Formula("A2+A3");
            s.SetContentsOfCell("A1", "2.0");
            s.SetContentsOfCell("A2", "=A1-1.0");
            s.SetContentsOfCell("A3", "2.0");
            s.SetContentsOfCell("A4", "=A2+A3");
            s.SetContentsOfCell("A3", "5.0");
            Assert.AreEqual(5.0, s.GetCellContents("A3"));
            Assert.AreEqual(f, s.GetCellContents("A4"));
            Assert.AreEqual(5.0, s.GetCellValue("A3"));
            Assert.AreEqual(6.0, s.GetCellValue("A4"));

            s.GetNamesOfAllNonemptyCells();
        }

        [TestMethod()]
        public void TestSetCellContentsGetComplex()
        {
            Spreadsheet s = new Spreadsheet();
            Formula f = new Formula("A2+A3");
            s.SetContentsOfCell("A1", "2.0");
            s.SetContentsOfCell("A2", "=A1-1.0");
            s.SetContentsOfCell("A3", "2.0");
            s.SetContentsOfCell("A4", "=A2+A3");
            s.SetContentsOfCell("A3", "5.0");
            s.SetContentsOfCell("B1", "7.5");
            s.SetContentsOfCell("B2", "=A4+B1");

            Assert.AreEqual(5.0, s.GetCellContents("A3"));
            Assert.AreEqual(f, s.GetCellContents("A4"));
            Assert.AreEqual(5.0, s.GetCellValue("A3"));
            Assert.AreEqual(6.0, s.GetCellValue("A4"));
            Assert.AreEqual(13.5, s.GetCellValue("B2"));

            s.SetContentsOfCell("A4", "=A2");

            Assert.AreEqual(8.5, s.GetCellValue("B2"));
        }

        [TestMethod()]
        public void TestSetCellContentsGetComplex2()
        {
            Spreadsheet s = new Spreadsheet();
            Formula f = new Formula("A2+A3");
            s.SetContentsOfCell("A1", "2.0");
            s.SetContentsOfCell("A2", "=A1");
            s.SetContentsOfCell("A1", "3.0");

            Assert.AreEqual(3.0, s.GetCellValue("A2"));
        }

        [TestMethod()]
        public void TestSetCellContentsGetComplex3()
        {
            Spreadsheet s = new Spreadsheet();
            Formula f = new Formula("A2+A3");

            s.SetContentsOfCell("A1", "2.0");
            s.SetContentsOfCell("A1", "Jim");

            Assert.AreEqual("Jim", s.GetCellValue("A1"));
        }

        [TestMethod()]
        public void TestSetCellContentsGetComplex4()
        {
            Spreadsheet s = new Spreadsheet();
            Formula f = new Formula("A2+A3");
            s.SetContentsOfCell("A1", "2.0");
            s.SetContentsOfCell("A2", "=A1");
            s.SetContentsOfCell("A1", "3.0");
            s.SetContentsOfCell("A1", "5.0");
            s.SetContentsOfCell("A2", "=A1");
            s.SetContentsOfCell("A1", "Jim");

            Assert.AreEqual("Jim", s.GetCellValue("A1"));
            Assert.AreEqual("Jim", s.GetCellValue("A2"));
        }


        [TestMethod()]
        public void TestSave()
        {
            Spreadsheet s = new Spreadsheet();
            Formula f = new Formula("A2+A3");
            s.SetContentsOfCell("A2", "1.5");
            s.SetContentsOfCell("A3", "2.0");
            s.SetContentsOfCell("A4", "=A2+A3");
            s.SetContentsOfCell("B2", "3");
            s.SetContentsOfCell("B7", "Jim");
            s.SetContentsOfCell("C4", "=B2+A4");
            s.Save("test.xml");
        }

        [TestMethod()]
        public void TestSaveGet()
        {
            Spreadsheet s = new Spreadsheet();
            Formula f = new Formula("A2+A3");
            s.SetContentsOfCell("A2", "1.5");
            s.SetContentsOfCell("A3", "2.0");
            s.SetContentsOfCell("A4", "=A2+A3");
            s.SetContentsOfCell("B2", "3");
            s.SetContentsOfCell("B7", "Jim");
            s.SetContentsOfCell("C4", "=B2+A4");
            s.Save("test.xml");

            Spreadsheet s1 = new Spreadsheet("test.xml", str => true, str => str, "default");
            s1.Save("test2.xml");
            Assert.AreEqual(f, s1.GetCellContents("A4"));
            Assert.AreEqual(1.5, s1.GetCellContents("A2"));
            Assert.AreEqual("default", s1.GetSavedVersion("test2.xml"));
        }

        [TestMethod()]
        public void StressTest2()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "10.0");
            s.SetContentsOfCell("A4", "14.0");
            for (int i = 5; i < 1000; i++)
            {
                s.SetContentsOfCell("A" + i, "=A" + (i - 1));
                s.GetCellValue("A" + (i - 1));
            }
        }
    }
}