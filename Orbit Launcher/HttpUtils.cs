using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Windows.Forms;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using OrbitLauncher.Authentication;

namespace OrbitLauncher
{
    public static class StatusCode
    {
        public static string OK = "OK";
        public static string GENERAL_FAILURE = "GENERAL_FAILURE";
        public static string INVALID_CREDENTIALS = "INVALID_CREDENTIALS";
        public static string STALE_SESSION = "STALE_SESSION";
    }

    class RestClient
    {
        enum StatusCode { OK, GENERAL_FAILURE, INVALID_CREDENTIALS, STALE_SESSION };


        public async Task<string> MakeRequestAsync(String endpoint, Dictionary<string, string> values)
        {
            var responseString = "";

            using (var client = new HttpClient())
            {

                var content = new FormUrlEncodedContent(values);

                var response = await client.PostAsync(endpoint, content);

                responseString = await response.Content.ReadAsStringAsync();
            }

            return responseString;
        }

        public async Task<string> GetRemoteVersionNumber()
        {
            String ret = "";

            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(Lib.API_UPDATES_GETLATESTVERSION);
                if (response.IsSuccessStatusCode)
                {
                    String versionResp = await response.Content.ReadAsStringAsync();

                    ResponseVersion version = JsonConvert.DeserializeObject<ResponseVersion>(versionResp);
                    ret = version.versionNumber;
                }
            }

            return ret;
        }

        public async Task<string> DownloadInstaller()
        {
            using (var client = new HttpClient())
            {
                client.Get
            }
        }

    } // class

}
