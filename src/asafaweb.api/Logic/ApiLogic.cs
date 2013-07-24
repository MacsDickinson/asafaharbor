using System.Net;
using Newtonsoft.Json;
using asafaweb.api.Exceptions;
using asafaweb.api.Models;

namespace asafaweb.api.Logic
{
    public class ApiLogic
    {
        private readonly string _username;
        private readonly string _key;
        private readonly string _urlToScan;
        private const string ApiUrlFormat = "https://asafaweb.com/Api/Scanner?url={0}&username={1}&apikey={2}";

        private string ScanUrl
        {
            get { return string.Format(ApiUrlFormat, _urlToScan, _username, _key); }
        }

        public ApiLogic(string username,string key, string urlToScan)
        {
            _username = username;
            _key = key;
            _urlToScan = urlToScan;
        }

        public ApiScanResult Scan()
        {
            try
            {
                string jsonString = GetJsonResponse(ScanUrl);
                return ApiScanResult(jsonString);
            }
            catch (WebException ex)
            {
                if (ex.Message == "The remote server returned an error: (401) Unauthorized.")
                {
                    throw new ApiException("Unauthorized: Ensure you have specified your correct API username and key.");
                }
                throw;
            }
        }

        public static ApiScanResult ApiScanResult(string jsonString)
        {
            return JsonConvert.DeserializeObject<ApiScanResult>(jsonString);
        }

        public string GetJsonResponse(string url)
        {
            WebClient webclient = new WebClient();
            return webclient.DownloadString(url);
        }
    }
}