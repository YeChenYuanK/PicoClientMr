using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace LekeNet
{
    class TcpSocket
    {
        public static Socket Connect(string server, int port)
        {
            Socket s = null;
            IPEndPoint ipe = new IPEndPoint(IPAddress.Parse(server), port);
            Socket tempSocket =
                new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            tempSocket.Connect(ipe);
            
            if (tempSocket.Connected)
            {
                LogUtil.Log("net", "socket connect success");
                s = tempSocket;
            }

            return s;
        }

    }
}
