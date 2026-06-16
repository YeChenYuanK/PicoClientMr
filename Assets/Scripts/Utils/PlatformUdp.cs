using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.XR;

public class PlatformUdp : MonoBehaviour
{
    private UdpClient recvUdp;
    private UdpClient sendUdp;
    private void Awake()
    {
#if !UNITY_ANDROID
        // 平台 UDP 控制(端口 44444 收 close、退出拉起 Pico 启动器)仅用于 Android/Pico,
        // 非 Android 工程(BuildPC)整体禁用,避免 FocusAndroidUtil 的 AndroidJavaClass 崩溃。
        enabled = false;
        return;
#else
        DontDestroyOnLoad(this.gameObject);
#endif
    }
    void Start()
    {
        try
        {
            this.recvUdp = new UdpClient(new IPEndPoint(IPAddress.Any, 44444));
        }
        catch (System.Net.Sockets.SocketException e)
        {
            UnityEngine.Debug.LogError(e);
            // udp 端口绑定失败
            UnityEngine.Debug.Log($"网络UDP 端口绑定失败 ： {44444}");
        }

        this.sendUdp = new UdpClient();
    }

    IPEndPoint platformEndpoint;

    void Update()
    {
        if (this.recvUdp == null) return;
        while (this.recvUdp.Available > 0)
        {
            byte[] data = recvUdp.Receive(ref platformEndpoint);
            try
            {
                string cmdStr = System.Text.Encoding.UTF8.GetString(data);
                if (cmdStr == "close")
                {
                    UnityEngine.Debug.Log($"收到平台udp消息 结束游戏");
                    Application.Quit();
                }
                else
                {
                    Debug.Log("收到平台udp消息==========" + cmdStr);
                    Application.Quit();
                }
            }catch(System.Exception e)
            {
                Debug.Log("收到平台udp消息错误==========" + e.Message);
                Application.Quit();
            }
        }
    }

    private void OnApplicationQuit()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        Debug.Log("[EXIT-DIAG] OnApplicationQuit FIRED (real quit, process is terminating)");
        Debug.Log("ControlClient DisConnect");

        // ControlCenter.Instance.StopServer();
        FocusAndroidUtil.StartLauncher();
#endif
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        Debug.Log("[EXIT-DIAG] OnApplicationPause pause=" + pauseStatus + " (true=backgrounded, NOT a quit -> may go cached)");
    }

}
