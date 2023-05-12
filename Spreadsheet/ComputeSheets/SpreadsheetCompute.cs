using System;
using System.Text.RegularExpressions;
using Spreadsheet.ComputeSheets.Funcition;
using Spreadsheet.Models;

namespace Spreadsheet.ComputeSheets
{
    public static class SpreadsheetCompute
    {

        public static void Compute(SpreadsheetResponse spreadsheet)
        {
            List<Function> functions = InitFunctions.AllFunctionsInit();

            foreach (var sheet in spreadsheet.Sheets)
            {
                foreach (var data in sheet.Data)
                {
                    for (int col = 0; col < data.Count; col++)
                    {
                        var cell = data[col];
                        if (cell.Type == DataType.Formula)
                        {
                            var formula = (cell.Value as string).Substring(1);
                            foreach (var function in functions)
                            {
                                if (function.Contains(formula) || Regex.IsMatch(formula,function.Name))
                                {
                                    if (function.Execute(sheet, formula) is string && (string)function.Execute(sheet, formula) == "error")
                                    {
                                        data[col] = new Cell { Value = "#ERROR: Incompatible types" };
                                    }
                                    else
                                    {
                                        data[col].Value = function.Execute(sheet, formula);
                                    }
                                }
                            }

                        }
                    }
                }
            }
        }
    }
}

