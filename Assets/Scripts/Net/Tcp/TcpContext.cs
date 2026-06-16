using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;


namespace LekeNet
{
    /**
    * 网络包管理
    */
    public class TcpContext : TcpNetHandler
    {
        public TcpContext()
        {
        }

        private Socket socket;

        private TcpSession tcpSession;

        private MemoryStream readBuff;

        private static TcpContext instance = null;

        public static TcpContext Instance
        {
            get {
                if (instance == null)
                {
                    instance = new TcpContext();
                }
                return instance;
            }
        }

        public TcpSession TcpSession
        {
            get
            {
                return tcpSession;
            }

            set
            {
                tcpSession = value;
            }
        }

        /**
         * 初始化
         */
        public bool Init(string ip, int port)
        {
            socket = TcpSocket.Connect(ip, port);
            if(socket != null)
            {
                TcpSession = new TcpSession(socket);
            } else
            {
                return false;
            }
            this.StartNetHandler();
            //异步等待数据
            this.StartRecive();
            return true;
        }

        public void StartRecive()
        {
            readBuff = new MemoryStream();
            AsyncCallback callBack = new AsyncCallback(ReciveDataSync);
            result = socket.BeginReceive(bytes, 0, bytes.Length, SocketFlags.None, callBack, socket);
        }

        public void ReciveDataSync(IAsyncResult ar)
        {
            int bytesRead = 0;
            try
            {
                //读取数据
                bytesRead = socket.EndReceive(ar);
                ReceiveDataCallback(bytes, bytesRead);
                AsyncCallback callBack = new AsyncCallback(ReciveDataSync);
                socket.BeginReceive(bytes, 0, bytes.Length, SocketFlags.None, callBack, socket);
            }
            catch (Exception ex)
            {
                LogUtil.Log("net", "tcp socket disconnect");
            }
        }

        private byte[] bytes = new byte[204800];

        private IAsyncResult result;

        private void ReceiveData()
        {
            //Debug.Log("receiveData");
            while (true)
            {
                try
                {
                    if (!socket.Connected)
                    {
                        LogUtil.Log("net", "KickOut");
                        break;
                    }
                    int bytesRead = socket.Receive(bytes);
                    ReceiveDataCallback(bytes, bytesRead);
                }
                catch (Exception ex)
                {

                }
            }
        }

        public override void Destory()
        {
            base.Destory();
            if (socket != null)
            {
                socket.Close();
                LogUtil.Log("net", "Socket Close");
            }
        }

        //receive call back
        private void ReceiveDataCallback(byte[] bs, int bytesRead)
        {
            LogUtil.Log("net", "data recv , " + bytesRead);
            try
            {
                if (bytesRead > 0)
                {
                    readBuff.Write(bs, 0, bytesRead);

                    //尝试解析协议
                    while (readBuff.Position >= TcpPacket.HEAD_SIZE)
                    {
                        TcpPacket packet = Decode();
                        if (packet != null)
                        {
                            this.AddPacket(this.tcpSession, packet);
                            LogUtil.Log("net", "data decode packet : " + packet.OpCode);
                        }
                        else
                        {
                            LogUtil.Log("net", "data decode fail , ");
                            break;
                        }
                    }
                }
                else
                {
                    LogUtil.Log("net", "read 0");
                }
            }
            catch (Exception e)
            {
                LogUtil.Log("net", e.ToString());
            }
        }

        private TcpPacket Decode()
        {
            int oldPos = (int)readBuff.Position;
            if (oldPos >= TcpPacket.HEAD_SIZE)
            {
                readBuff.Position = 0;
                BinaryReader binaryReader = new BinaryReader(readBuff);
                int opCode = ConvertEndianHelper.CheckEndian(binaryReader.ReadInt32());
                int buffLen = ConvertEndianHelper.CheckEndian(binaryReader.ReadInt32());
                binaryReader.ReadInt32();
                binaryReader.ReadInt32();

                if ((oldPos - (int)readBuff.Position) >= buffLen)
                {
                    byte[] bytes = binaryReader.ReadBytes(buffLen);

                    TcpPacket packet = TcpPacket.CreatePacket(opCode, bytes);

                    if (oldPos >= (int)readBuff.Position)
                    {
                        int pos = (int)readBuff.Position;
                        readBuff.Position = 0;
                        if (oldPos > pos)
                        {
                            readBuff.Write(readBuff.GetBuffer(), pos, oldPos - pos);
                        }
                    }

                    return packet;
                }
                else
                {
                    readBuff.Position = oldPos;
                }
            }

            return null;
        }
    }

}