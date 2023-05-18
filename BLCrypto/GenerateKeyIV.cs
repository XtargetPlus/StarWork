using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace BLCrypto
{
    public class GenerateKeyIV
    {
        public Dictionary<byte[], byte[]> GetKeyIV()
        {
            using (Aes aes = Aes.Create())
            {
                aes.KeySize = 256;
                Dictionary<byte[], byte[]> result = new()
                {
                    { aes.Key, aes.IV }
                };
                return result;
            }
        }
        
        public byte[] GetKeyByte(byte[] value)
        {
            byte[] keyByte = new byte[32];
            if (value.Length < 32)
            {
                int i = 0;
                while (i < value.Length)
                {
                    keyByte[i] = value[i];
                    i++;
                }
            }
            else
            {
                int i = 0;
                while (i < 32)
                {
                    keyByte[i] = value[i];
                    i++;
                }
            }
            return keyByte;
        }
        public byte[] GetIVByte(byte[] value)
        {
            byte[] ivByte = new byte[16];
            if (value.Length < 16)
            {
                int i = 0;
                while (i < value.Length)
                {
                    ivByte[i] = value[i];
                    i++;
                }
            }
            else
            {
                int i = 0;
                while (i < 16)
                {
                    ivByte[i] = value[i];
                    i++;
                }
            }
            return ivByte;
        }
    }
}
