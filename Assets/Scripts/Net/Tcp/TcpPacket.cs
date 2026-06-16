using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using ProtoBuf;
using ProtoBuf.Meta;
namespace LekeNet
{

    public class TcpPacket
    {
        public TcpPacket()
        {
        }

        public static TcpPacket CreatePacket(int opcode, Object proto)
        {
            TcpPacket packet = new TcpPacket();
            MemoryStream stream = new MemoryStream();
            packet.opcode = opcode;
            if (proto != null)
            {
                Serializer.Serialize(stream, proto);
                int size = (int)stream.Position;
                packet.data = new byte[size];
                stream.Position = 0;
                stream.Read(packet.data, 0, size);
            } else
            {
                packet.data = new byte[0];
            }
            
            return packet;
        }

        public static TcpPacket CreatePacket(int opcode, byte[] data)
        {
            TcpPacket packet = new TcpPacket();
            packet.opcode = opcode;
            packet.data = data;

            return packet;
        }

        public const short HEAD_SIZE = 16;
        public const int DATA_MASK = 0xEC;
        public const short PACKET_HEAD = 0x5D6B;

        public byte[] data;

        int opcode = 0;
        bool needXor = true;

        public TcpPacket(byte[] data, int opcode)
        {
            this.data = data;
            this.opcode = opcode;
        }

        public TcpPacket(int opcode, byte[] bytes)
        {
            this.data = bytes;
            this.opcode = opcode;
        }

        public int OpCode
        {
            get { return opcode; }
        }

        public bool IsNeedXor
        {
            get { return needXor; }
            set { this.needXor = value;}
        }

        public int Size
        {
            get { return data.Length; }
        }

        public string ToString()
        {
            return "opcode=" + opcode + "&size=" + data.Length; 
        }

        public void WriteToStream(Stream stream)
        {
            //BinaryBigEndianWriter bw = new BinaryBigEndianWriter(stream);
            BinaryWriter binaryWriter = new BinaryWriter(stream);
            // 协议头
            //binaryWriter.Write(ConvertEndianHelper.CheckEndian(Packet.PACKET_HEAD));
            // 协议id
            binaryWriter.Write(ConvertEndianHelper.CheckEndian(opcode));

            // 发送字节长度
            binaryWriter.Write(ConvertEndianHelper.CheckEndian(data.Length));
            // 预留字节
            int b = 0;
            binaryWriter.Write(b);
            // 压缩标记
            binaryWriter.Write(b);
            //if (needXor)
            //{
            //    for (int i = 0; i < data.Length; i++)
            //    {
            //        binaryWriter.Write((byte)(data[i] ^ DATA_MASK));
            //    }
            //}
            //else
            //{
            binaryWriter.Write(data);
            //}
        }

        private uint CalcCrc(byte[] bytes, uint offset, uint size, uint crc)
        {
            uint hash = crc;
            for (uint i = offset; i < offset + size; i++)
            {
                // hash ^= ((i & 1) == 0) ? ((hash << 7) ^ (bytes[i] & 0xff) ^ (hash >> 3)) : (~((hash << 11) ^ (bytes[i] & 0xff) ^ (hash >> 5)));
            }
            return hash;

        }

        public Stream GenerateStream()
        {
            MemoryStream stream = new MemoryStream();
            stream.Write(data, 0, data.Length);
            stream.Position = 0;
            return stream;
        }

        public T Deserialize<T>()
        {
            return Serializer.Deserialize<T>(this.GenerateStream());
        }

    }
}
