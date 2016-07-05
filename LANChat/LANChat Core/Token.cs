using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace LANChat_Core
{
    [Serializable]
    public class Token
    {
        public string signature { get; }

        public Token()
        {
            var rnd = new Random((int)DateTime.Now.Ticks);
            int seed = rnd.Next(5844);
            MD5 md5Hash = MD5.Create();
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(seed.ToString()));

            // Create a new Stringbuilder to collect the bytes and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            signature = sBuilder.ToString();
        }
    }
}
