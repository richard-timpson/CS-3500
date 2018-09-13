// Skeleton implementation written by Joe Zachary for CS 3500, September 2013.
// Version 1.1 (Fixed error in comment for RemoveDependency.)
// Version 1.2 - Daniel Kopta 
//               (Clarified meaning of dependent and dependee.)
//               (Clarified names in solution/project structure.)

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SpreadsheetUtilities
{

    /// <summary>
    /// (s1,t1) is an ordered pair of strings
    /// t1 depends on s1; s1 must be evaluated before t1
    /// 
    /// A DependencyGraph can be modeled as a set of ordered pairs of strings.  Two ordered pairs
    /// (s1,t1) and (s2,t2) are considered equal if and only if s1 equals s2 and t1 equals t2.
    /// Recall that sets never contain duplicates.  If an attempt is made to add an element to a 
    /// set, and the element is already in the set, the set remains unchanged.
    /// f
    /// Given a DependencyGraph DG:
    /// 
    ///    (1) If s is a string, the set of all strings t such that (s,t) is in DG is called dependents(s).
    ///        (The set of things that depend on s)    
    ///        
    ///    (2) If s is a string, the set of all strings t such that (t,s) is in DG is called dependees(s).
    ///        (The set of things that s depends on) 
    //
    // For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
    //     dependents("a") = {"b", "c"}
    //     dependents("b") = {"d"}
    //     dependents("c") = {}
    //     dependents("d") = {"d"}
    //     dependees("a") = {}
    //     dependees("b") = {"a"}
    //     dependees("c") = {"a"}
    //     dependees("d") = {"b", "d"}
    /// </summary>
    public class DependencyGraph
    {
        /// <summary>
        /// Creates an empty DependencyGraph.
        /// </summary>
        private readonly Dictionary<string, HashSet<string>> dependents;
        private readonly Dictionary<string, HashSet<string>> dependees;
        public DependencyGraph()
        {
            dependents = new Dictionary<string, HashSet<string>>();
            dependees = new Dictionary<string, HashSet<string>>();
        }


        /// <summary>
        /// The number of ordered pairs in the DependencyGraph.
        /// </summary>
        public int Size
        {
            get
            {
                if (dependents.Count == 0)
                    return 0;
                else
                {
                    int size = 0;
                    foreach (KeyValuePair<string, HashSet<string>> dependent in dependents)
                    {
                        size += dependent.Value.Count();
                    }
                    return size;
                }
            }
        }


        /// <summary>
        /// The size of dependees(s).
        /// This property is an example of an indexer.  If dg is a DependencyGraph, you would
        /// invoke it like this:
        /// dg["a"]
        /// It should return the size of dependees("a")
        /// </summary>
        public int this[string s]
        {
            get
            {
                if (dependees.ContainsKey(s))
                    return dependees[s].Count();
                else
                    return 0;
            }
        }


        /// <summary>
        /// Reports whether dependents(s) is non-empty.
        /// </summary>
        public bool HasDependents(string s)
        {
            if (dependents.ContainsKey(s))
                return !(dependents[s].Count == 0);
            else
                return false;
        }


        /// <summary>
        /// Reports whether dependees(s) is non-empty.
        /// </summary>
        public bool HasDependees(string s)
        {
            if (dependees.ContainsKey(s))
                return !(dependees[s].Count == 0);
            else
                return false;
        }


        /// <summary>
        /// Enumerates dependents(s).
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            if (dependents.ContainsKey(s))
                return dependents[s];
            else
                return Enumerable.Empty<string>();
        }

        /// <summary>
        /// Enumerates dependees(s).
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            if (dependees.ContainsKey(s))
                return dependees[s];
            else
                return Enumerable.Empty<string>();
        }


        /// <summary>
        /// <para>Adds the ordered pair (s,t), if it doesn't exist</para>
        /// 
        /// <para>This should be thought of as:</para>   
        /// 
        ///   t depends on s
        ///
        /// </summary>
        /// <param name="s"> s must be evaluated first. T depends on S</param>
        /// <param name="t"> t cannot be evaluated until s is</param>        /// 
        public void AddDependency(string s, string t)
        {
            if (dependents.ContainsKey(s) && dependees.ContainsKey(s))
            {
                Debug.WriteLine("Trying to add duplicate dependency");
            }
            // We need two new hashsets, that we append to both 
            else if (!dependents.ContainsKey(s) && !dependees.ContainsKey(t))
            {
                //create two new hashsets for both
                HashSet<string> dependentsHash = new HashSet<string>();
                HashSet<string> dependeesHash = new HashSet<string>();
                // add t to dependents hashset, and s to dependees hashset
                dependentsHash.Add(t);
                dependeesHash.Add(s);
                // add s and new hashset to dependents, 
                dependents.Add(s, dependentsHash);
                // add t and new dependees to dependees
                dependees.Add(t, dependeesHash);
            }
            // We need one new hashset for the dependees
            else if (dependents.ContainsKey(s) && !dependees.ContainsKey(t))
            {
                // create new hashset for dependees
                HashSet<string> dependeesHash = new HashSet<string>();
                // add s to new dependees hashset
                dependeesHash.Add(s);
                // add new dependee hashset to dependenes, at the key t
                dependees.Add(t, dependeesHash);
                // add t to the already existing hashset for dependents, at key s
                dependents[s].Add(t);
            }
            else if (!dependents.ContainsKey(s) && dependees.ContainsKey(t))
            {
                // create new hashset for dependents
                HashSet<string> dependentsHash = new HashSet<string>();
                // add t to new dependents hashset
                dependentsHash.Add(t);
                // and new dependents hashset to dependees, at the key s
                dependents.Add(s, dependentsHash);
                // add t to the already existing hashsett for dependees, at key t
                dependees[t].Add(s);
            }
        }
        private void AddDependencyRecursive(string s, string t)
        {

        }







            //if (!dependents.ContainsKey(s))
            //{
            //    HashSet<string> dependentsValue = new HashSet<string>();
            //    dependentsValue.Add(t);
            //    dependents.Add(s, dependentsValue);

            //    HashSet<string> dependeesValue = new HashSet<string>();
            //    dependeesValue.Add(s);
            //    dependees.Add(t, dependeesValue);
            //}
            //else if (!dependees.ContainsKey(t))
            //{
            //    HashSet<string> dependeesValue = new HashSet<string>();
            //    dependeesValue.Add(s);
            //    dependees.Add(t, dependeesValue);

            //    dependents[s].Add(t);
            //}
            //else
            //{
            //    dependents[s].Add(t);
            //    dependees[t].Add(s);
            //}
        



        /// <summary>
        /// Removes the ordered pair (s,t), if it exists
        /// </summary>
        /// <param name="s"></param>
        /// <param name="t"></param>
        public void RemoveDependency(string s, string t)
        {
            if (dependents.ContainsKey(s))
            {
                if (dependents[s].Remove(t))
                    dependees[t].Remove(s);
            }
        }


        /// <summary>
        /// Removes all existing ordered pairs of the form (s,r).  Then, for each
        /// t in newDependents, adds the ordered pair (s,t).
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            if (dependents.Remove(s))
            {
                HashSet<string> newDep = new HashSet<string>();
                newDep.UnionWith(newDependents);
                dependents.Add(s, newDep);
            }
        }


        /// <summary>
        /// Removes all existing ordered pairs of the form (r,s).  Then, for each 
        /// t in newDependees, adds the ordered pair (t,s).
        /// </summary>
        public void ReplaceDependees(string s, IEnumerable<string> newDependees)
        {
            if (dependees.Remove(s))
            {
                HashSet<string> newDep = new HashSet<string>();
                newDep.UnionWith(newDependees);
                dependees.Add(s, newDep);
            }
        }

    }

}

