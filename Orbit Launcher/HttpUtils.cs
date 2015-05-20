using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                client.Timeout = new TimeSpan(0, 0, 5);
                client.BaseAddress = new Uri(Lib.CARDINAL_SERVER);

                var content = new FormUrlEncodedContent(values);

                var response = await client.PostAsync(endpoint, content);

                responseString = await response.Content.ReadAsStringAsync();
            }

            return responseString;
        }

        public String GetRequest(String endpoint)
        {
            return GetRequest(endpoint, "");
        }

        public String GetRequest(String endpoint, String baseaddress)
        {
            String ret = "";
            try
            {
                using (var client = new HttpClient())
                {
                    client.Timeout = new TimeSpan(0, 0, 5);

                    if (baseaddress == "")
                    {
                        client.BaseAddress = new Uri(Lib.CARDINAL_SERVER);
                    }
                    else
                    {
                        client.BaseAddress = new Uri(baseaddress);
                    }

                    HttpResponseMessage HttpResponse = client.GetAsync(endpoint).Result;

                    if (HttpResponse.IsSuccessStatusCode)
                    {
                        HttpContent ResponseContent = HttpResponse.Content;
                        String Response = ResponseContent.ReadAsStringAsync().Result;
                        if (Response != null & Response != "")
                        {
                            ret = Response;
                        }
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Could not contact Cardinal account servers", "Connection Error");
                Process currentProcess = Process.GetCurrentProcess();

                currentProcess.Kill();

            }

            return ret;
        }

        public string GetRemoteVersionNumber()
        {
            String ret = "";

            String versionResp = GetRequest(Lib.API_UPDATES_GETLATESTVERSION);

            ResponseVersion version = JsonConvert.DeserializeObject<ResponseVersion>(versionResp);
            if (version.latestVersion != "")
            {
                ret = version.latestVersion;
            }
            else
            {
                ret = "0";
            }

            return ret;
        }

        public async Task DownloadInstaller()
        {
            using (var client = new HttpClient())
            {
                client.Timeout = new TimeSpan(0, 0, 5);
                client.BaseAddress = new Uri(Lib.CARDINAL_SERVER);

                using (var request = new HttpRequestMessage(HttpMethod.Get, Lib.API_UPDATES_GETINSTALLER))
                {
                    Stream contentStream = await (await client.SendAsync(request)).Content.ReadAsStreamAsync();

                    String exeLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Lib.TEMP_INSTALLER_LOCATION);

                    var stream = new FileStream(exeLocation, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true);

                    await contentStream.CopyToAsync(stream);

                    stream.Close();
                }
            }
        }

    } // class

}
