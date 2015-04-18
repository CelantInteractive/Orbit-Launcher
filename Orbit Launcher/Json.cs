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
    }
}
