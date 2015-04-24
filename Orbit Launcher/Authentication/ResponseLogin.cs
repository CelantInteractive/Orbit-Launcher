using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrbitLauncher.Authentication
{
    class ResponseLogin : ResponseFrame
    {
        public String cardinalId { get; set; }
        public String accessToken { get; set; }
        public String clientToken { get; set; }
    }
}
