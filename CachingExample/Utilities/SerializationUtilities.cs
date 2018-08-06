using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace CachingExample.Utilities
{
    public static class SerializationUtilities
    {

        public static byte[] Serialize(object model)
        {
            if (model != null)
            {
                BinaryFormatter bf = new BinaryFormatter();
                using (MemoryStream ms = new MemoryStream())
                {
                    bf.Serialize(ms, model);
                    return ms.ToArray();
                }
            }
            return null;
        }

        public static T Deserialize<T>(byte[] bytes) where T : class
        {
            return Deserialize(bytes) as T;
        }

        public static object Deserialize(byte[] bytes)
        {
            if (bytes != null && bytes.Length > 0)
            {
                var bf = new BinaryFormatter();
                using (var ms = new MemoryStream(bytes))
                {
                    return bf.Deserialize(ms);
                }
            }
            return null;
        }

    }
}
