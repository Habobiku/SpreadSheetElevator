using Newtonsoft.Json;

namespace Spreadsheet.Models
{
    public enum DataType
    {
        Number,
        String,
        Formula,
        Boolean,
        Error
    }

    [JsonConverter(typeof(CellConverter))]
    public class Cell
    {
        public DataType Type { get; set; }
        public object? Value { get; set; }
        public string? Name { get; set; }
    }

    public class Sheet
    {
        [JsonProperty("id")]
        public string? Id { get; set; }
        [JsonProperty("data")]
        public List<List<Cell>>? Data { get; set; }
        //[JsonProperty("tag")]
        //public string Tag { get; set; }
    }

    public class SpreadsheetResponse
    {
        [JsonProperty("submissionUrl")]
        public string? SubmissionUrl { get; set; }
        [JsonProperty("sheets")]
        public List<Sheet>? Sheets { get; set; }
    }

}

