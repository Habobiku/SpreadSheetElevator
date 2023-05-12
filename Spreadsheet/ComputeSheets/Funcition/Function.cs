using System.Text.RegularExpressions;
using Spreadsheet.Models;

namespace Spreadsheet.ComputeSheets.Funcition
{
    public abstract class Function
    {
        public abstract string Name { get; }
        public abstract object Execute(Sheet sheetData, string formula);

        public List<string> CellReferences { get; set; }

        public void SetCellReferences(string formula)
        {
            this.CellReferences = SplitFromula(formula);
        }

        public bool Contains(string command)
        {
            return command.Contains(this.Name);
        }

        public Cell? GetCell(Sheet sheetData, string cellRef)
        {

            if (cellRef.Length < 2)
                return new Cell { Value = Convert.ToInt64(cellRef) };

            var matches = Regex.Matches(cellRef, @"([A-Z]+)(\d+)");
            if (matches.Count != 1 || matches[0].Groups.Count != 3)
            {
                return null;
            }

            var columnName = matches[0].Groups[1].Value;
            var rowIndex = int.Parse(matches[0].Groups[2].Value) - 1;

            if (sheetData?.Data != null && sheetData.Data.Count > rowIndex)
            {
                var row = sheetData.Data[rowIndex];
                if (row.Count > GetColumnIndex(columnName))
                {
                    return row[GetColumnIndex(columnName)];
                }
            }

            return null;
        }

        public virtual List<string> SplitFromula(string formula)
        {
            return formula
        .Replace($"{Name}(", "")
        .Replace(")", "")
        .Split(",")
        .Select(cellRef => cellRef.Trim()).ToList();
        }

        private int GetColumnIndex(string columnName)
        {
            var index = 0;
            foreach (var c in columnName)
            {
                index *= 26;
                index += c - 'A' + 1;
            }
            return index - 1;
        }
    }

}

