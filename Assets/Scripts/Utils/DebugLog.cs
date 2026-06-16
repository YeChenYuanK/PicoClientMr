using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public enum LogLevel
{
    NONE = 0,
    INFO = 1,
    DEBUG = 2
}

public class LogTag
{
    public const string NETWORKING_TAG = "Networking";
    public const string GAME_TAG = "Game";
    public const string LOGIC_TAG = "LOGIC";
}

public class DebugLog
{
    public static string logFileUrl = Application.persistentDataPath + "/log.txt";
    public static List<string> ValidTags = new List<string>();
    private static List<string> CacheLogs = new List<string>();
#if UNITY_EDITOR
    public static bool enabledLogFile = false;
    public static LogLevel logLevel = LogLevel.INFO;
#else 
    public static bool enabledLogFile = true;
    public static LogLevel logLevel = LogLevel.NONE;
#endif
    public static void AddMintorTag(params string[] args)
    {
        if (logFileUrl == "")
        {
            logFileUrl = Application.persistentDataPath + "/log.txt";
        }
        ValidTags.AddRange(args);
    }

    public static void Log(string s, params object[] args)
    {
        logFileUrl = Application.persistentDataPath + "/log.txt";
        if (args.Length > 0)
        {
            s = string.Format(s, args);
        }
        s = $"[{DateUtil.CurDateTimeStr()}] {s}";
        Debug.Log(s);
        File.AppendAllText(logFileUrl, s);
    }

    public static void FlushCacheToFile()
    {
        if (enabledLogFile)
        {
            StringBuilder sb = new StringBuilder(2048);
            foreach (var log in CacheLogs)
            {
                sb.Append(log).Append("\n");
            }
            File.AppendAllText(logFileUrl, sb.ToString());
        }
    }

    public static void LogError(string s, System.Exception e = null)
    {
        if (e != null)
        {
            Debug.LogError(e);
        }
        s = $"[{DateUtil.CurDateTimeStr()}] {s}";
        Debug.LogError(s);
        if (enabledLogFile)
        {
            File.AppendAllText(logFileUrl, s + "\n");
        }
    }
}
