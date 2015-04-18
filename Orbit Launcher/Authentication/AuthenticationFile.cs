using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrbitLauncher.Authentication
{
    class AuthenticationFile
    {

        public string ClientToken { get; set; }
        public string SelectedProfile { get; set; }
        public List<Profile> Profiles { get; set; }
    }
}
