using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Dynamic;
using System.Windows.Forms;

using Newtonsoft.Json;
using Newtonsoft.Json.Utilities;

using OrbitLauncher.Authentication;
using HttpUtils;

namespace OrbitLauncher
{
    /// <summary>
    /// Useful functions for anything related to user authentication
    /// </summary>
    class AuthManager
    {
        public AuthenticationFile AuthenticationDb;

        /// <summary>
        /// Return all profiles from the authentication file
        /// </summary>
        /// <returns>List of profiles</returns>
        public List<Profile> GetProfiles()
        {
            var AuthenticationFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Lib.AUTHENTICATION_FILE_LOCATION); // Location of Authentication file
            
            if (!File.Exists(AuthenticationFilePath))
            {
                Console.WriteLine("No authentication file found. Creating.");
                AuthenticationFile NewAuthenticationFile = new AuthenticationFile();
                NewAuthenticationFile.Profiles = new List<Profile>();

                Json.CreateNewAuthenticationFile(AuthenticationFilePath, NewAuthenticationFile);
            }

            String FileContents = File.ReadAllText(AuthenticationFilePath);

            AuthenticationDb = JsonConvert.DeserializeObject<AuthenticationFile>(FileContents);

            return AuthenticationDb.Profiles;
        }

        private void AddNewProfile(string username, string accessToken, string UUID)
        {
            Profile profile = new Profile();
            profile.Username = username;
            profile.UUID = UUID;
            profile.AccessToken = accessToken;

            AuthenticationDb.Profiles.Add(profile);
            UpdateAuthenticationFile();
        }

        public void UpdateAuthenticationFile()
        {
            var AuthenticationFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Lib.AUTHENTICATION_FILE_LOCATION); // Location of Authentication file

            Json.WriteAuthenticationToFile(AuthenticationFilePath, this.AuthenticationDb);
        }

        public void AuthenticateNewProfile(string username, string password)
        {
            foreach (Profile profile in AuthenticationDb.Profiles)
            {
                if (profile.Username.Equals(username))
                {
                    MessageBox.Show("This user is already in the profiles list");
                }
            }
            var FrameLogin = new FrameLogin(username, password, "");

            string Data = JsonConvert.SerializeObject(FrameLogin, Formatting.None);
            var client = new RestClient(@"https://localhost:8443/Authentication/login", HttpVerb.POST, Data);

            var json = client.MakeRequest();

            ResponseLogin response = JsonConvert.DeserializeObject<ResponseLogin>(json);

            if (response != null)
            {
                if (response.statusCode.Equals("0x0"))
                {
                    AuthenticationDb.ClientToken = response.clientToken;
                    AddNewProfile(response.username, response.accessToken, response.UUID);
                }
                else
                {
                    MessageBox.Show(response.statusMessage, response.statusCode);
                }
            }
        }
    }
}
