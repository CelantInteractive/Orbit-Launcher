using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrbitLauncher
{
    /// <summary>
    /// Library file with static variables
    /// </summary>
    class Lib
    {
        static public string AUTHENTICATION_FILE_LOCATION = "Celant Interactive\\Orbit Launcher\\authentication.json";
        static public string VERSION_FILE_LOCATION = "Celant Interactive\\Orbit Launcher\\version.json";
        static public string CARDINAL_SERVER = @"http://localhost:8080";
        public static string API_LOGIN = CARDINAL_SERVER + "/login";
        public static string API_REFRESH = CARDINAL_SERVER + "/refresh";
        public static string API_VALIDATE = CARDINAL_SERVER + "/validate";
        public static string API_LOGOUT = CARDINAL_SERVER + "/logout";
        public static string API_INVALIDATE = CARDINAL_SERVER + "/invalidate";
        public static string API_UPDATES_GETLATESTVERSION = CARDINAL_SERVER + "/updates/getLatestVersion";
        public static string API_UPDATES_GETINSTALLER = CARDINAL_SERVER + "/updates/getInstaller";
    }
}
