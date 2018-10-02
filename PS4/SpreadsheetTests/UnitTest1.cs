using System;
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
        public void SingleSetandGet()
        {
            //Setting number
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("a1", 5);
            double number = (double)(sheet.GetCellContents("a1"));

            double ValidNumber = 5;
            Assert.AreEqual(number, ValidNumber);

            //Setting string
            sheet.SetCellContents("b1", "hello");
            string text = (string)sheet.GetCellContents("b1");

            Assert.AreEqual(text, "hello");

            //Setting Formula

            Formula formula = new Formula("a1 +5", s => s, s => true);
            sheet.SetCellContents("c1", formula);

            Formula TestFormula = (Formula)sheet.GetCellContents("c1");

            Assert.AreEqual(formula, TestFormula);
        }

        [TestMethod()]
        public void SetReturnsWithSingleCell()
        {
            Spreadsheet sheet = new Spreadsheet();

            HashSet<string> Dependents = (HashSet<string>)sheet.SetCellContents("a1", 5);
            Assert.IsTrue(Dependents.Count == 1);
        }

        [TestMethod()]
        public void SetReturnsWithMultipleCells()
        {
            Spreadsheet sheet = new Spreadsheet();


            sheet.SetCellContents("a1", 5);
            Formula formula = new Formula("a1+2");
            Formula formula1 = new Formula("a2+2");
            Formula formula2 = new Formula("a3+2");
            Formula formula3 = new Formula("a4+2");
            sheet.SetCellContents("a2", formula);
            sheet.SetCellContents("a3", formula1);
            sheet.SetCellContents("a4", formula2);
            sheet.SetCellContents("a5", formula3);

            HashSet<string> Dependents = (HashSet<string>)sheet.SetCellContents("a1", 5);
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
            Formula formula = new Formula("a1+5", s=>s, s=>true);
            Formula formula1 = new Formula("a3+5", s => s, s => true);


            sheet.SetCellContents("a1", 5);
            sheet.SetCellContents("a2", formula);
            sheet.SetCellContents("a3", 5);
            HashSet<string> Dependents = new HashSet<string>(sheet.SetCellContents("a2", formula1));

            Assert.IsTrue(Dependents.Count == 1);
            Assert.IsTrue(Dependents.Contains("a2"));
        }

        [TestMethod()]
        public void SetForExistingCell1()
        {
            Spreadsheet sheet = new Spreadsheet();
            Formula formula = new Formula("a1+5", s => s, s => true);
            Formula formula1 = new Formula("a2+5", s => s, s => true);
            Formula formula2 = new Formula("a3+5", s => s, s => true);



            sheet.SetCellContents("a1", 5);
            sheet.SetCellContents("a2", formula);
            sheet.SetCellContents("a3", formula1);
            sheet.SetCellContents("a4", formula2);

            HashSet<string> Dependents = new HashSet<string>(sheet.SetCellContents("a1", 5));

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

            sheet.SetCellContents("a1", 5);
            sheet.SetCellContents("a", "hello");
            sheet.SetCellContents("_", formula);
            sheet.SetCellContents("_1", 5);
            sheet.SetCellContents("Y_15", "hello");
            sheet.SetCellContents("____", formula);
            sheet.SetCellContents("a11111111", 5);
            sheet.SetCellContents("aaaaaaaaaa", "hello");
            sheet.SetCellContents("a______", formula);
            sheet.SetCellContents("a_2_43_42345__345", 5);


            double d1 = (double)sheet.GetCellContents("a1");
            string s1 = (string)sheet.GetCellContents("a");
            Formula f1 = (Formula)sheet.GetCellContents("_");
            double d2 = (double)sheet.GetCellContents("_1");
            string s2 = (string)sheet.GetCellContents("Y_15");
            Formula f2 = (Formula)sheet.GetCellContents("____");
            double d3 = (double)sheet.GetCellContents("a11111111");
            string s3 = (string)sheet.GetCellContents("aaaaaaaaaa");
            Formula f3 = (Formula)sheet.GetCellContents("a______");
            double d4 = (double)sheet.GetCellContents("a_2_43_42345__345");

            Assert.AreEqual(d1, 5);
            Assert.AreEqual(d2, 5);
            Assert.AreEqual(d3, 5);
            Assert.AreEqual(d4, 5);

            Assert.AreEqual(s1, "hello");
            Assert.AreEqual(s2, "hello");
            Assert.AreEqual(s3, "hello");

            Assert.AreEqual(f3, formula);
            Assert.AreEqual(f3, formula);
            Assert.AreEqual(f3, formula);
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

            sheet.SetCellContents("a1", 5);

            IEnumerator<string> names = sheet.GetNamesOfAllNonemptyCells().GetEnumerator();

            Assert.IsTrue(names.MoveNext());

            Assert.AreEqual("a1", names.Current);

            Assert.IsFalse(names.MoveNext());
        }


        [TestMethod()]
        public void GetNamesForMultipleCells()
        {
            Spreadsheet sheet = new Spreadsheet();
            Formula formula = new Formula("a1+5", s => s, s => true);


            sheet.SetCellContents("a1", 5);
            sheet.SetCellContents("a2", 5);
            sheet.SetCellContents("a3", "hello");
            sheet.SetCellContents("a4", formula);
            sheet.SetCellContents("a5", 5);
            sheet.SetCellContents("a6", "hello");


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
    }
    [TestClass]
    public class InvalidTests
    {
        
        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetWithSingleNumber()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("1", 5);
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetWithNumberAtFirst()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("1a", 5);
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetWithRandomSymbol()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("%456", 5);
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetWithRandom()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("a123%67", 5);
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetNullName()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents(null, 5);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetNullText()
        {
            Spreadsheet sheet = new Spreadsheet();
            string test = null;
            sheet.SetCellContents("a1", test);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetNullFormula()
        {
            Spreadsheet sheet = new Spreadsheet();
            Formula test = null;
            sheet.SetCellContents("a1", test);
        }

        [TestMethod()]
        public void SetReturnsWithMultipleWrong()
        {
            Spreadsheet sheet = new Spreadsheet();


            sheet.SetCellContents("a1", 5);
            Formula formula = new Formula("a1+2");
            Formula formula1 = new Formula("a2+2");
            Formula formula2 = new Formula("a3+2");
            Formula formula3 = new Formula("a6+2");
            sheet.SetCellContents("a2", formula);
            sheet.SetCellContents("a3", formula1);
            sheet.SetCellContents("a4", formula2);
            sheet.SetCellContents("a5", formula3);

            HashSet<string> Dependents = (HashSet<string>)sheet.SetCellContents("a1", 5);
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

            sheet.SetCellContents("a1", 5);

            Formula formula = new Formula("a3 +2");
            Formula formula1 = new Formula("a2 + 2");
            sheet.SetCellContents("a2", formula);
            sheet.SetCellContents("a3", formula1);
        }

        [TestMethod()]
        public void SetCircularDependecyDoesntChangeOnNewCell()
        {
            Spreadsheet sheet = new Spreadsheet();

            sheet.SetCellContents("a1", 5);

            Formula formula = new Formula("a3 +2");
            Formula formula1 = new Formula("a2 + 2");
            sheet.SetCellContents("a2", formula);
            try
            {
                sheet.SetCellContents("a3", formula1);
            }
            catch(CircularException e)
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

            sheet.SetCellContents("a1", 5);

            Formula formula2 = new Formula("a1 +5", s => s, s => true);
            sheet.SetCellContents("a2", formula2);

            Formula formula3 = new Formula("a2 +5", s => s, s => true);
            sheet.SetCellContents("a3", formula3);

            Formula formula4 = new Formula("a3 +5", s => s, s => true);
            sheet.SetCellContents("a4", formula4);

            Formula formula5 = new Formula("a4 +5", s => s, s => true);
            sheet.SetCellContents("a5", formula5);

            HashSet<string> DentsBeforeCircExcep = new HashSet<string>(sheet.SetCellContents("a1", 5));

            Formula BadFormula = new Formula("a1", s => s, s => true);
            try
            {
                sheet.SetCellContents("a3", BadFormula);
            }
            catch(CircularException E)
            {
                HashSet<string> DentsAfterCircExcep = new HashSet<string>(sheet.SetCellContents("a1", 5));
                Assert.IsTrue(DentsBeforeCircExcep.SetEquals(DentsAfterCircExcep));
            }
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetWithSingleNumber()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.GetCellContents("1");
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetWithNumberAtFirst()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.GetCellContents("1a");
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetWithRandomSymbol()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.GetCellContents("%456");
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetWithRandom()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.GetCellContents("a123%67");
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetNullName()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.GetCellContents(null);
        }


    }
}
