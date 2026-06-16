using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using LekeNet.Room;
using UnityEngine;
using System.Threading;
using com.gamestudio.cs;
using com.gamestudio.room;
using com.gamestudio.sys;

namespace LekeNet
{
    public class LekeUdp1 : UdpNetHandler
    {
        private UdpClient udpClient;
        // 接受协议的endpoint引用
        private IPEndPoint mIPEndPoint;
        // 写缓冲区
        private MemoryStream writeBuff;

        private UdpSession udpSession;

        private Thread protoThread;
        private bool receive;

        private IPEndPoint serverEndPoint;

        public UdpSession UdpSession
        {
            get
            {
                return udpSession;
            }

            set
            {
                udpSession = value;
            }
        }

        public LekeUdp1()
        {
            this.udpClient = new UdpClient();
            this.writeBuff = new MemoryStream();
            this.mIPEndPoint = new IPEndPoint(IPAddress.Any, 0);
            this.receive = false;
        }

        public void Init(string ipAddress, int port, BuildUdpSession buildSession)
        {
            this.StartUdpNetHandler();


            serverEndPoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);
            UdpPacket packet = UdpPacket.CreatePacket((int)GameOpCode.BUILD_UDP_SESSION, buildSession);
            packet.WriteToStream(writeBuff);
            udpClient.Send(writeBuff.GetBuffer(), (int)writeBuff.Position, serverEndPoint);
            writeBuff.Position = 0;

            mIPEndPoint = new IPEndPoint(IPAddress.Any, 0);
            receive = true;
            protoThread = new Thread(new ThreadStart(receiveUdpProto));
            protoThread.Start();
        }

        private void receiveUdpProto()
        {
            while (receive)
            {
                byte[] data = udpClient.Receive(ref mIPEndPoint);
                if (this.UdpSession == null)
                {
                    this.UdpSession = CreateSession(serverEndPoint);
                }

                UdpPacket p = UdpPacket.Decode(data, 0, data.Length);
                KeyValuePair<UdpSession, UdpPacket> pair = new KeyValuePair<UdpSession, UdpPacket>(this.UdpSession, p);
                this.AddPacket(pair);
            }
        }

        // 发送给一个端
        public void SendTo(string ip, int port, UdpPacket packet)
        {
            if (packet == null) return;
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            packet.WriteToStream(writeBuff);
            udpClient.Send(writeBuff.GetBuffer(), (int)writeBuff.Position, iPEndPoint);
            writeBuff.Position = 0;
        }

        // 创建Session
        public UdpSession CreateSession(IPEndPoint endPoint)
        {
            UdpSession sess = new UdpSession(udpClient, endPoint);
            return sess;
        }

        public override void Destory()
        {
            base.Destory();
            if(this.UdpSession != null)
            {
                this.UdpSession = null;
            }
            if (this.udpClient != null)
            {
                this.udpClient.Close();
                this.udpClient = null;
            }
            this.receive = false;
        }
    }
}
