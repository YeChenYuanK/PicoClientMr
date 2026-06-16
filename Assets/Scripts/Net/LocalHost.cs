using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;


namespace LekeNet
{
    public class LocalHost : LekeUdp
    {
        private IPEndPoint localEndPoint;

        private string localIp;

        private UdpModeContext udpContext;

        public LocalHost(UdpModeContext udpContext, IPEndPoint endPoint)
        {
            this.udpContext = udpContext;
            this.localEndPoint = endPoint;
            this.localIp = NetHelper.GetIpFromAddress(endPoint.Address);
            
        }

        public string Ip
        {
            get { return this.localIp; }
        }

        public void Start()
        {
            base.StartBind(this.localIp, SysCfg.UDP_PORT);
        }
        

    }


}
