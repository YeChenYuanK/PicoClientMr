using System;

namespace LekeNet
{
	// 网络通信模式
	public enum NetMode
	{
		UDP = 1,
		KCP = 2,
		TCP_BRIDGE = 3
	}

	// 广播通知的
	public enum NetMultuCastNotify {
		NEW_END_JOIN = 1
	}

}

