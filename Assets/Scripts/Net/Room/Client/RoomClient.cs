using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using com.gamestudio.room;
using com.gamestudio.sys;
using com.gamestudio.tank;
using LekeNet.Util;
using UnityEngine.SceneManagement;

namespace LekeNet.Room
{
    public class RoomClient : LekeUdp1
    {

        private Room curRoom;

        //private UdpSession1 clientSession;

        private Rpc rpc;

        private Thread checkThread;

        private volatile bool isCheck;

        private TcpContext tcpContext;

        public RoomClient(string ipAddress, int port, BuildUdpSession buildSession)
        {
            this.InitNetHandle();
            this.checkThread = new Thread(new ThreadStart(Check));
            isCheck = true;
            this.checkThread.Start();
            this.Init(ipAddress, port, buildSession);
        }

        public void InitNetHandle()
        {
            this.RegisterUdpNetHandler((int)GameOpCode.OPERATION, new UdpNetHandler.PacketHandle(OnOperation));
            this.RegisterUdpNetHandler((int)GameOpCode.BUILD_UDP_SESSION, new UdpNetHandler.PacketHandle(OnUdpSessionCreateSuc));
            this.RegisterUdpNetHandler((int)GameOpCode.SYS_OPERATION, new UdpNetHandler.PacketHandle(OnSysOperation));
            this.RegisterUdpNetHandler((int)GameOpCode.KICKED_OFF,new UdpNetHandler.PacketHandle(OnKickOff));
        }

        private void OnMemAdd(UdpSession session, UdpPacket packet)
        {
            RoomMem roomMem = packet.Deserialize<RoomMem>();
            RoomMember roomMember = RoomMember.FromProto(roomMem);
            this.curRoom.AddMember(roomMember);

            this.Dispatch(Event.ValueOf(RoomEvent.ROOM_MEMBER_ADD, roomMember));
        }

        // 推送消息
        public void PushOperation(int operationId, Object msg)
        {
            RoomOper oper = new RoomOper();
            oper.operid = operationId;
            if (msg == null)
            {
                oper.data = new byte[0];
            } else
            {
                oper.data = SerializerUtil.GenerateBytes(msg);
            }

            if(this.UdpSession != null)
            {
                this.UdpSession.Write(GameOpCode.OPERATION, oper);
            }
        }

        // 接受游戏操作推送消息
        private void OnOperation(UdpSession session, UdpPacket packet)
        {
            this.Dispatch(Event.ValueOf(RoomEvent.OPERATION, RoomOperation.ValueOf(packet.Deserialize<Operation>())));
        }

        // 接受系统操作推送消息
        private void OnSysOperation(UdpSession session, UdpPacket packet)
        {
            this.Dispatch(Event.ValueOf(RoomEvent.SYS_OPERATION, RoomOperation.ValueOf(packet.Deserialize<Operation>())));
        }

        /// <summary>
        /// 踢掉玩家
        /// </summary>
        /// <param name="session"></param>
        /// <param name="packet"></param>
        private void OnKickOff(UdpSession session, UdpPacket packet)
        {
            this.Dispatch(Event.ValueOf(RoomEvent.KICK_OFF, RoomOperation.ValueOf(packet.Deserialize<Operation>())));
        }

        private void OnUdpSessionCreateSuc(UdpSession session, UdpPacket packet)
        {
            RoomInfo roomInfo = packet.Deserialize<RoomInfo>();
            // 根据房间地图Id 加载场景 现在都是默认的值
            MainFrameCall.Instance.AddCall(new MainFrameCall.FrameCall(LoadMap), roomInfo);
        }

        private System.Object LoadMap(System.Object para)
        {
            com.gamestudio.room.RoomInfo roomInfo = para as com.gamestudio.room.RoomInfo;
            if(roomInfo.mapLevel == 1) {
                SceneManager.LoadScene("BattleRoyale");
            } else
            {
                SceneManager.LoadScene("Dev_BattleRoyale");
            }
            UnityEngine.Debug.Log("开始加载场景");
            return null;
        }

        public void SendRpcReqToServer(string rpcName, Object data, Rpc.RpcCallBack callBack)
        {
            if(this.UdpSession != null)
            {
                RPCReq rpcReq = new RPCReq();
                rpcReq.rpcName = rpcName;
                rpcReq.data = SerializerUtil.GenerateBytes(data);
                //this.UdpSession1.Write(OpCode.RPC_REQ, rpcReq);
            }

            this.rpc.AddRpcCallBack(rpcName, callBack);
        }

        private void OnRpcRet(UdpSession session, UdpPacket packet)
        {
            RPCResp rpcResp = packet.Deserialize<RPCResp>();
            Rpc.RpcCallBack callBack = this.rpc.GetRpcCallBack(rpcResp.rpcName);
            if (callBack != null)
            {
                LogUtil.Log("net", "recv rpc resp : " + LogUtil.GenByteString(rpcResp.data));            
                callBack(rpcResp.data);
            }
            
        }

        private void Check()
        {
            while(isCheck)
            {
                Thread.Sleep(100);

                if(this.UdpSession != null)
                {
                    this.UdpSession.SendPing();

                    if (!this.UdpSession.isConn())
                    {
                        // 已经失去与服务器的连接
                        this.Dispatch(Event.ValueOf(RoomEvent.LOST_SERVER_CONNECTION, null));
                    }
                } 
            }
        }

        public override void Destory()
        {
            this.isCheck = false;
            base.Destory();
        }

        // 主动退出与服务器的链接
        public void Disconnect()
        {
            if (this.UdpSession != null)
            {
                //this.UdpSession1.Write(OpCode.MEM_LEAVE, null);
            }
        }

        private void OnMemLeave(UdpSession session, UdpPacket packet)
        {
            RoomMem roomMem = packet.Deserialize<RoomMem>();
            RoomMember roomMember = this.curRoom.RemoveById(roomMem.id);
            this.Dispatch(Event.ValueOf(RoomEvent.ROOM_MEMBER_LEAVE, roomMember));
        }

        private void OnRoomStart(UdpSession session, UdpPacket packet)
        {
            this.Dispatch(Event.ValueOf(RoomEvent.ROOM_STATUS_CHANGE, null));
        }

        private void OnRoomStartComplete(UdpSession session, UdpPacket packet)
        {
            RoomHost roomHost = packet.Deserialize<RoomHost>();
            this.curRoom = Room.FromRoomHost(roomHost);
            this.Dispatch(Event.ValueOf(RoomEvent.ROOM_STATUS_CHANGE, this.curRoom));
        }

        public void NotifySceneComplete()
        {
            if(this.UdpSession != null)
            {
                //this.UdpSession1.Write(OpCode.SCENE_COMPLETE, null);
            }
        }
    }

    
}
