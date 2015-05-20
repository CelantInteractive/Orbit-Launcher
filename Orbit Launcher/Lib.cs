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
        static public string TEMP_INSTALLER_LOCATION = "Celant Interactive\\Orbit Launcher\\setup.exe";
        static public string TEMP_LOCATION = "Celant Interactive\\Orbit Launcher\\";

        static public string CARDINAL_BALANCER = @"http://cardinal.celantinteractive.com";
        static public string CARDINAL_SERVER { get; set; }

        public static string API_LOGIN = "/login";
        public static string API_REFRESH = "/refresh";
        public static string API_VALIDATE = "/validate";
        public static string API_LOGOUT = "/logout";
        public static string API_INVALIDATE = "/invalidate";
        public static string API_UPDATES_GETLATESTVERSION = "/updates/getLatestVersion";
        public static string API_UPDATES_GETINSTALLER = "/updates/getInstaller";
        public static string API_LOADBALANCE = "/loadbalance";
    }
}
