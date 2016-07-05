using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Shared
{
    public static class Utils
    {

        public static void WriteColour(string text, ConsoleColor colour = ConsoleColor.Gray)
        {
            ConsoleColor tmp = Console.ForegroundColor;

            Console.ForegroundColor = colour;
            Console.WriteLine("\r" + text);

            Console.ForegroundColor = tmp;

            Console.Write(">");
        }

        public static byte[] ObjectToByteArray(Object obj)
        {
                BinaryFormatter bf = new BinaryFormatter();
                using (var ms = new MemoryStream())
                {
                    bf.Serialize(ms, obj);
                    return ms.ToArray();
                }
        }

        public static Object ByteArrayToObject(byte[] arrBytes)
        {
            using (var memStream = new MemoryStream())
            {
                var binForm = new BinaryFormatter();
                memStream.Write(arrBytes, 0, arrBytes.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                var obj = binForm.Deserialize(memStream);
                return obj;
            }
        }
    }
}
