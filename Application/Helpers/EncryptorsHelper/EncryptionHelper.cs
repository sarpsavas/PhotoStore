using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helpers.Encryptors
{
    public class EncryptionHelper
    {
        public string Encryptor(string text)
        {
            SHA256 converter = SHA256.Create();

            text = $"d?h6_9#7@nbs*9{text}";
            byte[] byteA = converter.ComputeHash(Encoding.UTF8.GetBytes(text));

            StringBuilder builder = new StringBuilder();
            foreach (byte b in byteA)
            {
                builder.Append(b.ToString("x2"));
            }
            return builder.ToString();
        }
    }
}
