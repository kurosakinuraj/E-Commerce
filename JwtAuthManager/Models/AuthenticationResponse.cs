using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtAuthManager.Models
{
    public class AuthenticationResponse
    {
        public string UserName { get; set; }
        public string Token { get; set; }
        public int ExpiresIn { get; set; }
    }
}
