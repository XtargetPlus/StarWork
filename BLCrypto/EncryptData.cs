using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BLCrypto
{
    public class EncryptData
    {
        public async Task<byte[]> EncryptAes(string plainText, byte[] key, byte[] iv)
        {
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException(nameof(plainText), "текст меньше 0");
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException(nameof(key), "ключ меньше 0");
            if (iv == null || iv.Length <= 0)
                throw new ArgumentNullException(nameof(iv), "вектор инициализации меньше 0");

            byte[] encrypted;

            using(Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new())
                {
                    using (CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new(csEncrypt))
                        {
                            await swEncrypt.WriteAsync(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            return encrypted;
        }

        public (byte[], byte[]) HashValue(string value)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(16);
            byte[] valueByte = Encoding.UTF8.GetBytes(value);
            byte[] saltAndValue = salt.Concat(valueByte).ToArray();

            using (SHA256 mySHA256 = SHA256.Create())
            {
                byte[] hashValue = mySHA256.ComputeHash(saltAndValue);
                return (hashValue, salt);
            }
        }
    }
}
