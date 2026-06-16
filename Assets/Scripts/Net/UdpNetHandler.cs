using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using com.gamestudio.tank;

namespace LekeNet
{

    public class UdpNetHandler : EventDispatcher
    {
        private BlockingQueue<KeyValuePair<UdpSession, UdpPacket>> recvQueue;

        private Thread handleThread;

        public delegate void PacketHandle(UdpSession session, UdpPacket packet);

        private volatile bool isRunning = false;

        private Dictionary<int, PacketHandle> packetDict = new Dictionary<int, PacketHandle>();

        public void StartUdpNetHandler()
        {
            if (recvQueue != null)
            {
                recvQueue.Reset();
            }
            else
            {
                recvQueue = new BlockingQueue<KeyValuePair<UdpSession, UdpPacket>>();
            }
            isRunning = true;
            handleThread = new Thread(new ThreadStart(OnPacketHandle));
            handleThread.Name = "consum thread";
            handleThread.Start();
        }

        public void AddPacket(KeyValuePair<UdpSession, UdpPacket> pair)
        {
            this.recvQueue.Enqueue(pair);
        }

        public void RegisterUdpNetHandler(int opcode, PacketHandle packetHandle)
        {
            this.packetDict[opcode] = packetHandle;
        }

        public void RegisterUdpNetHandler(OpCode opcode, PacketHandle packetHandle)
        {
            this.packetDict[(int)opcode] = packetHandle;
        }

        public void OnPacketHandle()
        {
            while (isRunning)
            {
                KeyValuePair<UdpSession, UdpPacket> pair = this.recvQueue.Dequeue();
                UdpPacket packet = pair.Value;
                UdpSession session = pair.Key;
                if (packet != null && session != null)
                {
                    if (this.packetDict.ContainsKey(packet.Opcode))
                    {
                        long stime = DateUtil.NowMllSec;
                        try
                        {
                            this.packetDict[packet.Opcode](session, packet);
                        }
                        catch (Exception e)
                        {
                            LogUtil.Log("net", "stack trace : " + e.StackTrace);
                            LogUtil.Log("net", "handle fail opcode : " + packet.Opcode + " e: " + e.Message + " target : " + this.GetType());
                        } finally
                        {
                            long etime = DateUtil.NowMllSec;
                            long diff = etime - stime;
                            if(diff > 15)
                            {
                                UnityEngine.Debug.Log("handle time : " + (etime - stime));
                            }

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
