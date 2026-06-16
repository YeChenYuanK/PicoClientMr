using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using LekeNet;
using System.Collections.Generic;
using System.IO;
using com.gamestudio.tank;

namespace LekeNet
{
	// udp的上下文环境, 处理组播协议
	public class UdpModeContext : NetHandler
	{
		public UdpModeContext ()
		{
		}

		private UdpClient multiCastUdpClient ;

        private IPAddress multiCastAddress ;

		private IPEndPoint multiCastEndPoint;

		private UdpClient receiveMultiCastClient;

		// 存放所有的主机信息，数据来源于其他机器的组播通知
		private List<UdpSession> sessionList;
		// udp的本机host信息
		private LocalHost localHost ;

        public LocalHost LocalHost{
            get { return this.localHost; }
        }

        private MemoryStream writeBuf;

		// 初始化
		public bool Init() {
            this.writeBuf = new MemoryStream();
			// 默认加入组播的地址
			multiCastAddress = IPAddress.Parse (SysCfg.UDP_MULTICAST_GROUP);
			multiCastUdpClient = new UdpClient ();
			multiCastEndPoint = new IPEndPoint (multiCastAddress, SysCfg.UDP_MULTICAST_PORT);
            sessionList = new List<UdpSession>();
            StartNetHandler();
            // 监听组播通知
            StartReceiveMultiCast();
            // 广播通知所有人，这台机器加入
            UdpMultiNotify udpNotify = new UdpMultiNotify();
            udpNotify.id = 1;
            Multicast (UdpPacket.CreatePacket(OpCode.UDP_MULTI_INIT, udpNotify));
            // 上下文创建是否成功需要在这边处理
            int failTimes = 0;
            // 如果网络模块没有准备就一直等待
            while (!this.IsReady() && failTimes <= 10)
            {
                Thread.Sleep(1000);
                failTimes++;
                Multicast(UdpPacket.CreatePacket(OpCode.UDP_MULTI_INIT, udpNotify));
            }
            if (!this.IsReady())
            {
                LogUtil.Log("net", "udp context init fail");
                throw new LekeNetException(LekeExceptionCode.UDP_NET_WORK_FAIL);
            }
            return true;
        }

        public void Multicast(UdpPacket packet) {
            packet.WriteToStream(writeBuf);
			multiCastUdpClient.Send(writeBuf.GetBuffer(), (int)writeBuf.Position, multiCastEndPoint);
            writeBuf.Position = 0;
        }

        // 开启组播监听
		public void StartReceiveMultiCast()
		{
			if (receiveMultiCastClient != null) {
				receiveMultiCastClient.Close ();
			}
			receiveMultiCastClient = new UdpClient(new IPEndPoint(IPAddress.Any, SysCfg.UDP_MULTICAST_PORT));
			// 加入组播组
			receiveMultiCastClient.JoinMulticastGroup(multiCastAddress);
			receiveMultiCastClient.Ttl = SysCfg.UDP_TTL;

			new Thread(new ThreadStart(receiveMulti)).Start();

			LogUtil.Log ("net", "startReceiveMultiCast");
		}

        // 关闭组播监听
		public void StopRecvMultiCast()
		{
			receiveMultiCastClient.Close();
			receiveMultiCastClient = null;

			LogUtil.Log ("net", "stopRecvMultiCast");
		}

		// 创建本地LocalHost，这个需要bind udp接收
		private void CreateLocalHost(IPEndPoint endPoint) {
			localHost = new LocalHost (this, endPoint);
			localHost.Start ();
		}

		private void receiveMulti()
		{
			IPEndPoint remoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
			while (true)
			{
				try
				{
					// 关闭receiveUdpClient时此时会产生异常
					byte[] receiveBytes = receiveMultiCastClient.Receive(ref remoteIpEndPoint);
                    UdpPacket packet = UdpPacket.Decode(receiveBytes, 0, receiveBytes.Length);
                    switch ((OpCode)packet.Opcode) {
                        case OpCode.UDP_MULTI_INIT:
                            // 判断是否是自己给主机发送的
                            if (localHost == null)
                            {
                                string ip = NetHelper.GetIpFromAddress(remoteIpEndPoint.Address);
                                List<string> ips = NetHelper.GetLocalIPList();
                                if (ips.Contains(ip))
                                {
                                    // 创建本地host信息
                                    this.CreateLocalHost(remoteIpEndPoint);
                                }
                            }
                            if (this.localHost != null)
                            {
                                // 自己的端已经创建成功
                                UdpSession sess = this.localHost.CreateSession(remoteIpEndPoint);
                                sess.SendPing();
                            }
                            break;
                    }
                    if(this.localHost == null)
                    {
                        continue;
                    }
                    UdpSession session = this.localHost.FindSession(remoteIpEndPoint);
                    if (session == null)
                    {
                        // session 为空 需要创建新的session
                        session = this.localHost.CreateSession(remoteIpEndPoint);
                    }
                    session.OnRecv(packet);
                    KeyValuePair<UdpSession, UdpPacket> pair = new KeyValuePair<UdpSession, UdpPacket>(session, packet);
                    this.AddPacket(pair);
                }
				catch(Exception e)
				{
					LogUtil.Log ("net", "receive multicast break by socket exception");
					break;
				}
			}
			LogUtil.Log ("net", "receive multicast end");
		}

        public UdpSession CreateSession(IPEndPoint endPoint)
        {
            foreach (UdpSession session in this.sessionList)
            {
                if (session.Ip.Equals(NetHelper.GetIpFromAddress(endPoint.Address)))
                {
                    return session;
                }
            }
            UdpSession sess = new UdpSession(this.localHost.SendClient, endPoint);
            this.sessionList.Add(sess);
            return sess;
        }

		public void Destory() {
			StopRecvMultiCast ();
            if(localHost != null)
            {
                this.localHost.Destory();
            }
		}

        // 给所有组播添加进来的host发送消息
        public void Broadcast(UdpPacket packet)
        {
            foreach(UdpSession session in this.sessionList)
            {
                session.Write(packet);
            }
        }

        public void Broadcast(int opcode, Object msg)
        {
            UdpPacket packet = UdpPacket.CreatePacket(opcode, msg);
            this.Broadcast(packet);
        }

        // udp 上下文环境是否准备好
        public bool IsReady()
        {
            return this.localHost != null;
        }

        public UdpSession FindSession(IPEndPoint endPoint)
        {
            foreach(UdpSession session in this.sessionList)
            {
                if(session.Ip.Equals(NetHelper.GetIpFromAddress(endPoint.Address)))
                {
                    return session;
                }
            }
            return null;
        }



    }
}

