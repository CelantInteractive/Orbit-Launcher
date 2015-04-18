using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrbitLauncher.Authentication
{
    class FrameLogin
    {
        public string username;
        public string password;
        public string clientToken;

        public FrameLogin(string username, string password, string clientToken)
        {
            this.username = username;
            this.password = password;
            this.clientToken = clientToken;
        }
    }
}
