using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrbitLauncher.Authentication
{
    class ResponseLogin : ResponseFrame
    {
        public String UUID { get; set; }
        public String username { get; set; }
        public String accessToken { get; set; }
        public String clientToken { get; set; }
    }
}
