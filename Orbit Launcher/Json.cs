using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Newtonsoft.Json;

using OrbitLauncher.Authentication;

namespace OrbitLauncher
{
    class Json
    {
        public static void CreateNewAuthenticationFile(string filePath, AuthenticationFile authFile)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            string Json = JsonConvert.SerializeObject(authFile, Formatting.Indented);
            File.WriteAllText(filePath, Json);
        }

        public static void WriteAuthenticationToFile(string filePath, AuthenticationFile authFile)
        {
            string Json = JsonConvert.SerializeObject(authFile, Formatting.Indented);
            File.WriteAllText(filePath, Json);
        }

        public static AuthenticationFile RetrieveAuthenticationData()
        {
            AuthenticationFile AuthFile = new AuthenticationFile();
            var AuthenticationFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Lib.AUTHENTICATION_FILE_LOCATION); // Location of Authentication file
            try
            {
                if (!File.Exists(AuthenticationFilePath))
                {
                    Console.WriteLine("No authentication file found. Creating.");
                    AuthenticationFile NewAuthenticationFile = new AuthenticationFile();
                    NewAuthenticationFile.Profiles = new Dictionary<String, Profile>();

                    Json.CreateNewAuthenticationFile(AuthenticationFilePath, NewAuthenticationFile);
                }

                String FileContents = File.ReadAllText(AuthenticationFilePath);

                AuthFile = JsonConvert.DeserializeObject<AuthenticationFile>(FileContents);
                
            }
            catch (JsonSerializationException)
            {
                File.Delete(AuthenticationFilePath);
            }

            return AuthFile;
        }
    }
}
