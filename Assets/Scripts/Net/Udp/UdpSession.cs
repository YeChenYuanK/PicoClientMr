using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using com.gamestudio.sys;
using com.gamestudio.tank;
using LekeNet.Util;

namespace LekeNet
{
    // udp的 session 管理
    public class UdpSession
    {
        // 对端的信息
        private IPEndPoint endPoint;
        // 发送client
        private UdpClient sendClient;
        // 对端的ip
        private string ip;
        // 写数据缓冲区
        private MemoryStream writeBuff;
        // 上次发送ping的时间戳
        private long lastSendPingTime;
        // 当前的ping值
        private int curPing;

       

        private Dictionary<string, Object> attributeMap;
        // 上次接收到pong的值的时间
        private long lastRecvPongTime = 0;

        public UdpSession(UdpClient sendClient, IPEndPoint endPoint)
        {
            this.sendClient = sendClient;
            this.endPoint = endPoint;
            this.writeBuff = new MemoryStream();
            this.ip = NetHelper.GetIpFromAddress(endPoint.Address);
            this.attributeMap = new Dictionary<string, object>();
        }

        public string Ip
        {
            get { return ip; }
        }

        // 往对端写数据
        public void Write(UdpPacket packet)
        {
            if (packet == null) return;
            lock (writeBuff)
            {
                packet.WriteToStream(writeBuff);
                sendClient.Send(writeBuff.GetBuffer(), (int)writeBuff.Position, endPoint);
                writeBuff.Position = 0;
            }
            //LogUtil.Log("net", "write data to " + endPoint.Address.ToString() + ", opcode : " + packet.Opcode);
        }

        // 往对端写数据
        public void Write(OpCode opcode, Object msg)
        {
            UdpPacket packet = UdpPacket.CreatePacket((int)opcode, msg);
            Write(packet);
        }

        // 往对端写数据
        public void Write(GameOpCode opcode, Object msg)
        {
            UdpPacket packet = UdpPacket.CreatePacket((int)opcode, msg);
            Write(packet);
        }

        // 更新
        public void OnUpdate()
        {
            SendPing();
        }

        // 发送ping请求
        public void SendPing()
        {
            lastSendPingTime = DateHelper.NowMllSec;
            MPing ping = new MPing();
            ping.timestamp = lastSendPingTime;
            this.Write(OpCode.SYS_PING, ping);
        }

        public void OnCreate()
        {
            lastSendPingTime = DateHelper.NowMllSec;
        }

        public void OnRecv(UdpPacket packet)
        {
            if(this.lastSendPingTime == 0)
            {
                lastSendPingTime = DateHelper.NowMllSec;
            }
            if(packet.Opcode == (int)OpCode.SYS_PING)
            {
                //是ping的协议
                MPing ping = packet.Deserialize<MPing>();
                MPong pong = new MPong();
                pong.timestamp = DateHelper.NowMllSec;
                this.Write(OpCode.SYS_PONG, pong);
                this.lastSendPingTime = pong.timestamp;
            } else if(packet.Opcode == (int)OpCode.SYS_PONG)
            {
                MPong pong = packet.Deserialize<MPong>();
                this.curPing = (int)(pong.timestamp - lastSendPingTime);
                this.lastRecvPongTime = DateHelper.NowMllSec;
            }
        }

        public long LastSendPingTime
        {
            get { return this.lastSendPingTime; }
        }

        public long LastRecvPongTime
        {
            get
            {
                return lastRecvPongTime;
            }
        }

        public bool isConn()
        {
            if(this.LastRecvPongTime != 0 && DateHelper.NowMllSec - this.LastRecvPongTime > SysCfg.UDP_CHECK_TIMEOUT)
            {
                return false; 
            }
            return true;
        }

        public void SetAttr(string key, Object data)
        {
            this.attributeMap[key] = data;
        }

        public bool ContainsKey(string key)
        {
            return this.attributeMap.ContainsKey(key);
        }

        public Object GetAttr(string key)
        {
            if(this.attributeMap.ContainsKey(key)) { 
                return this.attributeMap[key];
            }
            return null;
        }

        public void Destory()
        {

        }
    }
}
