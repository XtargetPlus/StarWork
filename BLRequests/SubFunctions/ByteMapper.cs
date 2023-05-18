using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLRequests.SubFunctions
{
    public static class ByteMapper
    {
        public static string JoinComma(this byte[] value)
        {
            return string.Join(",", value);
        }
    }
}
