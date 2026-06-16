using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace LekeNet
{
    public class TcpSession
    {
        private Socket socket;
        private MemoryStream writeBuff;
        private MemoryStream readBuff;

        public bool IsActive = false;

        public TcpSession(Socket socket)
        {
            this.socket = socket;
            writeBuff = new MemoryStream();
            readBuff = new MemoryStream();

            IsActive = true;
        }

        /**
         * 发送协议包
         */
        public void SendProto(int opcode, System.Object proto)
        {
            TcpPacket packet = TcpPacket.CreatePacket(opcode, proto);
            SendProto(packet);
        }

        //同步发送数据
        public void SendProto(TcpPacket packet)
        {
            if (socket == null)
            {
                LogUtil.Log("net", "scoket is null");
                return;
            }

            if (!socket.Connected)
            {
                LogUtil.Log("net", "scoket lost connection");
                return;
            }

            writeBuff.Position = 0;
            packet.WriteToStream(writeBuff);
            int size = (int)writeBuff.Position;
            writeBuff.Position = 0;
            int result = socket.Send(writeBuff.GetBuffer(), 0, size, SocketFlags.None);
            if (result == 0)
            {
                LogUtil.Log("net", "send proto failed");
            }
            else
            {
                LogUtil.Log("net", "send success : " + result);
            }
        }

    }
}
