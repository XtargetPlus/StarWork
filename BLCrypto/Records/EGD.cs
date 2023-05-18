using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLCrypto.Records
{
    public class EGD
    {
        public EncryptData EncryptData { get; } = new();
        public GenerateKeyIV CreateKeyIv { get; } = new();
        public DecryptData DecryptData { get; } = new();
    }
}
