using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLCrypto
{
    public class AuthOptions
    {
        public const string ISSUER = "Server";
        public const string AUDIENCE = "Client";
        private static byte[] _key = null!;

        public AuthOptions(byte[] key)
        {
            _key = key;
        }

        public static SymmetricSecurityKey GetSymmetricSecurityKey() => new(_key);
    }
}
