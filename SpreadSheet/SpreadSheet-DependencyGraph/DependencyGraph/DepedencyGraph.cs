// PS2 - Dependency Graph Implementation. 
// Richard Timpson
// CS 3500 - Software Practice
// 9/14/18
// University of Utah

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SpreadsheetUtilities
{

    /// <summary>
    /// A dependency graph implementation with several add, remove, and get methods. 
    /// </summary>
    public class DependencyGraph
    {
        /// <summary>
        /// Creates an empty DependencyGraph, using two Dictionaries dependents and dependees
        /// With a string as their key, and a Hashset as their value. 
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
        public int Size{ get; private set;}


        /// <summary>
        /// The size of dependees(s).
        /// </summary>
        public int this[string s]
        {
            get
            {
                if (HasDependees(s))
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
        /// Helper method to see if a specific dependee already exsits. 
        /// </summary>
        /// <param name="s">The key for dependee</param>
        /// <param name="t">The value of the dependent to check</param>
        /// <returns>True or false</returns>
        private bool HasSpecificDependent(string s, string t )
        {
            if (HasDependents(s) && dependents[s].Contains(t))
                return true;
            else
                return false;
        }
        /// <summary>
        /// Helper method to see if a specific dependee already exsits. 
        /// </summary>
        /// <param name="s">The key for dependee</param>
        /// <param name="t">The value of the dependent to check</param>
        /// <returns>True or false</returns>
        private bool HasSpecificDependee(string s, string t)
        {
            if (HasDependees(t) && dependees[t].Contains(s))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Returns an enumerator with  the dependents of 's'.
        /// Otherwise returns empty enumerator. 
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            if (HasDependents(s))
            {
                HashSet<string> dependentCopy = new HashSet<string>(dependents[s]);
                return dependentCopy;
            }
            else
                return Enumerable.Empty<string>();
        }

        /// <summary>
        /// Returns an enumerator with the dependees of 's', if it exists. 
        /// Otherwise returns empty enumerator. 
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            if (HasDependees(s))
            {
                HashSet<string> dependeesCopy = new HashSet<string>(dependees[s]);
                return dependeesCopy;
            }
            else
                return Enumerable.Empty<string>();
        }
        /// <summary>
        /// Helper method to add dependent. Used for AddDependency function
        /// </summary>
        /// <param name="s">Add 's' first</param>
        /// <param name="t">Add 't' after 's'</param>
        private void AddDependent(string s, string t)
        {
            if (HasDependents(s))
            {
                dependents[s].Add(t);
            }
            else if (!HasDependents(s))
            {
                HashSet<string> dependentsHash = new HashSet<string>();
                dependentsHash.Add(t);
                dependents.Add(s, dependentsHash);
            }
        }
        /// <summary>
        /// Helper method to add dependee.Used for AddDependency function
        /// </summary>
        /// <param name="s">Add 's' first</param>
        /// <param name="t">Add 't' after 's' </param>
        private void AddDependee(string s, string t)
        {
            if (HasDependees(t))
            {
                dependees[t].Add(s);
            }
            else if (!HasDependees(t))
            {
                HashSet<string> dependeesHash = new HashSet<string>();
                dependeesHash.Add(s);
                dependees.Add(t, dependeesHash);
            }
        }
        /// <summary>
        /// <para>Adds the ordered pair (s,t), if it doesn't exist</para>
        /// </summary>
        /// <param name="s"> s must be evaluated first. T depends on S</param>
        /// <param name="t"> t cannot be evaluated until s is</param>        /// 
        public void AddDependency(string s, string t)
        {
            //if the dependency does not already exist
            if (!(HasSpecificDependent(s,t) && HasSpecificDependee(s, t)))
            {
                AddDependee(s, t);
                AddDependent(s, t);
                Size++;
            }            
        }
        private void RemoveActualDependency(string s, string t)
        {
            dependents[s].Remove(t);
            dependees[t].Remove(s);
        }

        /// <summary>
        /// Removes the ordered pair (s,t), if it exists
        /// </summary>
        /// <param name="s"></param>
        /// <param name="t"></param>
        public void RemoveDependency(string s, string t)
        {
            // if the input exists in the form (s, t)
            if (HasSpecificDependent(s,t) && HasSpecificDependee(s,t))
            {
                //remove dependency of form (s,t)
                RemoveActualDependency(s, t);

                //remove hashset from dictionary if empty
                if (dependents[s].Count == 0)
                    dependents.Remove(s);
                if (dependees[t].Count == 0)
                    dependees.Remove(t);
                Size -= 1;
            }
            else
            {
                Debug.WriteLine("Trying to remove a dependency that does not exist");
            }
        }
        /// <summary>
        /// Removes all existing ordered pairs of the form (s,r).  Then, for each
        /// t in newDependents, adds the ordered pair (s,t).
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            // removing dependents if there are any
            if (HasDependents(s))
            {
                foreach (string t in dependents[s].ToList())
                {
                    RemoveDependency(s, t);
                }
            }
            //adding newdependents from list
            foreach (string t in newDependents.ToList())
            {
                AddDependency(s, t);
            }
            
        }
        /// <summary>
        /// Removes all existing ordered pairs of the form (r,s).  Then, for each 
        /// t in newDependees, adds the ordered pair (t,s).
        /// </summary>
        public void ReplaceDependees(string s, IEnumerable<string> newDependees)
        {
            //removing dependees if there are any 
            if (HasDependees(s))
            {
                foreach (string t in dependees[s].ToList())
                {
                    RemoveDependency(t, s);
                }
            }
            //adding new dependencies from list
            foreach (string t in newDependees.ToList())
            {
                AddDependency(t, s);
            }
        }

    }

}

