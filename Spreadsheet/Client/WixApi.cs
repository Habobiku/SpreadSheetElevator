using System.Text;
using Newtonsoft.Json;
using Spreadsheet.Models;

namespace Spreadsheet.Client
{
	public class WixApi
	{
		private static string? _url;
        readonly HttpClient _client;

        public WixApi()
        {
            _url = Config._url;
            _client = new HttpClient();
        }

        public async Task<SpreadsheetResponse?> GetSheets()
		{
            var response = await _client.GetAsync(_url+$"sheets?tag=ifcells");
            var settings = new JsonSerializerSettings
            {
                Converters = new List<JsonConverter> { new CellConverter() },
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented
            };
            response.EnsureSuccessStatusCode();
            var content = response.Content.ReadAsStringAsync().Result;
            if (content == "[]")
            {
                return null;
            }
            var result = JsonConvert.DeserializeObject<SpreadsheetResponse>(content,settings);
            return result;
        }

        public async Task<Task<string>> PostSheets(string url, SpreadsheetPost sheets )
        {
            var settings = new JsonSerializerSettings
            {
                Converters = new List<JsonConverter> { new CellPostConverter() },
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented
            };
            var json = JsonConvert.SerializeObject(sheets, settings);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var post = await _client.PostAsync(url, data);
            var postcontent = post.Content.ReadAsStringAsync();
            Console.WriteLine(postcontent.Result);
            return postcontent;
        }
    }
}

