using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace XJCALLS
{
    public class XJAPI
    {
        private string BaseURL = "https://wnt.aos.biz";
        private string LoginURL = "/webdata/Login";
        private string LogoutURL = "/webdata/Logout";
        private string WebFunctionURL = "/webdata/WebFunction";
        private string ServiceKey = "aos-jct-2019";

        private string SessionID = "";
        private string StateID = "";

        public XJAPI(string baseURL = "", string loginURL = "", string logoutURL = "", string webFunctionURL = "", string serviceKey = "")
        {            
            this.BaseURL = (baseURL == "") ? this.BaseURL : baseURL;
            this.LoginURL = (loginURL == "") ? this.LoginURL : loginURL;
            this.LogoutURL = (logoutURL == "") ? this.LogoutURL : logoutURL;
            this.WebFunctionURL = (webFunctionURL == "") ? this.WebFunctionURL : webFunctionURL;
            this.ServiceKey = (serviceKey == "") ? this.ServiceKey : serviceKey;
            this.SessionID = "";
            this.StateID = "";
        }

        public async Task<string> Login(string username, string password)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("aos-service-key", this.ServiceKey);

            var queryParams = new Dictionary<string, string>   {
                { "username", username },
                { "password", password }
            };

            var url = QueryHelpers.AddQueryString(this.BaseURL + this.LoginURL, queryParams!);

            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            this.SessionID = await response.Content.ReadAsStringAsync();

            client.Dispose();

            return this.SessionID;
        }

        public async Task<string> GetStateId(string function)
        {
            var payload = new
            {
                proc = "INIT",
                stId = ""
            };

            var result = await WebFunction(function, payload);
            string tmp = result?.stId;
            return result!.stId;
        }

        public async Task<string> DisposeStateId(string function, string stateId)
        {
            var payload = new
            {
                proc = "EXIT",
                stId = stateId,
            };

            var result = await WebFunction(function, payload);

            return result!.mvMsgs;
        }

        public async Task<dynamic> WebFunction(string function, dynamic payload)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("aos-service-key", this.ServiceKey);
            client.DefaultRequestHeaders.Add("aos-session-id", this.SessionID);
            client.DefaultRequestHeaders.Add("aos-web-function", function);
            client.DefaultRequestHeaders.Add("aos-content-type", "JSON");
            client.DefaultRequestHeaders.Add("Accept", "*/*");

            string json = JsonConvert.SerializeObject(payload);

            var url = this.BaseURL + this.WebFunctionURL;

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();

            var data = JsonConvert.DeserializeObject<dynamic>(result);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var tmp = System.Text.Json.JsonSerializer.Deserialize<JsonObject>(result, options);

            client.Dispose();

            return data!;
            //return result;
        }

        public async Task<string> Logout()
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("aos-service-key", this.ServiceKey);
            client.DefaultRequestHeaders.Add("aos-session-id", this.SessionID);

            var url = this.BaseURL + this.LogoutURL;

            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();

            client.Dispose();

            this.SessionID = "";
            this.StateID = "";

            return this.SessionID;
        }
    }
}
