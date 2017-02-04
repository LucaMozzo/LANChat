using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Shared
{
    /// <summary>
    /// Contains methods used across projects
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Writes on the console with the given colour, and sets the console colour back to default
        /// </summary>
        /// <param name="text">Text to be printed</param>
        /// <param name="colour">The foreground colour</param>
        public static void WriteColour(string text, ConsoleColor colour = ConsoleColor.Gray)
        {
            ConsoleColor tmp = Console.ForegroundColor;

            Console.ForegroundColor = colour;
            Console.WriteLine("\r" + text);

            Console.ForegroundColor = tmp;

            Console.Write(">");
        }

        /// <summary>
        /// Converts a serializable object into a byte array for transmission
        /// </summary>
        /// <param name="obj">The object</param>
        /// <returns>The byte array</returns>
        public static byte[] ObjectToByteArray(object obj)
        {
                BinaryFormatter bf = new BinaryFormatter();
                using (var ms = new MemoryStream())
                {
                    bf.Serialize(ms, obj);
                    return ms.ToArray();
                }
        }

        /// <summary>
        /// Builds an object from a byte array
        /// </summary>
        /// <param name="arrBytes">The input byte array</param>
        /// <returns>The deserialized object</returns>
        public static Object ByteArrayToObject(byte[] arrBytes)
        {
            try
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
            catch (SerializationException e)
            {
                WriteColour("The message you're trying to receive is bigger than the buffer", ConsoleColor.Red);
                return null;
            }
        }
    }
}
