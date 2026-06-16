using System;

namespace LekeNet
{
	// 相关配置
	public class SysCfg
	{
		public SysCfg ()
		{
		}

		// 需要加入的组播地址
		public static string UDP_MULTICAST_GROUP = "234.5.6.7";

		// 需要加入的组播地址的端口
		public static int UDP_MULTICAST_PORT = 9997;

		// UDP TTL
		public static short UDP_TTL = 128;

        // UDP 客户机端口
        public static int UDP_PORT = 9996;

        // UDP 服务器端口
        public static int UDP_SERVER_PORT = 9995;

        // UDP room client port
        public static int UDP_CLIENT_PORT = 9994;

        // 检测Udp 失效时间判定条件
        public static int UDP_CHECK_TIMEOUT = 5000;

        // 是否自动分配阵营
        public static bool IS_AUTO_CAMP = true;

        // 内网测试服务器50ms延迟
        public static int SERVER_DELAY = 50;


        //TCP服务器地址
        public static string TCP_SERVER_IP = "203.195.209.149";
        //TCP服务器端口
        public static int TCP_SERVER_PORT = 9595;


    }
}

