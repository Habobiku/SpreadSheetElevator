using Newtonsoft.Json;

namespace Spreadsheet.Models
{
    public class CellPostConverter : JsonConverter<CellPost>
    {
        public override bool CanRead => false;
        public override bool CanWrite => true;

        public override CellPost ReadJson(JsonReader reader, Type objectType, CellPost? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, CellPost? value, JsonSerializer serializer)
        {
            writer.WriteValue(value.Value);
        }
    }
}

