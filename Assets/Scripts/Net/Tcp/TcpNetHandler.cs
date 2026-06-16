using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using com.gamestudio.tank;

namespace LekeNet
{
    public class TcpNetHandler:EventDispatcher
    {
        private BlockingQueue<KeyValuePair<TcpSession, TcpPacket>> recvQueue;

        private Thread handleThread;

        public delegate void TcpPacketHandle(TcpSession tcpSession, TcpPacket packet);

        private volatile bool isRunning = false;

        private Dictionary<int, TcpPacketHandle> packetDict = new Dictionary<int, TcpPacketHandle>();

        public void StartNetHandler()
        {
            if (recvQueue != null)
            {
                recvQueue.Reset();
            }
            else
            {
                recvQueue = new BlockingQueue<KeyValuePair<TcpSession, TcpPacket>>();
            }
            isRunning = true;
            handleThread = new Thread(new ThreadStart(OnPacketHandle));
            handleThread.Name = "consum thread";
            handleThread.Start();
        }

        public void AddPacket(TcpSession tcpSession, TcpPacket packet)
        {
            this.recvQueue.Enqueue(new KeyValuePair<TcpSession, TcpPacket>(tcpSession, packet));
        }

        public void RegisterNetHandler(int opcode, TcpPacketHandle packetHandle)
        {
            this.packetDict[opcode] = packetHandle;
        }

        public void RegisterNetHandler(OpCode opcode, TcpPacketHandle packetHandle)
        {
            this.packetDict[(int)opcode] = packetHandle;
        }

        public void OnPacketHandle()
        {
            while (isRunning)
            {
                KeyValuePair<TcpSession, TcpPacket> pair = this.recvQueue.Dequeue();
                TcpPacket packet = pair.Value;
                TcpSession tcpSession = pair.Key;
                if (packet != null && tcpSession != null)
                {
                    if (this.packetDict.ContainsKey(packet.OpCode))
                    {
                        try
                        {
                            this.packetDict[packet.OpCode](tcpSession, packet);
                        }
                        catch (Exception e)
                        {
                            LogUtil.Log("net", "stack trace : " + e.StackTrace);
                            LogUtil.Log("net", "tcp handle fail opcode : " + packet.OpCode + " e: " + e.Message + " target : " + this.GetType());
                        }
                    }
                }
            }
        }

        public virtual void Destory()
        {
            this.isRunning = false;
            this.recvQueue.Close();
        }
    }
}
