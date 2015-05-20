namespace OrbitLauncher.Authentication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    class AuthenticationFile
    {

        public string ClientToken { get; set; }
        public string SelectedProfile { get; set; }
        public Dictionary<String, Profile> Profiles { get; set; }
    }
}
