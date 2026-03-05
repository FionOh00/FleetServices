using FleetServicesWebAPI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data.Odbc;
using System.Diagnostics.Eventing.Reader;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Xml.Linq;
using XJCALLS;
using static FleetServices.Controllers.FleetServicesController;


namespace FleetServices.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class FleetServicesController : ControllerBase
    {

        //private readonly string _connectionString = "Driver={IBM i Access ODBC Driver};System=192.168.100.100;Uid=fssdata;Pwd=Pinkfish22;DBQ=MVMSGS;";

        private readonly ILogger<FleetServicesController> _logger;
        //private const string ServerUrlBase = "https://wnt.aos.biz";
        //private const string ServerUrlBase1 = "/webdata/Login";
        //private const string ServerUrlBase2 = "/webdata/Logout";
        //private const string ServerUrlBase3 = "/webdata/WebFunction";
        //public static string aosTransStatus = "";
        //public static bool isTransSuccess = false;
        private static XJAPI xj;
        string stateId;



        public  FleetServicesController(ILogger<FleetServicesController> logger)
        {
            _logger = logger;
        }


        //[HttpGet("FleetServices")]
        //public IActionResult GetEDIData()
        //{

        //    var FSData = new List<Dictionary<string, object>>();

        //    using (var connection = new OdbcConnection(_connectionString))
        //    {
        //        connection.Open();

        //        string query = "SELECT  eoara,      eoodr# eoodr,     TO_CHAR(eoltid) eoltid,     eoprvordno,     eostat,     TO_CHAR(eodate) eodate, " +
        //                       "        eotime,     eocust,     TO_CHAR(eopdat) eopdat,     eoptim,     TO_CHAR(eoddat) eoddat, " +
        //                       "        eodtim,     eoinit,     eocons,     eoldat,         eoocty,     eoost,      eodcty||eodst eodest, " +
        //                       "        eoedcd,     TO_CHAR(eorsdt) eorsdt,     eorstm,     eouser,     eoagnt, " +
        //                       "        eocns# eocns,     eotrcd,     eodv# eodv,      CHAR(' ', 98) AS filler, " +
        //                       "        CASE eoinit WHEN 'upd' THEN 'U' WHEN 'cnt' THEN 'Z' WHEN 'can' THEN 'C' ELSE eostat END AS init_status, " +
        //                       "        COALESCE(custmast.cubctc, eoocty) AS OCity,     COALESCE(custmast.cubst,  eoost)  AS OSt, " +
        //                       "        COALESCE(custmasi.cubctc, eodcty) AS DCity,     COALESCE(custmasi.cubst,  eodst)  AS DSt " +
        //                       "FROM    iesproof.edordp  edordp LEFT JOIN iesproof.custmast custmast ON custmast.cucode = edordp.eocons " +
        //                       "LEFT JOIN iesproof.custmasi custmasi ON custmasi.cucode = edordp.eoldat " +
        //                       "limit  100";

        //        var data = new Dictionary<string, object>();

        //        using (var command = new OdbcCommand(query, connection))
        //        {
        //            using (var reader = command.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    data = new Dictionary<string, object>();
        //                    for (int i = 0; i < reader.FieldCount; i++)
        //                    {
        //                        data[reader.GetName(i)] = reader.IsDBNull(i) ? null : reader.GetValue(i);
        //                    }
        //                    FSData.Add(data);
        //                }
        //            }
        //        }
        //    }
        //    return Ok(FSData);

        //}


        //================================================================

        //[HttpGet("GetOpts")]
        [HttpPost("GetOpts")]
        //public static async Task RunDemoAsync()
        public async Task<Object> GetOpts([FromBody] dynamic data)
        {
            xj = new XJAPI();

            string s = await xj.Login("SEBASTIAN", "bluedonkey");

            Console.WriteLine("session: " + s);

            string stateId = await xj.GetStateId("XJ_UNSRV");

            Console.WriteLine("stId: " + stateId);

            var data1 = JsonConvert.DeserializeObject<dynamic>(data.ToString());
            var result = await GETOPTS(stateId, data1);

            var table = result.response.tables[0]; // mvDtls table
            var dtos = new List<OptDto>();

            foreach (var row in table.rows)
            {
                var dto = new OptDto();
                //foreach (var col in row.Cols)
                //for (int i = 0; i < row.cols.Count; i++)
                //{
                dto.Code = row.cols[0].data.Value;
                dto.Text = row.cols[1].data.Value;
                dto.Alw = row.cols[2].data.Value;
                    // Find header name matching the column nbr
                    //var header = table.Header.FirstOrDefault(h => h.Nbr == col.Nbr);
                    //if (header != null)
                    //{
                    //dto.Fields[header.Name] = col.Data;
                    //var tblName = table.header[i].name.ToString();
                    //if (tblName.Length > 4 && tblName.Substring(0, 4) == "dest")
                    //{
                    //    dto.Fields["dest"] = row.cols[i + 2].data.Value.PadRight(11) + row.cols[i + 1].data;  //shnm
                    //    dto.IProp["dest"] = row.cols[i].iProp.Value;
                    //    dto.Fields[tblName] = row.cols[i].data.Value;
                    //    dto.Fields[table.header[i + 1].name.ToString()] = row.cols[i + 1].data.Value;
                    //    dto.Fields[table.header[i + 2].name.ToString()] = row.cols[i + 2].data.Value;
                    //    i += 2;
                    //}
                    //else if (tblName.Length > 4 && tblName.Substring(0, 4) == "cont")
                    //{
                    //    dto.Fields["cont"] = row.cols[i + 2].data.Value.PadRight(11) + row.cols[i + 1].data;  //shnm
                    //    dto.IProp["cont"] = row.cols[i].iProp.Value;
                    //    dto.Fields[tblName] = row.cols[i].data.Value;
                    //    dto.Fields[table.header[i + 1].name.ToString()] = row.cols[i + 1].data.Value;
                    //    dto.Fields[table.header[i + 2].name.ToString()] = row.cols[i + 2].data.Value;
                    //    i += 2;
                    //}
                    //else if (tblName.Length > 2 && tblName.EndsWith("Dt"))
                    //{
                    //    dto.Fields[tblName] = (row.cols[i].data as string)?.Length > 5 ? row.cols[i].data.Value.Substring(0, 5) : row.cols[i].data.Value;
                    //    dto.IProp[tblName] = row.cols[i].iProp.Value;
                    //}
                    //else
                    //{
                    //    var tmp1 = row.cols[i].data;
                    //    dto.Fields[tblName] = row.cols[i].data.Value;
                    //    dto.IProp[tblName] = row.cols[i].iProp.Value;
                    //}
                    //}
                //}

                dtos.Add(dto);
            }


            string message = await xj.DisposeStateId("XJ_UNSRV", stateId);

            Console.WriteLine("message: " + message);

            string s2 = await xj.Logout();

            return dtos;

        }



        [HttpGet("FleetServices")]
        //public static async Task RunDemoAsync()
        public async Task<Object> GetFleetServices()
        {


            //var accessToken = await GetAccessTokenViaOwnerPasswordAsync();
            //Console.WriteLine(accessToken);

            //using var client = new HttpClient
            //{
            //    BaseAddress = new Uri(ServerUrlBase)
            //};

            //client.DefaultRequestHeaders.Add("aos-service-key", "aos-jct-2019");
            //client.DefaultRequestHeaders.Add("aos-session-id", accessToken);
            ////client.DefaultRequestHeaders.Add("aos-web-function", "XJ_MSGSN");
            //client.DefaultRequestHeaders.Add("aos-web-function", "XJ_UNSRV");
            //client.DefaultRequestHeaders.Add("aos-content-type", "JSON");
            //client.DefaultRequestHeaders.Add("Accept", "*/*");

            //var stId = await GetStIdAsync(client);
            //var result = await GetDTLS(client, stId);
            xj = new XJAPI();

            string s = await xj.Login("SEBASTIAN", "bluedonkey");

            Console.WriteLine("session: " + s);

            string stateId = await xj.GetStateId("XJ_UNSRV");

            Console.WriteLine("stId: " + stateId);
            var result = await GETUNITS(stateId);
            //var tmp = result.response.tables[0].rows;
            ////return result;
            //Console.WriteLine(result);

            //// Deserialize
            //var options = new JsonSerializerOptions
            //{
            //    PropertyNameCaseInsensitive = true
            //};

            //var root = JsonSerializer.Deserialize<Root>(result, options);

            //if (root?.Response == null || root.Response.Tables.Count == 0)
            var tmp = result.response.tables[0];
            if (result.response == null || result.response.tables[0] == null)
            {
                //Console.WriteLine("No tables found in JSON");
                return result;
            }

            var table = result.response.tables[0]; // mvDtls table
            var dtos = new List<FSDto>();

            foreach (var row in table.rows)
            {
                var dto = new FSDto();
                //foreach (var col in row.Cols)
                for (int i = 0; i < row.cols.Count; i++)
                {
                    // Find header name matching the column nbr
                    //var header = table.Header.FirstOrDefault(h => h.Nbr == col.Nbr);
                    //if (header != null)
                    //{
                    //dto.Fields[header.Name] = col.Data;
                    var tblName = table.header[i].name.ToString();
                    if (tblName.Length > 4 && tblName.Substring(0, 4) == "dest")
                    {
                        dto.Fields["dest"] = row.cols[i+2].data.Value.PadRight(11) + row.cols[i + 1].data;  //shnm
                        dto.IProp["dest"] = row.cols[i].iProp.Value;
                        dto.Fields[tblName] = row.cols[i].data.Value;
                        dto.Fields[table.header[i+1].name.ToString()] = row.cols[i+1].data.Value;
                        dto.Fields[table.header[i+2].name.ToString()] = row.cols[i+2].data.Value;
                        i += 2;
                    }
                    else if (tblName.Length > 4 && tblName.Substring(0, 4) == "cont")
                    {
                        dto.Fields["cont"] = row.cols[i + 2].data.Value.PadRight(11) + row.cols[i + 1].data;  //shnm
                        dto.IProp["cont"] = row.cols[i].iProp.Value;
                        dto.Fields[tblName] = row.cols[i].data.Value;
                        dto.Fields[table.header[i + 1].name.ToString()] = row.cols[i + 1].data.Value;
                        dto.Fields[table.header[i + 2].name.ToString()] = row.cols[i + 2].data.Value;
                        i += 2;
                    }
                    else if (tblName.Length > 2 && tblName.EndsWith("Dt"))
                    {
                        dto.Fields[tblName] = (row.cols[i].data as string)?.Length > 5? row.cols[i].data.Value.Substring(0, 5) : row.cols[i].data.Value;
                        dto.IProp[tblName] = row.cols[i].iProp.Value;
                    }
                    else
                    {
                        var tmp1 = row.cols[i].data;
                        dto.Fields[tblName] = row.cols[i].data.Value;
                        dto.IProp[tblName] = row.cols[i].iProp.Value;
                    }
                    //}
                }

                dtos.Add(dto);
            }


            string message = await xj.DisposeStateId("XJ_UNSRV", stateId);

            Console.WriteLine("message: " + message);

            string s2 = await xj.Logout();

            return dtos;
            // Example: print first 5 messages
            //foreach (var msg in dtos.Take(500))
            //{
            //    foreach (var kvp in msg.Fields)
            //    {
            //        Console.Write($"{kvp.Key}: {kvp.Value} | ");
            //    }
            //    Console.WriteLine();
            //}

            // Example: search messages with status "Received"
            //var received = dtos.Where(d => d.Fields.TryGetValue("status", out var s) && s == "Received").ToList();
            //Console.WriteLine($"\nTotal 'Received' messages: {received.Count}");

            //var exitResponse = await Exit(client, stId);
        }

        #region Get Token
        //private static async Task<string> GetAccessTokenViaOwnerPasswordAsync()
        //{
        //    using var client = new HttpClient
        //    {
        //        BaseAddress = new Uri(ServerUrlBase)
        //    };

        //    // API Key in header
        //    client.DefaultRequestHeaders.Add("aos-service-key", "aos-jct-2019");

        //    var form = new Dictionary<string, string>
        //    {
        //        ["username"] = "SEBASTIAN",
        //        ["password"] = "bluedonkey"
        //    };

        //    var content = new FormUrlEncodedContent(form);

        //    var response = await client.PostAsync(ServerUrlBase1, content);
        //    var body = await response.Content.ReadAsStringAsync();

        //    if (!response.IsSuccessStatusCode)
        //    {
        //        Console.WriteLine(
        //            $"HTTP {(int)response.StatusCode} {response.ReasonPhrase}\n{body}"
        //        );
        //    }

        //    return body;
        //}
        #endregion

        #region Exit
        //private static async Task<string> Exit(HttpClient client, string stId)
        //{
        //    string body = string.Empty;

        //    try
        //    {
        //        var requestBody = new
        //        {
        //            mvMsgs = new
        //            {
        //                proc = "EXIT",
        //                stId = stId
        //            }
        //        };

        //        // Serialize to JSON
        //        var secondjson = JsonSerializer.Serialize(requestBody);

        //        // Attach body with REAL Content-Type
        //        var content = new StringContent(
        //            secondjson,
        //            Encoding.UTF8,
        //            "application/json"
        //        );

        //        var resultafter = await client.PostAsync(ServerUrlBase3, content);
        //        body = resultafter.Content.ReadAsStringAsync().Result;
        //        Console.WriteLine(resultafter.IsSuccessStatusCode);
        //        Console.WriteLine(body);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }

        //    return body;
        //}
        #endregion

        //private static async Task<string> GetStIdAsync(HttpClient client)
        //{
        //    aosTransStatus = $"";
        //    isTransSuccess = true;
        //    string body = string.Empty;
        //    string stId = string.Empty;

        //    string SQLcmd = $"select * from [[custlib]].HIFLDCONTX where HCPROGRAM = 'XJ_UNSRV' order by hcfield ";
        //    HttpContent x = new StringContent(SQLcmd);

        //    try
        //    {
        //        var requestBody = new
        //        {
        //            unSrv = new
        //            {
        //                proc = "INIT",
        //                stId = ""
        //            }
        //        };

        //        var json = JsonSerializer.Serialize(requestBody);

        //        var content = new StringContent(
        //            json,
        //            Encoding.UTF8,
        //            "application/json"
        //        );

        //        var result = await client.PostAsync(ServerUrlBase3, content);
        //        body = result.Content.ReadAsStringAsync().Result;
        //        Console.WriteLine(result.IsSuccessStatusCode);

        //        var data = JsonSerializer.Deserialize<ApiResponse>(body);
        //        stId = data!.stId;
        //    }
        //    catch (Exception ex)
        //    {
        //        isTransSuccess = false;
        //        aosTransStatus = $"AS400 Call Error: {ex.Message}";
        //        Console.WriteLine(aosTransStatus);
        //    }

        //    return stId;
        //}

        private static async Task<dynamic> GETUNITS(string stId)
        {
            string body = string.Empty;

            //try
            //{
                var requestBody = new UnitRequest
                {
                    //units = new Units
                    //{
                        //proc = "GETDTLS",
                        proc = "GETUNITS",
                        //proc = "DOCS",
                        stId = stId,
                        //optsGdRows = "50",
                        //optsGdPage = "1",
                        unitsGdRows = "50",
                        unitsGdPage = "1",
                        unitsGdSortDi = "A",
                        unitsGdSortCo = "UNIT",
                        unitsGdNewData = "Y",

                        //"optsGdRows":"100","optsGdPage":"1"

                        //unHdr = new UnHdr
                        //{
                        //    title = "",
                        //    drvMgr1 = "",
                        //    drvMgr2 = "",
                        //    drvMgr3 = "",
                        //    drvMgr4 = "",
                        //    drvMgr5 = "",
                        //    drvMgr6 = "",
                        //    drvMgr7 = "",
                        //    drvMgr8 = "",
                        //    drvMgr9 = "",
                        //    drvMgr10 = "",
                        //    cust1 = "",
                        //    cust2 = "",
                        //    cust3 = "",
                        //    cust4 = "",
                        //    cust5 = "",
                        //    cust6 = "",
                        //    fltMgr1 = "*ALL",
                        //    unitDiv1 = "",
                        //    unitDiv2 = "",
                        //    unitDiv3 = "",
                        //    unit = "",
                        //    stat = "",
                        //    order = "",
                        //    disp = "",
                        //    cnd = "",
                        //    trlr = "",
                        //    destCtyC = "",
                        //    destSt = "",
                        //    destShnm = "",
                        //    etaDt = "",
                        //    etaTm = "",
                        //    ptaDt = "",
                        //    ptaTm = "",
                        //    contCtyC = "",
                        //    contSt = "",
                        //    contShnm = "",
                        //    drvr1 = "",
                        //    drHome1 = "",
                        //    dr7DyMl1 = "",
                        //    drRDO1 = "",
                        //    dr01hr1 = "",
                        //    dr02hr1 = "",
                        //    dr03hr1 = "",
                        //    //nxStpNbr = "",
                        //    //nxStpCtyC = "",
                        //    //nxStpSt = "",
                        //    //nxStpShnm = "",
                        //    //nxStpMiles = "",
                        //    //nxStpApDt1 = "",
                        //    //nxStpApTm1 = "",
                        //    //nxStpApDt2 = "",
                        //    //nxStpApTm2 = "",
                        //    //nxStpPrjDt = "",
                        //    //nxStpPrjTm = "",
                        //    //nxStpArvDt = "",
                        //    //nxStpArvTm = "",
                        //    //satCtyC = "",
                        //    //satSt = "",
                        //    //satShnm = "",
                        //    //satDt = "",
                        //    //satTm = "",
                        //    //usrDefn = "",
                        //}
                    //}
                };
                var data = await xj.WebFunction("XJ_UNSRV", requestBody);
                return data;
                //var json = JsonSerializer.Serialize(requestBody);

                //var content = new StringContent(
                //    json,
                //    Encoding.UTF8,
                //    "application/json"
                //);

                //var result = await client.PostAsync(ServerUrlBase3, content);
                //body = result.Content.ReadAsStringAsync().Result;
            //}
            //catch (Exception ex)
            //{
            //    //isTransSuccess = false;
            //    //aosTransStatus = $"AS400 Call Error: {ex.Message}";
            //    return null;
            //}
        }


        private static async Task<dynamic> GETOPTS(string stId, dynamic payload)
        {
            string body = string.Empty;

            //try
            //{
            //string jsonStr = JsonConvert.DeserializeObject<dynamic>(payload1);
            //JsonObject payload = JsonConvert.DeserializeObject<dynamic>(payload1);
            //dynamic payload = (dynamic)payload1;
            //dynamic payload = JsonConvert.DeserializeObject<dynamic>(payload1);
            //var payload = payload1.opt;
            // UnUnit ununit = (UnUnit)payload1;
            //string test = payload1.object.opt;
            var requestBody = new OptRequest
            {
                //units = new Units
                //{
                //proc = "GETDTLS",
                proc = "GETOPTS",
                //proc = "EDITOPT",
                //proc = "DOCS",
                stId = stId,
                //unUnit = payload,
                //unUnit = new UnUnit
                //{
                //    opt = "",
                //    unit = "",
                //    stat = "",
                //    order = "",
                //    cnd = "",
                //    trlr = "",
                //    destCtyC = "",
                //    destSt = "",
                //    destShnm = "",
                //    etaDt = "",
                //    etaTm = "",
                //    ptaDt = "",
                //    ptaTm = "",
                //    contCtyC = "",
                //    contSt = "",
                //    contShnm = "",
                //    drvr1 = "",
                //    drHome1 = "",
                //    dr7DyMl1 = "",
                //    drRDO1 = "",
                //    dr01hr1 = "",
                //    dr02hr1 = "",
                //    dr03hr1 = "",
                //    nxStpNbr = "",
                //    nxStpCtyC = "",
                //    nxStpSt = "",
                //    nxStpShnm = "",
                //    nxStpMiles = "",
                //    nxStpApDt1 = "",
                //    nxStpApTm1 = "",
                //    nxStpApDt2 = "",
                //    nxStpApTm2 = "",
                //    nxStpPrjDt = "",
                //    nxStpPrjTm = "",
                //    nxStpArvDt = "",
                //    nxStpArvTm = "",
                //    satCtyC = "",
                //    satSt = "",
                //    satShnm = "",
                //    satDt = "",
                //    satTm = "",
                //    plnUnit = "",
                //    plnOrd = "",
                //    plnOrdSt = "",
                //    plnDisp = "",
                //    plnLdNbr = "",
                //    plnDspNbr = "",
                //    plnNbrDsp = "",
                //    curOrd = "",
                //    curOrdSt = "",
                //    curDisp = "",
                //    curLdNbr = "",
                //    curDspNbr = "",
                //    curNbrDsp = "",
                //    disp = "",
                //    drvr2 = "",
                //    trlr1l = "",
                //    trlr2l = "",
                //    trlr3l = "",
                //    cndAry = "",
                //    seqAry = "",
                //    ordStat = "",
                //    unitLatt = "",
                //    unitLong = "",
                //    lcallDt = "",
                //    lcallTm = "",
                //    dr1div = "",
                //    untDiv = "",
                //    drvMgr = "",
                //    unitHrs = "",
                //    //
                //    //
                //    //"opt":"","unit":"","stat":"","order":"","cnd":"","trlr":"","destCtyC":"","destSt":"","destShnm":"",
                //    ////"etaDt":"","etaTm":"","ptaDt":"","ptaTm":"","contCtyC":"","contSt":"","contShnm":"","drvr1":"","drHome1":"",
                //    //"dr7DyMl1":"0","drRDO1":"0","dr01hr1":"","dr02hr1":"","dr03hr1":""
                //    //"nxStpNbr":"","nxStpCtyC":"","nxStpSt":"","nxStpShnm":"","nxStpMiles":"","nxStpApDt1":"","nxStpApTm1":"","nxStpApDt2":"",
                //    //"nxStpApTm2":"","nxStpPrjDt":"","nxStpPrjTm":"","nxStpArvDt":"","nxStpArvTm":"","satCtyC":"","satSt":"","satShnm":"",
                //    //"satDt":"","satTm":"","plnUnit":"","plnOrd":"","plnOrdSt":"","plnDisp":"","plnLdNbr":"","plnDspNbr":"","plnNbrDsp":"",
                //    //"curOrd":"","curOrdSt":"","curDisp":"","curLdNbr":"","curDspNbr":"","curNbrDsp":"","disp":"","drvr2":"","trlr1l":"",
                //    //"trlr2l":"","trlr3l":"","cndAry":"","seqAry":"","":"","unitLatt":"0","unitLong":"0","lcallDt":"","lcallTm":"",
                //    //"dr1div":"","untDiv":"","drvMgr":"","unitHrs":""
                //},
                unUnit = new UnUnit
                {
                    opt = payload.opt,
                    unit = payload.unit,
                    stat = payload.stat,
                    order = payload.order,
                    cnd = payload.cnd,
                    trlr = payload.trlr,
                    destCtyC = payload.destCtyC,
                    destSt = payload.destSt,
                    destShnm = payload.destShnm,
                    etaDt = payload.etaDt,
                    etaTm = payload.etaTm,
                    ptaDt = payload.ptaDt,
                    ptaTm = payload.ptaTm,
                    contCtyC = payload.contCtyC,
                    contSt = payload.contSt,
                    contShnm = payload.contShnm,
                    drvr1 = payload.drvr1,
                    drHome1 = payload.drHome1,
                    dr7DyMl1 = payload.dr7DyMl1,
                    drRDO1 = payload.drRDO1,
                    dr01hr1 = payload.dr01hr1,
                    dr02hr1 = payload.dr02hr1,
                    dr03hr1 = payload.dr03hr1,
                    nxStpNbr = payload.nxStpNbr,
                    nxStpCtyC = payload.nxStpCtyC,
                    nxStpSt = payload.nxStpSt,
                    nxStpShnm = payload.nxStpShnm,
                    nxStpMiles = payload.nxStpMiles,
                    nxStpApDt1 = payload.nxStpApDt1,
                    nxStpApTm1 = payload.nxStpApTm1,
                    nxStpApDt2 = payload.nxStpApDt2,
                    nxStpApTm2 = payload.nxStpApTm2,
                    nxStpPrjDt = payload.nxStpPrjDt,
                    nxStpPrjTm = payload.nxStpPrjTm,
                    nxStpArvDt = payload.nxStpArvDt,
                    nxStpArvTm = payload.nxStpArvTm,
                    satCtyC = payload.satCtyC,
                    satSt = payload.satSt,
                    satShnm = payload.satShnm,
                    satDt = payload.satDt,
                    satTm = payload.satTm,
                    plnUnit = payload.plnUnit,
                    plnOrd = payload.plnOrd,
                    plnOrdSt = payload.plnOrdSt,
                    plnDisp = payload.plnDisp,
                    plnLdNbr = payload.plnLdNbr,
                    plnDspNbr = payload.plnDspNbr,
                    plnNbrDsp = payload.plnNbrDsp,
                    curOrd = payload.curOrd,
                    curOrdSt = payload.curOrdSt,
                    curDisp = payload.curDisp,
                    curLdNbr = payload.curLdNbr,
                    curDspNbr = payload.curDspNbr,
                    curNbrDsp = payload.curNbrDsp,
                    disp = payload.disp,
                    drvr2 = payload.drvr2,
                    trlr1l = payload.trlr1l,
                    trlr2l = payload.trlr2l,
                    trlr3l = payload.trlr3l,
                    cndAry = payload.cndAry,
                    seqAry = payload.seqAry,
                    ordStat = payload.ordStat,
                    unitLatt = payload.unitLatt,
                    unitLong = payload.unitLong,
                    lcallDt = payload.lcallDt,
                    lcallTm = payload.lcallTm,
                    dr1div = payload.dr1div,
                    untDiv = payload.untDiv,
                    drvMgr = payload.drvMgr,
                    unitHrs = payload.unitHrs,
                },
                optsGdRows = "100",
                optsGdPage = "1",
                //unitsGdRows = "50",
                //unitsGdPage = "1",
                //unitsGdSortDi = "A",
                //unitsGdSortCo = "UNIT",
                //unitsGdNewData = "Y",
            };
            var result = await xj.WebFunction("XJ_UNSRV", requestBody);
            //var data = JsonConvert.SerializeObject(result);
            return result;
        }

        public class ApiResponse
        {
            public required string function { get; set; }
            public required string proc { get; set; }
            public required string stId { get; set; }
        }

        public class UnitRequest
        {
            //public Units units { get; set; }
            public string proc { get; set; }
            public string stId { get; set; }
            //public UnHdr unHdr { get; set; }
            public string unitsGdRows { get; set; }
            public string unitsGdPage { get; set; }
            public string unitsGdSortDi { get; set; }
            public string unitsGdSortCo { get; set; }
            public string unitsGdNewData { get; set; }
        }

        public class OptRequest
        {
            public string proc { get; set; }
            public string stId { get; set; }
            
            public UnUnit unUnit { get; set; }
            public string optsGdRows { get; set; }
            public string optsGdPage { get; set; }
        }


        public class UnSrvs
        {
            public string proc { get; set; }
            public string stId { get; set; }

            public string body { get; set; }
        }

        public class Units
        {
            public string proc { get; set; }
            public string stId { get; set; }
            //public UnHdr unHdr { get; set; }
            public string unitsGdRows { get; set; }
            public string unitsGdPage { get; set; }
            public string unitsGdSortDi { get; set; }
            public string unitsGdSortCo { get; set; }
            public string unitsGdNewData { get; set; }

        }

        public class UnHdr
        {
            public string title { get; set; }
            public string drvMgr1 { get; set; }
            public string drvMgr2 { get; set; }
            public string drvMgr3 { get; set; }
            public string drvMgr4 { get; set; }
            public string drvMgr5 { get; set; }
            public string drvMgr6 { get; set; }
            public string drvMgr7 { get; set; }
            public string drvMgr8 { get; set; }
            public string drvMgr9 { get; set; }
            public string drvMgr10 { get; set; }
            public string cust1 { get; set; }
            public string cust2 { get; set; }
            public string cust3 { get; set; }
            public string cust4 { get; set; }
            public string cust5 { get; set; }
            public string cust6 { get; set; }
            public string fltMgr1 { get; set; }
            public string unitDiv1 { get; set; }
            public string unitDiv2 { get; set; }
            public string unitDiv3 { get; set; }
            public string unit { get; set; }
            public string stat { get; set; }
            public string order { get; set; }
            public string disp { get; set; }
            public string cnd { get; set; }
            public string trlr { get; set; }
            public string destCtyC { get; set; }
            public string destSt { get; set; }
            public string destShnm { get; set; }
            public string etaDt { get; set; }
            public string etaTm { get; set; }
            public string ptaDt { get; set; }
            public string ptaTm { get; set; }
            public string contCtyC { get; set; }
            public string contSt { get; set; }
            public string contShnm { get; set; }
            public string drvr1 { get; set; }
            public string drHome1 { get; set; }
            public string dr7DyMl1 { get; set; }
            public string drRDO1 { get; set; }
            public string dr01hr1 { get; set; }
            public string dr02hr1 { get; set; }
            public string dr03hr1 { get; set; }
            public string nxStpNbr { get; set; }
            public string nxStpCtyC { get; set; }
            public string nxStpSt { get; set; }
            public string nxStpShnm { get; set; }
            public string nxStpMiles { get; set; }
            public string nxStpApDt1 { get; set; }
            public string nxStpApTm1 { get; set; }
            public string nxStpApDt2 { get; set; }
            public string nxStpApTm2 { get; set; }
            public string nxStpPrjDt { get; set; }
            public string nxStpPrjTm { get; set; }
            public string nxStpArvDt { get; set; }
            public string nxStpArvTm { get; set; }
            public string satCtyC { get; set; }
            public string satSt { get; set; }
            public string satShnm { get; set; }
            public string satDt { get; set; }
            public string satTm { get; set; }
            public string usrDefn { get; set; }

        }


        public class UnUnit
        {
            public string opt { get; set; }
            public string unit { get; set; }
            public string stat { get; set; }
            public string order { get; set; }
            public string cnd { get; set; }
            public string trlr { get; set; }
            public string destCtyC { get; set; }
            public string destSt { get; set; }
            public string destShnm { get; set; }
            public string etaDt { get; set; }
            public string etaTm { get; set; }
            public string ptaDt { get; set; }
            public string ptaTm { get; set; }
            public string contCtyC { get; set; }
            public string contSt { get; set; }
            public string contShnm { get; set; }
            public string drvr1 { get; set; }
            public string drHome1 { get; set; }
            public string dr7DyMl1 { get; set; }
            public string drRDO1 { get; set; }
            public string dr01hr1 { get; set; }
            public string dr02hr1 { get; set; }
            public string dr03hr1 { get; set; }
            public string nxStpNbr { get; set; }
            public string nxStpCtyC { get; set; }
            public string nxStpSt { get; set; }
            public string nxStpShnm { get; set; }
            public string nxStpMiles { get; set; }
            public string nxStpApDt1 { get; set; }
            public string nxStpApTm1 { get; set; }
            public string nxStpApDt2 { get; set; }
            public string nxStpApTm2 { get; set; }
            public string nxStpPrjDt { get; set; }
            public string nxStpPrjTm { get; set; }
            public string nxStpArvDt { get; set; }
            public string nxStpArvTm { get; set; }
            public string satCtyC { get; set; }
            public string satSt { get; set; }
            public string satShnm { get; set; }
            public string satDt { get; set; }
            public string satTm { get; set; }
            public string plnUnit { get; set; }
            public string plnOrd { get; set; }
            public string plnOrdSt { get; set; }
            public string plnDisp { get; set; }
            public string plnLdNbr { get; set; }
            public string plnDspNbr { get; set; }
            public string plnNbrDsp { get; set; }
            public string curOrd { get; set; }
            public string curOrdSt { get; set; }
            public string curDisp { get; set; }
            public string curLdNbr { get; set; }
            public string curDspNbr { get; set; }
            public string curNbrDsp { get; set; }
            public string disp { get; set; }
            public string drvr2 { get; set; }
            public string trlr1l { get; set; }
            public string trlr2l { get; set; }
            public string trlr3l { get; set; }
            public string cndAry { get; set; }
            public string seqAry { get; set; }
            public string ordStat { get; set; }
            public string unitLatt { get; set; }
            public string unitLong { get; set; }
            public string lcallDt { get; set; }
            public string lcallTm { get; set; }
            public string dr1div { get; set; }
            public string untDiv { get; set; }
            public string drvMgr { get; set; }
            public string unitHrs { get; set; }

            //"opt":"","unit":"","stat":"","order":"","cnd":"","trlr":"","destCtyC":"","destSt":"","destShnm":"",
            ////"etaDt":"","etaTm":"","ptaDt":"","ptaTm":"","contCtyC":"","contSt":"","contShnm":"","drvr1":"","drHome1":"",
            //"dr7DyMl1":"0","drRDO1":"0","dr01hr1":"","dr02hr1":"","dr03hr1":"","nxStpNbr":"","nxStpCtyC":"","nxStpSt":"","nxStpShnm":"",
            ////"nxStpMiles":"","nxStpApDt1":"","nxStpApTm1":"","nxStpApDt2":"","nxStpApTm2":"","nxStpPrjDt":"","nxStpPrjTm":"",
            //"nxStpArvDt":"","nxStpArvTm":"","satCtyC":"","satSt":"","satShnm":"","satDt":"","satTm":"","plnUnit":"","plnOrd":"",
            ////"plnOrdSt":"","plnDisp":"","plnLdNbr":"","plnDspNbr":"","plnNbrDsp":"","curOrd":"","curOrdSt":"","curDisp":"","curLdNbr":"",
            //"curDspNbr":"","curNbrDsp":"","disp":"","drvr2":"","trlr1l":"","trlr2l":"","trlr3l":"","cndAry":"","seqAry":"","ordStat":"",
            //"unitLatt":"0","unitLong":"0","lcallDt":"","lcallTm":"","dr1div":"","untDiv":"","drvMgr":"","unitHrs":""
        }

        public class UnDisp
        {
            public string title { get; set; }
            public string drvMgr1 { get; set; }
            public string drvMgr2 { get; set; }
            public string drvMgr3 { get; set; }
            public string drvMgr4 { get; set; }
            public string drvMgr5 { get; set; }
            public string drvMgr6 { get; set; }
            public string drvMgr7 { get; set; }
            public string drvMgr8 { get; set; }
            public string drvMgr9 { get; set; }
            public string drvMgr10 { get; set; }
            public string cust1 { get; set; }
            public string cust2 { get; set; }
            public string cust3 { get; set; }
            public string cust4 { get; set; }
            public string cust5 { get; set; }
            public string cust6 { get; set; }
            public string fltMgr1 { get; set; }
            public string unitDiv1 { get; set; }
            public string unitDiv2 { get; set; }
            public string unitDiv3 { get; set; }
            public string unit { get; set; }
            public string stat { get; set; }
            public string order { get; set; }
            public string disp { get; set; }
            public string cnd { get; set; }
            public string trlr { get; set; }
            public string destCtyC { get; set; }
            public string destSt { get; set; }
            public string destShnm { get; set; }
            public string etaDt { get; set; }
            public string etaTm { get; set; }
            public string ptaDt { get; set; }
            public string ptaTm { get; set; }
            public string contCtyC { get; set; }
            public string contSt { get; set; }
            public string contShnm { get; set; }
            public string drvr1 { get; set; }
            public string drHome1 { get; set; }
            public string dr7DyMl1 { get; set; }
            public string drRDO1 { get; set; }
            public string dr01hr1 { get; set; }
            public string dr02hr1 { get; set; }
            public string dr03hr1 { get; set; }
            public string nxStpNbr { get; set; }
            public string nxStpCtyC { get; set; }
            public string nxStpSt { get; set; }
            public string nxStpShnm { get; set; }
            public string nxStpMiles { get; set; }
            public string nxStpApDt1 { get; set; }
            public string nxStpApTm1 { get; set; }
            public string nxStpApDt2 { get; set; }
            public string nxStpApTm2 { get; set; }
            public string nxStpPrjDt { get; set; }
            public string nxStpPrjTm { get; set; }
            public string nxStpArvDt { get; set; }
            public string nxStpArvTm { get; set; }
            public string satCtyC { get; set; }
            public string satSt { get; set; }
            public string satShnm { get; set; }
            public string satDt { get; set; }
            public string satTm { get; set; }
            public string usrDefn { get; set; }
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


        public class FSDto
        {
            public Dictionary<string, string> Fields { get; set; } = new();
            public Dictionary<string, string> IProp { get; set; } = new();
        }

        public class OptDto
        {
            public string Code { get; set; }
            public string Text { get; set; }
            public string Alw { get; set; }
        }
    }
}
