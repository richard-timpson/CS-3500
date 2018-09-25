using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SpreadsheetUtilities;

namespace SS
{
    public class Spreadsheet : AbstractSpreadsheet
    {
        private Dictionary<string, string> NonemptyCells = new Dictionary<string, string>();
        public override IEnumerable<String> GetNamesOfAllNonemptyCells()
        {
            List<string> NonemptyCellNames = new List<string>();

            foreach (KeyValuePair<string, string> cell in NonemptyCells)
            {
                NonemptyCellNames.Add(cell.Key);
            }
            return NonemptyCellNames;
        }


        public override object GetCellContents(string name)
        {
            throw new NotImplementedException();
        }


        public override ISet<String> SetCellContents(String name, double number)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }


    }



}
