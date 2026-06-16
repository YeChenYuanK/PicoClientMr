using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class ProtoCmd
{
    public int CmdId = 0;
    public string ServerIp = "";
    public string SceneSize = "";
}

public class BroadcastAskServer : MonoBehaviour
{
    public string ResultServerIp = "";

    void Start()
    {
               
    }

    private IPEndPoint remoteEndPoint;

    void Update()
    {
        if (this.recvUdp == null) return;
        while (this.recvUdp.Available > 0)
        {
            byte[] data = recvUdp.Receive(ref remoteEndPoint);
            string protostr = System.Text.Encoding.UTF8.GetString(data);
            UnityEngine.Debug.Log($"收到平台udp消息 {protostr}");
            ProtoCmd proto = JsonConvert.DeserializeObject<ProtoCmd>(protostr);
            this.OnProto(proto);
        }
    }

    private void OnProto(ProtoCmd proto)
    {
        if(proto.CmdId == 10002)
        {
            // 收到服务器IP
            ResultServerIp = proto.ServerIp;
            SceneDefine.CurGameStartSceneSizeParam = proto.SceneSize;
        }
    }

    /// <summary>
    /// 组播地址endpoint
    /// </summary>
    private IPEndPoint multiCastEndPoint;

    /// <summary>
    /// 组播协议发送端
    /// </summary>
    private UdpClient multicastUdpClient;

    private UdpClient recvUdp;

    public void Init()
    {
        this.multicastUdpClient = new UdpClient(new IPEndPoint(IPAddress.Any, 0));
        this.multiCastEndPoint = new IPEndPoint(IPAddress.Parse("255.255.255.255"), 9111);
        this.recvUdp = new UdpClient(new IPEndPoint(IPAddress.Any, 9112));
    }

    public void AskServer()
    {
        ProtoCmd cmd = new ProtoCmd()
        {
            CmdId = 10001,
            ServerIp = "",

        };
        string protoStr = JsonConvert.SerializeObject(cmd);
        byte[] data = System.Text.Encoding.UTF8.GetBytes(protoStr);
        this.multicastUdpClient.Send(data, data.Length, multiCastEndPoint);
        Debug.Log("发送广播消息 : " + protoStr + " " + data.Length);
    }
}
