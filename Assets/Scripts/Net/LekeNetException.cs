using System;

namespace LekeNet
{
	public enum LekeExceptionCode {
        // UDP 网络环境无法初始化 可能原因（组播地址被占用）
        UDP_NET_WORK_FAIL,

    }

	public class LekeNetException : Exception
	{
		

		public LekeNetException (LekeExceptionCode exceptionCode)
		{
			this.exceptionCode = exceptionCode;
		}
		// 异常编号
		private LekeExceptionCode exceptionCode ;

		public LekeExceptionCode ExceptionCode {
			get { return this.exceptionCode; }
		}

	}

}

