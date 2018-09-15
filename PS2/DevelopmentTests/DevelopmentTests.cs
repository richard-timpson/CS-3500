using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;


namespace DevelopmentTests
{
    /// <summary>
    ///This is a test class for DependencyGraphTest and is intended
    ///to contain all DependencyGraphTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DependencyGraphTest
    {

        /// <summary>
        ///Empty graph should contain nothing
        ///</summary>
        [TestMethod()]
        public void SimpleEmptyTest()
        {
            DependencyGraph t = new DependencyGraph();
            Assert.AreEqual(0, t.Size);
        }


        /// <summary>
        ///Empty graph should contain nothing
        ///</summary>
        [TestMethod()]
        public void SimpleEmptyRemoveTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            Assert.AreEqual(1, t.Size);
            t.RemoveDependency("x", "y");
            Assert.AreEqual(0, t.Size);
        }


        /// <summary>
        ///Empty graph should contain nothing
        ///</summary>
        [TestMethod()]
        public void SimpleEmptyTest2()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            IEnumerator<string> e1 = t.GetDependees("y").GetEnumerator();
            Assert.IsTrue(e1.MoveNext());
            Assert.AreEqual("x", e1.Current);
            IEnumerator<string> e2 = t.GetDependents("x").GetEnumerator();
            Assert.IsTrue(e2.MoveNext());
            Assert.AreEqual("y", e2.Current);
            t.RemoveDependency("x", "y");
            Assert.IsFalse(t.GetDependees("y").GetEnumerator().MoveNext());
            Assert.IsFalse(t.GetDependents("x").GetEnumerator().MoveNext());
        }


        /// <summary>
        ///Replace on an empty DG shouldn't fail
        ///</summary>
        [TestMethod()]
        public void SimpleReplaceTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            Assert.AreEqual(t.Size, 1);
            t.RemoveDependency("x", "y");
            t.ReplaceDependents("x", new HashSet<string>());
            t.ReplaceDependees("y", new HashSet<string>());
        }



        ///<summary>
        ///It should be possibe to have more than one DG at a time.
        ///</summary>
        [TestMethod()]
        public void StaticTest()
        {
            DependencyGraph t1 = new DependencyGraph();
            DependencyGraph t2 = new DependencyGraph();
            t1.AddDependency("x", "y");
            Assert.AreEqual(1, t1.Size);
            Assert.AreEqual(0, t2.Size);
        }




        /// <summary>
        ///Non-empty graph contains something
        ///</summary>
        [TestMethod()]
        public void SizeTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("c", "b");
            t.AddDependency("b", "d");
            Assert.AreEqual(4, t.Size);
        }


        /// <summary>
        ///Non-empty graph contains something
        ///</summary>
        [TestMethod()]
        public void SizeTest2()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("c", "b");
            t.AddDependency("b", "d");

            IEnumerator<string> e = t.GetDependees("a").GetEnumerator();
            Assert.IsFalse(e.MoveNext());

            e = t.GetDependees("b").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            String s1 = e.Current;
            Assert.IsTrue(e.MoveNext());
            String s2 = e.Current;
            Assert.IsFalse(e.MoveNext());
            Assert.IsTrue(((s1 == "a") && (s2 == "c")) || ((s1 == "c") && (s2 == "a")));

            e = t.GetDependees("c").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("a", e.Current);
            Assert.IsFalse(e.MoveNext());

            e = t.GetDependees("d").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("b", e.Current);
            Assert.IsFalse(e.MoveNext());
        }


        /// <summary>
        ///Non-empty graph contains something
        ///</summary>
        [TestMethod()]
        public void SizeTest3()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("a", "b");
            t.AddDependency("c", "b");
            t.AddDependency("b", "d");
            t.AddDependency("c", "b");
            Assert.AreEqual(4, t.Size);
        }





        /// <summary>
        ///Non-empty graph contains something
        ///</summary>
        [TestMethod()]
        public void SizeTest4()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("a", "d");
            t.AddDependency("c", "b");
            t.RemoveDependency("a", "d");
            t.AddDependency("e", "b");
            t.AddDependency("b", "d");
            t.RemoveDependency("e", "b");
            t.RemoveDependency("x", "y");
            Assert.AreEqual(4, t.Size);
        }


        /// <summary>
        ///Non-empty graph contains something
        ///</summary>
        [TestMethod()]
        public void SizeTest5()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "b");
            t.AddDependency("a", "z");
            t.ReplaceDependents("b", new HashSet<string>());
            t.AddDependency("y", "b");
            t.ReplaceDependents("a", new HashSet<string>() { "c" });
            t.AddDependency("w", "d");
            t.ReplaceDependees("b", new HashSet<string>() { "a", "c" });
            t.ReplaceDependees("d", new HashSet<string>() { "b" });

            IEnumerator<string> e = t.GetDependees("a").GetEnumerator();
            Assert.IsFalse(e.MoveNext());

            e = t.GetDependees("b").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            String s1 = e.Current;
            Assert.IsTrue(e.MoveNext());
            String s2 = e.Current;
            Assert.IsFalse(e.MoveNext());
            Assert.IsTrue(((s1 == "a") && (s2 == "c")) || ((s1 == "c") && (s2 == "a")));

            e = t.GetDependees("c").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("a", e.Current);
            Assert.IsFalse(e.MoveNext());

            e = t.GetDependees("d").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("b", e.Current);
            Assert.IsFalse(e.MoveNext());
        }
        /// <summary>
        ///Using lots of data
        ///</summary>
        [TestMethod()]
        public void StressTest()
        {
            // Dependency graph
            DependencyGraph t = new DependencyGraph();

            // A bunch of strings to use
            const int SIZE = 200;
            string[] letters = new string[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                letters[i] = ("" + (char)('a' + i));
            }

            // The correct answers
            HashSet<string>[] dents = new HashSet<string>[SIZE];
            HashSet<string>[] dees = new HashSet<string>[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                dents[i] = new HashSet<string>();
                dees[i] = new HashSet<string>();
            }

            // Add a bunch of dependencies
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 1; j < SIZE; j++)
                {
                    t.AddDependency(letters[i], letters[j]);
                    dents[i].Add(letters[j]);
                    dees[j].Add(letters[i]);
                }
            }

            // Remove a bunch of dependencies
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 4; j < SIZE; j += 4)
                {
                    t.RemoveDependency(letters[i], letters[j]);
                    dents[i].Remove(letters[j]);
                    dees[j].Remove(letters[i]);
                }
            }

            // Add some back
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 1; j < SIZE; j += 2)
                {
                    t.AddDependency(letters[i], letters[j]);
                    dents[i].Add(letters[j]);
                    dees[j].Add(letters[i]);
                }
            }

            // Remove some more
            for (int i = 0; i < SIZE; i += 2)
            {
                for (int j = i + 3; j < SIZE; j += 3)
                {
                    t.RemoveDependency(letters[i], letters[j]);
                    dents[i].Remove(letters[j]);
                    dees[j].Remove(letters[i]);
                }
            }

            // Make sure everything is right
            for (int i = 0; i < SIZE; i++)
            {
                Assert.IsTrue(dents[i].SetEquals(new HashSet<string>(t.GetDependents(letters[i]))));
                Assert.IsTrue(dees[i].SetEquals(new HashSet<string>(t.GetDependees(letters[i]))));
            }
        }
        /// <summary>
        /// simple size test
        /// </summary>
        [TestMethod()]
        public void RichardSizeTest1()
        {
            DependencyGraph t = new DependencyGraph();

            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("b", "c");
            t.AddDependency("c", "d");

            Assert.AreEqual(4, t.Size);
        }
        /// <summary>
        /// Testing the indexer
        /// </summary>
        [TestMethod()]
        public void RichardIndexerTest1()
        {
            DependencyGraph t = new DependencyGraph();

            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("b", "c");
            t.AddDependency("c", "d");

            int size = t["c"];

            Assert.AreEqual(2, size);
        }
        /// <summary>
        /// Testing the indexer with remove
        /// </summary>
        [TestMethod()]
        public void RichardIndexerTest2()
        {
            DependencyGraph t = new DependencyGraph();

            t.AddDependency("a", "b");
            t.RemoveDependency("a", "b");

            t.AddDependency("a", "c");
            t.RemoveDependency("a", "c");

            t.AddDependency("b", "c");
            t.RemoveDependency("b", "c");

            t.AddDependency("c", "d");
            t.AddDependency("e", "c");


            int size = t["c"];

            Assert.AreEqual(1, size);
        }
        /// <summary>
        /// Testing the indexer with no value
        /// </summary>
        [TestMethod()]
        public void RichardIndexerTest3()
        {
            DependencyGraph t = new DependencyGraph();

            t.AddDependency("a", "b");
            t.RemoveDependency("a", "b");

            t.AddDependency("a", "c");
            t.RemoveDependency("a", "c");

            t.AddDependency("b", "c");
            t.RemoveDependency("b", "c");

            t.AddDependency("c", "d");
            t.AddDependency("e", "c");
            t.RemoveDependency("e", "c");


            int size = t["c"];

            Assert.AreEqual(0, size);
        }
        /// <summary>
        /// Testing with value that isn't an ordered pair
        /// </summary>
        [TestMethod()]
        public void RichardIndexerTest4()
        {
            DependencyGraph t = new DependencyGraph();

            t.AddDependency("a", "b");

            int size = t["c"];

            Assert.AreEqual(0, size);
        }
        /// <summary>
        /// Testing add
        /// </summary>
        [TestMethod()]
        public void RichardGetDependentsTest1()
        {
            DependencyGraph t = new DependencyGraph();

            t.AddDependency("a", "b");
            IEnumerator<string> e = t.GetDependents("a").GetEnumerator();

            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("b", e.Current);
        }
        /// <summary>
        /// Testing add with multiple
        /// </summary>
        [TestMethod()]
        public void RichardGetDependentsTest2()
        {
            DependencyGraph t = new DependencyGraph();

            t.AddDependency("a", "b");

            t.AddDependency("b", "c");
            t.AddDependency("c", "d");
            t.AddDependency("d", "e");


            IEnumerator<string> depDict = t.GetDependents("a").GetEnumerator();

            Assert.IsTrue(depDict.MoveNext());
            Assert.AreEqual("b", depDict.Current);

            depDict = t.GetDependents("b").GetEnumerator();

            Assert.IsTrue(depDict.MoveNext());
            Assert.AreEqual("c", depDict.Current);

            depDict = t.GetDependents("c").GetEnumerator();

            Assert.IsTrue(depDict.MoveNext());
            Assert.AreEqual("d", depDict.Current);
        }
        /// <summary>
        /// Testing add with remove to none
        /// </summary>
        [TestMethod()]
        public void RichardGetDependentsTest3()
        {
            DependencyGraph t = new DependencyGraph();

            t.AddDependency("a", "b");

            t.AddDependency("b", "c");
            t.AddDependency("c", "d");
            t.AddDependency("d", "e");


            IEnumerator<string> depDict = t.GetDependents("a").GetEnumerator();

            Assert.IsTrue(depDict.MoveNext());
            Assert.AreEqual("b", depDict.Current);

            depDict = t.GetDependents("b").GetEnumerator();

            Assert.IsTrue(depDict.MoveNext());
            Assert.AreEqual("c", depDict.Current);

            depDict = t.GetDependents("c").GetEnumerator();

            Assert.IsTrue(depDict.MoveNext());
            Assert.AreEqual("d", depDict.Current);

            t.RemoveDependency("a", "b");

            depDict = t.GetDependents("a").GetEnumerator();

            Assert.IsFalse(depDict.MoveNext());

        }
        /// <summary>
        /// Testing add multiple dependents to one dependee
        /// </summary>
        [TestMethod()]
        public void RichardGetDependentsTest4()
        {
            DependencyGraph t = new DependencyGraph();

            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("a", "d");
            t.AddDependency("a", "e");


            IEnumerator<string> depDict = t.GetDependents("a").GetEnumerator();

            Assert.IsTrue(depDict.MoveNext());
            Assert.AreEqual("b", depDict.Current);


            Assert.IsTrue(depDict.MoveNext());
            Assert.AreEqual("c", depDict.Current);


            Assert.IsTrue(depDict.MoveNext());
            Assert.AreEqual("d", depDict.Current);

            Assert.IsTrue(depDict.MoveNext());
            Assert.AreEqual("e", depDict.Current);
        }
        /// <summary>
        /// testing add with multiple dependents to one dependee, and remove multiple dependents with one dependee
        /// </summary>
        [TestMethod()]
        public void RichardGetDependentsTest5()
        {
            DependencyGraph t = new DependencyGraph();

            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("a", "d");
            t.AddDependency("a", "e");


            IEnumerator<string> depDict = t.GetDependents("a").GetEnumerator();

            Assert.IsTrue(depDict.MoveNext());
            Assert.AreEqual("b", depDict.Current);


            Assert.IsTrue(depDict.MoveNext());
            Assert.AreEqual("c", depDict.Current);


            Assert.IsTrue(depDict.MoveNext());
            Assert.AreEqual("d", depDict.Current);

            Assert.IsTrue(depDict.MoveNext());
            Assert.AreEqual("e", depDict.Current);

            t.RemoveDependency("a", "b");
            t.RemoveDependency("a", "c");

            depDict = t.GetDependents("a").GetEnumerator();

            Assert.IsTrue(depDict.MoveNext());
            Assert.AreEqual("d", depDict.Current);

            Assert.IsTrue(depDict.MoveNext());
            Assert.AreEqual("e", depDict.Current);

            Assert.IsFalse(depDict.MoveNext());

        }
        /// <summary>
        /// Testing add with multiple dependee's to one dependent
        /// </summary>
        [TestMethod()]
        public void RichardGetDependentsTest6()
        {
            DependencyGraph t = new DependencyGraph();

            t.AddDependency("b", "a");
            t.AddDependency("c", "a");
            t.AddDependency("d", "a");
            t.AddDependency("e", "a");


            IEnumerator<string> depDict = t.GetDependents("b").GetEnumerator();

            Assert.IsTrue(depDict.MoveNext());
            Assert.AreEqual("a", depDict.Current);

            depDict = t.GetDependents("c").GetEnumerator();

            Assert.IsTrue(depDict.MoveNext());
            Assert.AreEqual("a", depDict.Current);

            depDict = t.GetDependents("d").GetEnumerator();

            Assert.IsTrue(depDict.MoveNext());
            Assert.AreEqual("a", depDict.Current);

            depDict = t.GetDependents("e").GetEnumerator();

            Assert.IsTrue(depDict.MoveNext());
            Assert.AreEqual("a", depDict.Current);

            Assert.IsFalse(depDict.MoveNext());
        }
        /// <summary>
        /// Testing add with multiple dependee's to one dependent
        /// </summary>
        [TestMethod()]
        public void RichardGetDependentsTest7()
        {
            DependencyGraph t = new DependencyGraph();

            t.AddDependency("b", "a");
            t.AddDependency("c", "a");
            t.AddDependency("d", "a");
            t.AddDependency("e", "a");


            IEnumerator<string> depDict = t.GetDependents("b").GetEnumerator();

            Assert.IsTrue(depDict.MoveNext());
            Assert.AreEqual("a", depDict.Current);

            depDict = t.GetDependents("c").GetEnumerator();

            Assert.IsTrue(depDict.MoveNext());
            Assert.AreEqual("a", depDict.Current);

            depDict = t.GetDependents("d").GetEnumerator();

            Assert.IsTrue(depDict.MoveNext());
            Assert.AreEqual("a", depDict.Current);

            depDict = t.GetDependents("e").GetEnumerator();

            Assert.IsTrue(depDict.MoveNext());
            Assert.AreEqual("a", depDict.Current);

            t.RemoveDependency("d", "a");
            t.RemoveDependency("b", "a");

            Assert.IsFalse(depDict.MoveNext());
        }
        /// <summary>
        /// Testing add 
        /// </summary>
        [TestMethod()]
        public void RichardGetDependeesTest1()
        {
            DependencyGraph t = new DependencyGraph();

            t.AddDependency("a", "b");
            IEnumerator<string> e = t.GetDependees("b").GetEnumerator();

            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("a", e.Current);
        }
        /// <summary>
        /// testing add with multiple
        /// </summary>
        [TestMethod()]
        public void RichardGetDependeesTest2()
        {
            DependencyGraph t = new DependencyGraph();

            t.AddDependency("a", "b");

            t.AddDependency("b", "c");
            t.AddDependency("c", "d");
            t.AddDependency("d", "e");


            IEnumerator<string> depDict = t.GetDependees("b").GetEnumerator();

            Assert.IsTrue(depDict.MoveNext());
            Assert.AreEqual("a", depDict.Current);

            depDict = t.GetDependees("c").GetEnumerator();

            Assert.IsTrue(depDict.MoveNext());
            Assert.AreEqual("b", depDict.Current);

            depDict = t.GetDependees("d").GetEnumerator();

            Assert.IsTrue(depDict.MoveNext());
            Assert.AreEqual("c", depDict.Current);
        }
        /// <summary>
        /// Testing add and remove with none
        /// </summary>
        [TestMethod()]
        public void RichardGetDependeesTest3()
        {
            DependencyGraph t = new DependencyGraph();

            t.AddDependency("a", "b");

            t.AddDependency("b", "c");
            t.AddDependency("c", "d");
            t.AddDependency("d", "e");


            IEnumerator<string> depDict = t.GetDependees("b").GetEnumerator();

            Assert.IsTrue(depDict.MoveNext());
            Assert.AreEqual("a", depDict.Current);

            depDict = t.GetDependees("c").GetEnumerator();

            Assert.IsTrue(depDict.MoveNext());
            Assert.AreEqual("b", depDict.Current);

            depDict = t.GetDependees("d").GetEnumerator();

            Assert.IsTrue(depDict.MoveNext());
            Assert.AreEqual("c", depDict.Current);

            t.RemoveDependency("a", "b");

            depDict = t.GetDependents("a").GetEnumerator();

            Assert.IsFalse(depDict.MoveNext());

        }
        [TestMethod()]
        public void RichardDuplicateTest()
        {
            DependencyGraph t = new DependencyGraph();

            t.AddDependency("a", "b");
            t.AddDependency("a", "b");

            IEnumerator<string> depDict = t.GetDependents("a").GetEnumerator();
            Assert.IsTrue(depDict.MoveNext());

            Assert.AreEqual("b", depDict.Current);

            Assert.IsFalse(depDict.MoveNext());
        }
        [TestMethod()]
        public void RichardDuplicateTest1()
        {
            DependencyGraph t = new DependencyGraph();

            t.AddDependency("a", "b");
            t.AddDependency("a", "a");

            IEnumerator<string> dentDict = t.GetDependents("a").GetEnumerator();

            Assert.IsTrue(dentDict.MoveNext());
            Assert.AreEqual("b", dentDict.Current);

            Assert.IsTrue(dentDict.MoveNext());
            Assert.AreEqual("a", dentDict.Current);

            IEnumerator<string> deeDict = t.GetDependees("a").GetEnumerator();

            Assert.IsTrue(deeDict.MoveNext());
            Assert.AreEqual("a", deeDict.Current);
        }
        [TestMethod()]
        public void RichardEmptyDependencyTest()
        {
            DependencyGraph t = new DependencyGraph();

            t.AddDependency(" ", " ");

            IEnumerator<string> depDict = t.GetDependees(" ").GetEnumerator();


            Assert.IsTrue(depDict.MoveNext());
            Assert.AreEqual(" ", depDict.Current);
        }
        [TestMethod()]
        public void RichardRemoveEmptyDependencyTest()
        {
            DependencyGraph t = new DependencyGraph();

            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.RemoveDependency("a", "b");
            t.RemoveDependency("a", "d");

            IEnumerator<string> depDict = t.GetDependents("a").GetEnumerator();

            Assert.IsTrue(depDict.MoveNext());
            Assert.AreEqual("c", depDict.Current);
        }
        [TestMethod()]
        public void RichardReplaceDependentsWithEmptyTest()
        {
            DependencyGraph t = new DependencyGraph();

            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("a", "c");

            HashSet<string> emptyHash = new HashSet<string>();

            t.ReplaceDependents("a", emptyHash);

            IEnumerator<string> dentDict = t.GetDependents("a").GetEnumerator();

            Assert.IsFalse(dentDict.MoveNext());
        }
        [TestMethod()]
        public void RichardReplaceDependeesWithEmptyTest()
        {
            DependencyGraph t = new DependencyGraph();

            t.AddDependency("a", "b");
            t.AddDependency("b", "b");
            t.AddDependency("c", "b");
            t.AddDependency("d", "b");

            HashSet<string> emptyHash = new HashSet<string>();

            t.ReplaceDependees("b", emptyHash);

            IEnumerator<string> deeDict = t.GetDependees("b").GetEnumerator();

            Assert.IsFalse(deeDict.MoveNext());
        }
        [TestMethod()]
        public void RichardReplaceDependentsWithNewTest()
        {
            DependencyGraph t = new DependencyGraph();

            t.AddDependency("a", "b");

            HashSet<string> dentHashSet = new HashSet<string>();

            dentHashSet.Add("x");
            dentHashSet.Add("y");

            t.ReplaceDependees("a", dentHashSet);

            IEnumerator<string> deeDict = t.GetDependees("a").GetEnumerator();

            Assert.IsTrue(deeDict.MoveNext());
            Assert.AreEqual("x", deeDict.Current);

            Assert.IsTrue(deeDict.MoveNext());
            Assert.AreEqual("y", deeDict.Current);
        }
    }
}




