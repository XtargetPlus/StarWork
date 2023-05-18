using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLRequests.SubFunctions
{
    public static class StringMapper
    {
        public static byte[] ConvertToSplitByte(this string text)
        {
            return text.Split(",").Select(byte.Parse).ToArray();
        }

        public static byte[] UTF8GetBytes(this string text)
        {
            return Encoding.UTF8.GetBytes(text);
        }
    }
}
