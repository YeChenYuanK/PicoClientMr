using System;
using System.Collections.Generic;

namespace LekeNet
{
	public class LogUtil
	{
		public LogUtil ()
		{
		}

		private static List<string> modeNameList = new List<string>();

		public delegate void LogInfo(string msg);

		private static LogInfo logInfo = null;

		private static void logConsle(string msg) {
			Console.WriteLine (msg.ToLower());
		}

		public static void openLogMode(string mode) {
			modeNameList.Add(mode);
		}

        public static void SetLogInfo(LogInfo l)
        {
            logInfo = l;
        }

		public static void Log(string modeName, string logMsg) {
			if (!modeNameList.Contains (modeName.ToLower())) {
				return;
			}
			if (logInfo == null) {
				logInfo = new LogInfo (logConsle);
			}
			if (logInfo != null) {
				logInfo (logMsg);
			}
		}

        public static string GenByteString(byte[] bytes)
        {
            if(bytes == null || bytes.Length == 0)
            {
                return " empty ";
            }
            string s = "";
            foreach(byte b in bytes) {
                s += b + ",";
            }
            return s;
        }
	}
}

