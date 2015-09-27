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

namespace OrbitLauncher
{
    /// <summary>
    /// Useful functions for anything related to user authentication
    /// </summary>
    class AuthManager
    {
        public AuthenticationFile AuthenticationDb;

        Main Parent;

        public AuthManager(Main parent)
        {
            this.Parent = parent;
        }

        /// <summary>
        /// Return all profiles from the authentication file
        /// </summary>
        /// <returns>List of profiles</returns>
        public Dictionary<String,Profile> GetProfiles()
        {
            AuthenticationDb = AuthFileManager.RetrieveAuthenticationData();

            return AuthenticationDb.Profiles;
        }

        private void AddNewProfile(string email, string accessToken, string cardinalId)
        {
            Profile profile = new Profile();
            profile.Email = email;
            profile.AccessToken = accessToken;

            AuthenticationDb.Profiles.Add(cardinalId,profile);
            UpdateAuthenticationFile();
        }

        public void UpdateAuthenticationFile()
        {
            var AuthenticationFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Lib.AUTHENTICATION_FILE_LOCATION); // Location of Authentication file

            AuthFileManager.WriteAuthenticationToFile(AuthenticationFilePath, this.AuthenticationDb);
        }

        public async void AuthenticateNewProfile(string email, string password)
        {
            ResponseLogin response = new ResponseLogin();
            try
            {

                foreach (KeyValuePair<String, Profile> profile in AuthenticationDb.Profiles)
                {
                    if (profile.Value.Email.Equals(email))
                    {
                        MessageBox.Show("This user is already in the profiles list");
                        return;
                    }
                }

                Dictionary<string, string> Data = new Dictionary<string, string>()
                {
                    {"email", email},
                    {"password", password},
                    {"clientToken", AuthenticationDb.ClientToken}
                };
                var client = new RestClient();

                string json = await client.MakeRequestAsync(Lib.API_LOGIN, Data);

                response = JsonConvert.DeserializeObject<ResponseLogin>(json);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            if (response != null & !response.Equals(""))
            {
                if (response.statusCode.Equals(StatusCode.OK))
                {
                    AuthenticationDb.ClientToken = response.clientToken;
                    AddNewProfile(email, response.accessToken, response.cardinalId);
                    Parent.RefreshProfileList();
                }
                else
                {
                    if (response.statusCode.Equals(StatusCode.INVALID_CREDENTIALS))
                    {
                        MessageBox.Show("Invalid email/password combination");
                    }
                    else
                    {
                        MessageBox.Show("An internal error occured.\n Please wait and try again, and if you still cannot log in, contact customer support with Unique Support Code: " + response.uniqueSupport);
                    }
                    
                }
            }
        }
    }
}
