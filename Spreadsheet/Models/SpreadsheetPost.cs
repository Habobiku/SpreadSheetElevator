using Newtonsoft.Json;

namespace Spreadsheet.Models
{
    public class CellPost
    {
        [JsonProperty("value")]
        public object? Value { get; set; }
    }

    public class SheetPost
    {
        [JsonProperty("id")]
        public string? Id { get; set; }
        [JsonProperty("data")]
        public List<List<CellPost>>? Data { get; set; }
    }

    public class SpreadsheetPost
    {
        [JsonProperty("email")]
        public string? Email { get; set; }
        [JsonProperty("results")]
        public List<SheetPost>? Results { get; set; }
    }

}

