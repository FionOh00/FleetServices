using System.Text.Json.Serialization;

namespace FleetServicesWebAPI
{
    public class FleetService
    {
        //public DateOnly Date { get; set; }

        //public int TemperatureC { get; set; }

        //public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        //public string? Summary { get; set; }
        public string EOARA {  get; set; }
        public string EOODR { get; set; }
        public int EOLTID { get; set; }
        public string EOPRVORDNO { get; set; }
        public string EOSTAT { get; set; }
        public string EODATE { get; set; }
        public string EOTIME { get; set; }
        public string EOCUST { get; set; }
        public string EOPDAT { get; set; }
        public string EOPTIM { get; set; }
        public string EODDAT { get; set; }
        public string EODTIM { get; set; }
        public string EOINIT { get; set; }
        public string EOCONS { get; set; }
        public string EOLDAT { get; set; }
        public string EOOCTY { get; set; }
        public string EOOST { get; set; }
        public string EODCTY { get; set; }
        public string EODST { get; set; }
        public string EOEDCD { get; set; }
        public int EORSDT { get; set; }
        public string EORSTM { get; set; }
        public string EOUSER { get; set; }
        public string EOAGNT { get; set; }
        public string EOCNS { get; set; }
        public string EOTRCD { get; set; }
        public string EODV { get; set; }
        public string FILLER { get; set; }
        public string INIT_STATUS { get; set; }
        public string OCITY { get; set; }
        public string OST { get; set; }

        public string DCITY { get; set; }
        public string DST { get; set; }

    }

    public class ApiResponse
    {
        public required string function { get; set; }
        public required string proc { get; set; }
        public required string stId { get; set; }

    }

    // Root JSON
    public class Root
    {
        public string Function { get; set; } = string.Empty;
        public string Proc { get; set; } = string.Empty;
        public string StId { get; set; } = string.Empty;
        public Response? Response { get; set; }
    }

    public class Response
    {
        [JsonPropertyName("header")]
        public List<ResponseHeader> Headers { get; set; } = new();

        [JsonPropertyName("tables")]
        public List<Table> Tables { get; set; } = new();
    }

    public class ResponseHeader
    {
        [JsonPropertyName("nbr")]
        public string Nbr { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("data")]
        public string Data { get; set; } = string.Empty;

        [JsonPropertyName("iProp")]
        public string IProp { get; set; } = string.Empty;
    }

    public class Table
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("header")]
        public List<TableHeader> Header { get; set; } = new();

        [JsonPropertyName("rows")]
        public List<TableRow> Rows { get; set; } = new();
    }

    public class TableHeader
    {
        [JsonPropertyName("nbr")]
        public string Nbr { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }

    public class TableRow
    {
        [JsonPropertyName("rowId")]
        public string RowId { get; set; } = string.Empty;

        [JsonPropertyName("cols")]
        public List<TableCol> Cols { get; set; } = new();
    }

    public class TableCol
    {
        [JsonPropertyName("nbr")]
        public string Nbr { get; set; } = string.Empty;

        [JsonPropertyName("data")]
        public string Data { get; set; } = string.Empty;

        [JsonPropertyName("iProp")]
        public string IProp { get; set; } = string.Empty;
    }

    // Clean DTO for frontend / business
    public class MessageDto
    {
        public Dictionary<string, string> Fields { get; set; } = new();
    }
}
