using System.Text;
using Spreadsheet.Models;

namespace Spreadsheet.ComputeSheets.Funcition
{
    public class AssignValue : Function
    {
        public override string Name => "^[A-Z][0-9].*";

        public override object Execute(Sheet sheetData, string formula)
        {
            SetCellReferences(formula);

            var cell = GetCell(sheetData, CellReferences[0]);
            var value = cell.Value;

            if (value is string && ((string)value).Length == 3)
            {
                if (cell.Type == DataType.Formula)
                {
                    value = Execute(sheetData, (string)value);
                }
            }

            return value;
        }
    }

    public class SumFunction : Function
    {
        public override string Name => "SUM";

        public override object Execute(Sheet sheetData, string formula)
        {
            SetCellReferences(formula);
            var sum = 0.0;

            foreach (var cellRef in CellReferences)
            {
                var cell = GetCell(sheetData, cellRef);
                if (cell != null && cell.Type == DataType.Number && cell.Value is long value)
                {
                    sum += value;
                }
                else
                {
                    return "error";
                }
            }

            return (long)sum;
        }
    }

    public class MultiplyFunction : Function
    {
        public override string Name => "MULTIPLY";

        public override object Execute(Sheet sheetData, string formula)
        {
            SetCellReferences(formula);

            var result = 1.0;

            foreach (var cellRef in CellReferences)
            {
                var cell = GetCell(sheetData, cellRef);
                if (cell != null && cell.Type == DataType.Number && cell.Value is long value)
                {
                    result *= value;
                }
                else
                {
                    return "error";
                }
            }

            return (long)result;
        }
    }


    public class DivideFunction : Function
    {
        public override string Name => "DIVIDE";

        public override object Execute(Sheet sheetData, string formula)
        {
            SetCellReferences(formula);
            var result = 0.0;
            foreach (var cellRef in CellReferences)
            {
                var cell = GetCell(sheetData, cellRef);
                if (cell != null && cell.Type == DataType.Number && cell.Value is long value)
                {
                    if (value == 0)
                    {
                        return double.PositiveInfinity;
                    }
                    if (result == 0)
                        result = value;
                    else
                        result /= value;
                }
                else
                {
                    return "error";
                }
            }



            return result;
        }
    }

    public class GtFunction : Function
    {
        public override string Name => "GT";

        public override object Execute(Sheet sheetData, string formula)
        {

            SetCellReferences(formula);
            if(CellReferences[0][0] != 'I')
            {
                var cell = GetCell(sheetData, CellReferences[0]);
                var cellSecond = GetCell(sheetData, CellReferences[1]);
                if (cell != null && cellSecond != null && cell.Type == DataType.Number && cell.Value is long value && cellSecond.Type == DataType.Number && cellSecond.Value is long valueSecond)
                {
                    return value > valueSecond;
                }
            }

            return "error";
        }
    }

    public class EqFunction : Function
    {
        public override string Name => "EQ";

        public override object Execute(Sheet sheetData, string formula)
        {
            SetCellReferences(formula);
            double? prevValue = null;

            foreach (var cellRef in CellReferences)
            {
                var cell = GetCell(sheetData, cellRef);
                if (cell is { Type: DataType.Number, Value: double value })
                {
                    if (prevValue != null)
                    {
                        var curValue = value;
                        if (prevValue != curValue)
                            return (bool)false;
                    }
                    prevValue = value;
                }
                else
                {
                    return "error";
                }
            }
            return (bool)true;
        }
    }

    public class NotFunction : Function
    {
        public override string Name => "NOT";

        public override object Execute(Sheet sheetData, string formula)
        {
            SetCellReferences(formula);
            bool result = true;

            foreach (var cellRef in CellReferences)
            {
                var cell = GetCell(sheetData, cellRef);
                if (cell is { Type: DataType.Boolean, Value: bool value })
                {
                    result &= !value;
                }
                else
                {
                    return "error";
                }
            }
            return (bool)result;
        }
    }

    public class AndFunction : Function
    {
        public override string Name => "AND";

        public override object Execute(Sheet sheetData, string formula)
        {
            SetCellReferences(formula);
            List<bool> boolList = new List<bool>();
            foreach (var cellRef in CellReferences)
            {
                var cell = GetCell(sheetData, cellRef);
                if (cell is { Type: DataType.Boolean, Value: bool value })
                {
                    boolList.Add(value);
                }
                else
                {
                    return "error";
                }
            }

            return boolList.All(b => b == true);

        }
    }

    public class OrFunction : Function
    {
        public override string Name => "OR";

        public override object Execute(Sheet sheetData, string formula)
        {

            SetCellReferences(formula);
            List<bool> boolList = new List<bool>();

            foreach (var cellRef in CellReferences)
            {
                var cell = GetCell(sheetData, cellRef);
                if (cell is { Type: DataType.Boolean, Value: bool value })
                {
                    boolList.Add(value);
                }
                else
                {
                    return "error";
                }
            }
            return boolList.Any(b => b == true);
        }
    }

    public class IfFunction : Function
    {
        public override string Name => "IF";

        public override object Execute(Sheet sheetData, string formula)
        {
            SetCellReferences(formula);
            bool result = true;
            List<Function> functions = InitFunctions.BoolFunctionsInit();
            var conditionString = CellReferences[0] + "," + CellReferences[1] + ")";

            foreach (var function in functions)
            {
                if (function.Contains(conditionString))
                {
                    result = (bool)function.Execute(sheetData, conditionString);
                    break;
                }
                else
                {
                    return "error";
                }
            }

            if (result)
                return GetCell(sheetData, CellReferences[2]).Value;
            return GetCell(sheetData, CellReferences[3]).Value;
        }
    }

    public class ConcatFunction : Function
    {
        public override string Name => "CONCAT";
        
        public override List<string> SplitFromula(string formula)
        {
            List<string> cell = new();
            int startIndex = formula.IndexOf('(') + 1;
            int endIndex = formula.LastIndexOf(')');
            int argStart = startIndex;
            bool inQuotes = false;
            for (int i = startIndex; i < endIndex; i++)
            {
                char c = formula[i];
                if (c == '\"')
                {
                    inQuotes = !inQuotes;
                }
                else if (c == ',' && !inQuotes)
                {
                    cell.Add(formula.Substring(argStart, i - argStart).Trim().Trim('"'));
                    argStart = i + 1;
                }
            }
            cell.Add(formula.Substring(argStart, endIndex - argStart).Trim().Trim('"'));
            return  cell;
        }
        public override object Execute(Sheet sheetData, string formula)
        {
            SetCellReferences(formula);
            var stringBuilder = new StringBuilder();

            foreach (var t in CellReferences)
            {
                string argumentValue;
                try
                {
                    argumentValue = (string)GetCell(sheetData, t).Value;
                }
                catch
                {
                    argumentValue = t;
                }
                stringBuilder.Append(argumentValue);
            }

            return stringBuilder.ToString();
        }
    }

    public class LtFunction : Function
    {
        public override string Name => "LT";

        public override object Execute(Sheet sheetData, string formula)
        {
            SetCellReferences(formula);
            double? prevValue = null;

            foreach (var cellRef in CellReferences)
            {
                var cell = GetCell(sheetData, cellRef);
                if (cell != null && cell.Type == DataType.Number && cell.Value is long value)
                {
                    if (prevValue != null)
                    {
                        var curValue = value;
                        if (prevValue < curValue)
                            return (bool)true;
                    }
                    prevValue = value;
                }
                else
                {
                    return "error";
                }
            }

            return (bool)false;
        }
    }
}

