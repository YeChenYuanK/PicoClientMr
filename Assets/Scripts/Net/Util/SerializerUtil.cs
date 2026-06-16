using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using ProtoBuf;

namespace LekeNet.Util
{
    public class SerializerUtil
    {
        private static MemoryStream stream = new MemoryStream();

        private static MemoryStream deserStream = new MemoryStream();

        public static T Deserialize<T>(Stream stream)
        {
            return Serializer.Deserialize<T>(stream);
        }

        public static T Deserialize<T>(UdpPacket packet)
        {
            return Serializer.Deserialize<T>(packet.GenerateStream());
        }

        public static T Deserialize<T>(byte[] bytes)
        {
            lock (deserStream)
            {
            // deserStream = new MemoryStream();
                deserStream.Position = 0;
                deserStream.Write(bytes, 0, bytes.Length);
                deserStream.Position = 0;
                return Serializer.Deserialize<T>(deserStream);
            }
        }

        public static byte[] GenerateBytes(Object msg)
        {
            lock(stream) {
                Serializer.Serialize(stream, msg);
                int size = (int)stream.Position;
                byte[] data = new byte[size];
                stream.Position = 0;
                stream.Read(data, 0, size);
                stream.Position = 0;

                return data;
            }
        }
    }
}
