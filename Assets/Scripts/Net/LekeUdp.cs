using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using LekeNet.Room;

namespace LekeNet
{
    public class LekeUdp : NetHandler
    {
        private UdpClient sendClient;
        // 接受协议的udpClient
        private UdpClient recvClient;
        // 接受协议的endpoint引用
        private IPEndPoint mIPEndPoint;
        // 写缓冲区
        private MemoryStream writeBuff;

        private List<UdpSession> sessionList;

        public LekeUdp()
        {
            this.sendClient = new UdpClient();
            this.sessionList = new List<UdpSession>();
            this.writeBuff = new MemoryStream();
            this.mIPEndPoint = new IPEndPoint(IPAddress.Any, 0);
        }

        public UdpClient SendClient
        {
            get { return this.sendClient; }
        }

        public List<UdpSession> SessionList
        {
            get {
                List<UdpSession> sessList = new List<UdpSession>();
                sessList.AddRange(this.sessionList);
                return sessList;
            }
        }

        public void RemoveSession(UdpSession session)
        {
            lock(this.sessionList)
            {
                this.sessionList.Remove(session);
            }
        }

        public void AddSession(UdpSession session)
        {
            lock (this.sessionList)
            {
                this.sessionList.Add(session);
            }
        }

        // 开启监听
        public void StartBind(string ip, int port)
        {
            if (recvClient != null)
            {
                recvClient.Close();
            }

            if (this.sessionList == null)
            {
                this.sessionList = new List<UdpSession>();
            }
            else
            {
                this.sessionList.Clear();
            }
            if (sendClient != null)
            {
                sendClient.Close();
            }

            StartNetHandler();
            sendClient = new UdpClient();
            recvClient = new UdpClient(new IPEndPoint(IPAddress.Parse(ip), port));
            recvClient.BeginReceive(ReceiveCallback, this);
        }

        void ReceiveCallback(IAsyncResult ar)
        {
            byte[] data = (mIPEndPoint == null) ?
                recvClient.Receive(ref mIPEndPoint) :
                recvClient.EndReceive(ar, ref mIPEndPoint);
            try
            {
                UdpSession session = FindSession(mIPEndPoint);
                if (session == null)
                {
                    // session 为空 需要创建新的session
                    session = CreateSession(mIPEndPoint);
                }
                UdpPacket packet = UdpPacket.Decode(data, 0, data.Length);
                session.OnRecv(packet);
                KeyValuePair<UdpSession, UdpPacket> pair = new KeyValuePair<UdpSession, UdpPacket>(session, packet);
                this.AddPacket(pair);

                LogUtil.Log("net", "recv data from : " + mIPEndPoint.Address.ToString() + "opcode : " + packet.Opcode);
            }catch(Exception e)
            {
                LogUtil.Log("net", "server recv data error : " + e.Message + " " + e.StackTrace);
            }
            
            if (recvClient != null)
            {
                recvClient.BeginReceive(ReceiveCallback, this);
            }
        }

        // 发送给一个端
        public void SendTo(string ip, int port, UdpPacket packet)
        {
            //if (packet == null) return;
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            packet.WriteToStream(writeBuff);
            sendClient.Send(writeBuff.GetBuffer(), (int)writeBuff.Position, iPEndPoint);
            writeBuff.Position = 0;
        }

        // 创建Session
        public UdpSession CreateSession(IPEndPoint endPoint)
        {
            foreach (UdpSession session in this.SessionList)
            {
                if (session.Ip.Equals(NetHelper.GetIpFromAddress(endPoint.Address)))
                {
                    return session;
                }
            }
            UdpSession sess = new UdpSession(sendClient, endPoint);
            AddSession(sess);
            return sess;
        }

        // 根据端的信息查找已经存在的客户端Session
        public UdpSession FindSession(IPEndPoint endPoint)
        {
            foreach (UdpSession session in this.SessionList)
            {
                if (session.Ip.Equals(NetHelper.GetIpFromAddress(endPoint.Address)))
                {
                    return session;
                }
            }
            return null;
        }

        public override void Destory()
        {
            base.Destory();
            if(this.sendClient != null)
            {
                this.sendClient.Close();
                this.sendClient = null;
            }
            if (this.recvClient != null)
            {
                this.recvClient.Close();
                this.recvClient = null;
            }
        }
    }
}
