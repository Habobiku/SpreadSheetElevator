using Spreadsheet.Models;

namespace Spreadsheet.Extension
{
	public static class MapSheetToPost
	{
        public static SpreadsheetPost MapSpreadsheetResponseToPost(SpreadsheetResponse response, string email)
        {
            SpreadsheetPost spreadsheetPost = new SpreadsheetPost
            {
                Email = email,
                Results = new List<SheetPost>()
            };

            foreach (Sheet sheet in response.Sheets)
            {
                SheetPost sheetPost = new SheetPost
                {
                    Id = sheet.Id,
                    Data = new List<List<CellPost>>()
                };
                foreach (List<Cell> row in sheet.Data)
                {

                    List<CellPost> rowPost = new List<CellPost>();

                    foreach (Cell cell in row)
                    {
                        CellPost cellPost = new CellPost
                        {
                            Value = cell.Value
                        };

                        rowPost.Add(cellPost);
                    }

                    sheetPost.Data.Add(rowPost);
                }

                spreadsheetPost.Results.Add(sheetPost);
            }
            return spreadsheetPost;
        }
    }
}

