using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net.Sockets;
using ProtoBuf;
using ProtoBuf.Meta;
using System.IO;
using com.gamestudio.tank;

namespace LekeNet
{
    public class UdpPacket
    {
        public UdpPacket()
        {
        }

        public static UdpPacket CreatePacket(int opcode, Object proto)
        {
            UdpPacket packet = new UdpPacket();
            packet.opcode = opcode;
            if(proto != null) {
                MemoryStream stream = new MemoryStream();
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

        public static UdpPacket CreatePacket(OpCode opcode, Object proto)
        {
            return CreatePacket((int)opcode, proto);
        }

        public static UdpPacket CreatePacket(int opcode, byte[] data)
        {
            UdpPacket packet = new UdpPacket();
            packet.opcode = opcode;
            packet.data = data;

            return packet;
        }

        public const short HEAD_SIZE = 4;

        public byte[] data;

        int opcode = 0;
        bool needXor = true;

        private string toIp;

        public string ToIp
        {
            set
            {
                this.toIp = value;
            }
            get { return this.toIp; }
        }

        public UdpPacket(byte[] data, int opcode)
        {
            this.data = data;
            this.opcode = opcode;
        }

        public UdpPacket(int opcode, byte[] bytes)
        {
            this.data = bytes;
            this.opcode = opcode;
        }

        public int Opcode
        {
            get { return opcode; }
            set { this.opcode = value; }
        }

        public void setNeedXor(bool needXor)
        {
            this.needXor = needXor;
        }

        public bool isNeedXor()
        {
            return needXor;
        }

        public int getSize()
        {
            return data.Length;
        }

        public string ToString()
        {
            return "opcode=" + opcode + "&size=" + data.Length;
        }

        public void WriteToStream(Stream stream)
        {
            BinaryWriter binaryWriter = new BinaryWriter(stream);
            // 协议id
            binaryWriter.Write(ConvertEndianHelper.CheckEndian(opcode));
            if(data != null)
            {
                binaryWriter.Write(data);
            }
            binaryWriter.Flush();
        }


        public Stream GenerateStream()
        {
            MemoryStream stream = new MemoryStream();
            stream.Write(data, 0, data.Length);
            stream.Position = 0;
            return stream;
        }

        public UdpPacket Clone()
        {
            UdpPacket udpPakcet = new UdpPacket();
            udpPakcet.opcode = this.opcode;
            udpPakcet.data = this.data;
            udpPakcet.toIp = this.toIp;
            return udpPakcet;
        }

        public T Deserialize<T>()
        {
            return Serializer.Deserialize<T>(this.GenerateStream());
        }

        public static UdpPacket Decode(byte[] bytes, int begin, int offset)
        {
            if (bytes.Length >= UdpPacket.HEAD_SIZE)
            {
                int opCode = ConvertEndianHelper.CheckEndian(BitConverter.ToInt32(bytes, begin));
                byte[] databytes = new byte[offset - UdpPacket.HEAD_SIZE];
                Array.Copy(bytes, UdpPacket.HEAD_SIZE + begin, databytes, 0, offset - UdpPacket.HEAD_SIZE);
                UdpPacket packet = UdpPacket.CreatePacket(opCode, databytes);
                return packet;
            }

            return null;
        }
    }

}
