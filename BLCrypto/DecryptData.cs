using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace BLCrypto
{
    public class DecryptData
    {
        public async Task<string> DecryptAes(byte[] cipherText, byte[] key, byte[] iv)
        {
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException(nameof(cipherText), "текст меньше 0");
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException(nameof(key), "ключ меньше 0");
            if (iv == null || iv.Length <= 0)
                throw new ArgumentNullException(nameof(iv), "вектор инициализации меньше 0");

            string plainText;

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream msDecrypt = new(cipherText))
                {
                    using (CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new(csDecrypt))
                        {
                            plainText = await srDecrypt.ReadToEndAsync();
                        }
                    }
                }
            }
            return plainText;
        }

        public int CheckEquelHashPasswords(byte[] hashPassword, byte[] salt, string password)
        {
            byte[] saltAndPassword = salt.Concat(Encoding.UTF8.GetBytes(password)).ToArray();

            using (SHA256 mySHA256 = SHA256.Create())
            {
                byte[] hashValue = mySHA256.ComputeHash(saltAndPassword);
                if (hashValue.Length == hashPassword.Length)
                {
                    int i = 0;
                    while ((i < hashPassword.Length) && (hashPassword[i] == hashValue[i]))
                    {
                        i++;
                    }
                    if (i == hashPassword.Length)
                    {
                        return 0;
                    }
                }      
            }
            return 1;
        }
    }
}
