using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace Spreadsheet.Models
{
    public class CellConverter : JsonConverter<Cell>
    {
        public override bool CanRead => true;
        public override bool CanWrite => false;

        public override Cell ReadJson(JsonReader reader, Type objectType, Cell? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var value = serializer.Deserialize(reader);
            if (value == null)
            {
                return new Cell { Type = DataType.Error, Value = "#NULL!" };
            }

            var type = value switch
            {
                bool boolValue => DataType.Boolean,
                long longValue => DataType.Number,
                double doubleValue => DataType.Number,
                string stringValue => stringValue.StartsWith("=") ? DataType.Formula : int.TryParse(stringValue, out _) ? DataType.Number : DataType.String,
                _ => DataType.Error,
            };
           
            string pattern = @"sheets\[(\d+)\]\.data\[(\d+)\]\[(\d+)\]";

            Match match = Regex.Match(reader.Path, pattern);
            int row = 0;
            int colInt = 0;

            if (match.Success)
            {
                row = int.Parse(match.Groups[2].Value);
                colInt = int.Parse(match.Groups[3].Value);
            }

            row++;
            colInt++;
            var rowString = row.ToString();
            char col = (char)('A' + (colInt - 1) % 26);

            switch (type)
            {
                case DataType.Number:
                    try
                    {
                        return new Cell { Type = type, Value = (double)value, Name = col + rowString };
                    }
                    catch
                    {
                        return new Cell { Type = type, Value = (long)value, Name = col + rowString };

                    }
                case DataType.String:
                    return new Cell { Type = type, Value = (string)value, Name = col + rowString };
                case DataType.Boolean:
                    return new Cell { Type = type, Value = (bool)value, Name = col + rowString };
            }


            return new Cell { Type = type, Value = value, Name = col+rowString };
        }
        

        public override void WriteJson(JsonWriter writer, Cell? value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}

