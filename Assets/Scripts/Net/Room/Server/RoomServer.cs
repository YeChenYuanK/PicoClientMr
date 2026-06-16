using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using com.gamestudio.tank;
using LekeNet.Util;

namespace LekeNet.Room
{
    // 抽象游戏服务器主机，当玩家创建主机是就会创建RoomServer
    // 主机会处理加入、离开、房间内玩家操作等请求
    // 后期做多人PVE的时候 也是这个地方来处理怪物等状态的维护和通知发送
    public class RoomServer : LekeUdp
    {

        private Room curRoom;

        private Rpc rpc;

        private Thread checkThread;
        private volatile bool isCheck;

        public RoomServer(Room room)
        {
            this.curRoom = room;
            this.checkThread = new Thread(new ThreadStart(Check));
            isCheck = true;
            this.checkThread.Start();
        }

        public Room Room
        {
            get { return this.curRoom; }
        }

        // 开启
        public void Start(string ip)
        {
            this.StartBind(ip, SysCfg.UDP_SERVER_PORT);
            this.rpc = new Rpc();
            InitNet();
        }
        
        public void InitNet()
        {
            this.RegisterNetHandler(OpCode.JOIN_ROOM, new NetHandler.PacketHandle(OnJoinRoom));
            this.RegisterNetHandler(OpCode.OPERATION, new NetHandler.PacketHandle(OnOperation));
            this.RegisterNetHandler(OpCode.RPC_REQ, new NetHandler.PacketHandle(OnRpcReq));
            this.RegisterNetHandler(OpCode.MEM_LEAVE, new NetHandler.PacketHandle(this.OnMemLeave));
            this.RegisterNetHandler(OpCode.SCENE_COMPLETE, new NetHandler.PacketHandle(this.OnMemberSceneLoadComplete));
        }

        // 推送给所有成员
        public void PushOperation(RoomOperation operation)
        {
            this.BroadCast(OpCode.OPERATION, operation.ToProto());
        }

        // 推送单个成员
        public void PushOperation(int memId, RoomOperation operation)
        {
            foreach (UdpSession session in SessionList)
            {
                if (session.ContainsKey(RoomConstants.MEM_ID))
                {
                    int mId = (int)session.GetAttr(RoomConstants.MEM_ID);
                    if (mId == memId)
                    {
                        session.Write(OpCode.OPERATION, operation.ToProto());
                    }
                }
            }
        }

        // 推送消息
        public void PushOperation(int memId, int operationId, Object msg)
        {
            RoomOperation roomOper = new RoomOperation();
            roomOper.operId = operationId;
            if (msg == null)
            {
                roomOper.operData = new byte[0];
            }
            else
            {
                roomOper.operData = SerializerUtil.GenerateBytes(msg);
            }
            
            PushOperation(memId, roomOper);
        }

        public void PushError(UdpSession session, RoomErrCode errorCode)
        {
            RoomErrMsg errCode = new RoomErrMsg();
            errCode.errCode = errorCode;
            session.Write(OpCode.ERR_CODE_PUSH, errCode);
        }

        // 推送除了部分成员的其他人
        public void PushOperExclude(RoomOperation operation, params int[] memIds)
        {
            foreach (UdpSession session in SessionList)
            {
                if(session.ContainsKey(RoomConstants.MEM_ID))
                {
                    int memId = (int)session.GetAttr(RoomConstants.MEM_ID);
                    if(!memIds.Contains<int>(memId))
                    {
                        session.Write(OpCode.OPERATION, operation.ToProto());
                    }
                } else
                {
                    session.Write(OpCode.OPERATION, operation.ToProto());
                }
            }
        }

        public void BroadCast(OpCode opcode, Object msg)
        {
            foreach(UdpSession session in SessionList)
            {
                session.Write(opcode, msg);
            }
        }

        private void OnJoinRoom(UdpSession session, UdpPacket packet)
        {
            if(this.curRoom.RoomType == RoomType.SINGLE_PVE)
            {
                // 是单人pve,只能加入玩家自己，其他人不能加入
                PushError(session, RoomErrCode.ROOM_NOT_ALLOW_OTHERS);
            }
            JoinRoom joinRoom = packet.Deserialize<JoinRoom>();
            string ip = session.Ip;
            RoomMember roomMember = this.curRoom.GetMemberByIp(ip);
            if(roomMember != null)
            {
                // 该成员已经加入了该游戏
                return;
            }
            int joinCamp = joinRoom.joinCamp;
            if (this.curRoom.RoomType == RoomType.SINGLE_PVE || this.curRoom.RoomType == RoomType.MULTI_PVE)
            {
                // pve 模式下 默认加入1阵营，怪物都出现在2阵营
                joinCamp = 1;
            } else
            {
                if (SysCfg.IS_AUTO_CAMP)
                {
                    int memSize = this.curRoom.MemberList.Count + 1;
                    // 自动分配阵营
                    if (memSize % 2 == 0)
                    {
                        joinCamp = 2;
                    }
                    else
                    {
                        joinCamp = 1;
                    }
                }
            }

            roomMember = this.curRoom.AddMember(ip, joinRoom.name, joinCamp);

            session.SetAttr(RoomConstants.MEM_ID, roomMember.id);
            session.OnCreate();
            session.Write(OpCode.JOIN_ROOM_RET, this.curRoom.ToMsg());

            this.BroadCast(OpCode.MEM_ADD_NOTIFY, roomMember.ToProto());

            this.Dispatch(Event.ValueOf(RoomEvent.NEW_MEM_ADD_SER, roomMember));
        }

        // 处理玩家操作请求
        private void OnOperation(UdpSession session, UdpPacket packet)
        {
            int memId = 0;
            if(session.ContainsKey(RoomConstants.MEM_ID))
            {
                memId = (int)session.GetAttr(RoomConstants.MEM_ID);
            }
            Operation operation = packet.Deserialize<Operation>();
            RoomOperation roomOper = RoomOperation.ValueOf(operation);
            roomOper.SenderId = memId;
            this.Dispatch(Event.ValueOf(RoomEvent.OPERATION, roomOper));
        }

        private void OnRpcReq(UdpSession session, UdpPacket packet)
        {
            if(!session.ContainsKey(RoomConstants.MEM_ID))
            {
                return;
            }
            int memId = (int)session.GetAttr(RoomConstants.MEM_ID);
            RPCReq rpcReq = packet.Deserialize<RPCReq>();
            Rpc.RpcCall rpcCall = this.rpc.GetRpcCall(rpcReq.rpcName);
            if(rpcCall != null)
            {
                Object result = rpcCall(memId, rpcReq.data);
                RPCResp rpcResp = new RPCResp();
                rpcResp.rpcName = rpcReq.rpcName;
                if(result != null)
                {
                    rpcResp.data = SerializerUtil.GenerateBytes(result);

                    LogUtil.Log("net", "rpc resp : " + LogUtil.GenByteString(rpcResp.data));
                }

                session.Write(OpCode.RPC_RET, rpcResp);
            }
        }

        public void RegisterRpcCall(string rpcName, Rpc.RpcCall rpcCall)
        {
            this.rpc.AddRpcCall(rpcName, rpcCall);
        }

        private void Check()
        {
            while (isCheck)
            {
                Thread.Sleep(1000);
                if(this.curRoom.RoomStatus != RoomStatus.WAITING_PLAYER)
                {
                    // 在房间状态为等待加载时不处理 网络超时
                    long curTime = DateHelper.NowMllSec;
                    List<UdpSession> removeList = new List<UdpSession>();
                    foreach (UdpSession session in this.SessionList)
                    {
                        if (curTime - session.LastSendPingTime > SysCfg.UDP_CHECK_TIMEOUT)
                        {
                            // 该玩家已经消失
                            if (session.ContainsKey(RoomConstants.MEM_ID))
                            {
                                int memId = (int)session.GetAttr(RoomConstants.MEM_ID);
                                if (memId > 0)
                                {
                                    RoomMember roomMember = this.curRoom.RemoveById(memId);

                                    if (roomMember != null)
                                    {

                                        LogUtil.Log("net", "session check close memId : " + memId + " time : " + curTime + " " + session.LastSendPingTime);
                                        this.Dispatch(Event.ValueOf(RoomEvent.ROOM_MEMBER_LEAVE, memId));
                                        // 通知其他客户端
                                        this.BroadCast(OpCode.MEM_LEAVE_NOTIFY, roomMember.ToProto());
                                    }
                                }
                            }
                            removeList.Add(session);
                        }
                    }

                    foreach (UdpSession session in removeList)
                    {
                        session.Destory();
                        this.RemoveSession(session);
                    }
                }
                
                if(this.curRoom.RoomStatus == RoomStatus.WAITING_PLAYER)
                {
                    // check 所有成员的场景加载状态
                    bool isComplete = true;
                    foreach (RoomMember roomMember in this.curRoom.MemberList)
                    {
                        if (!roomMember.isSceneLoadComplete)
                        {
                            isComplete = false;
                        }
                    }

                    if(isComplete)
                    {
                        this.curRoom.ChangeRoomStatus(RoomStatus.FIGHTING);
                        this.Dispatch(Event.ValueOf(RoomEvent.FIGHT_START_COMPLETE, null));
                    }
                }
            }
        }

        public override void Destory()
        {
            this.isCheck = false;
            base.Destory();
        }

        private void OnMemLeave(UdpSession session, UdpPacket packet)
        {
            // 该玩家已经消失
            if (session.ContainsKey(RoomConstants.MEM_ID))
            {
                int memId = (int)session.GetAttr(RoomConstants.MEM_ID);
                if (memId > 0)
                {
                    RoomMember roomMember = this.curRoom.RemoveById(memId);

                    if (roomMember != null)
                    {
                        this.Dispatch(Event.ValueOf(RoomEvent.ROOM_MEMBER_LEAVE, memId));
                        // 通知其他客户端
                        this.BroadCast(OpCode.MEM_LEAVE_NOTIFY, roomMember.ToProto());
                        session.Destory();
                        this.SessionList.Remove(session);
                    }
                }
            }
        }


        public void StartFight()
        {
            this.Room.ChangeRoomStatus(RoomStatus.WAITING_PLAYER); 
            // 单人pve 不需要同步通知
            this.BroadCast(OpCode.FIGHT_START, this.Room.ToMsg());
        }

        private void OnMemberSceneLoadComplete(UdpSession session, UdpPacket packet)
        {
            if (session.ContainsKey(RoomConstants.MEM_ID))
            {
                int memId = (int)session.GetAttr(RoomConstants.MEM_ID);
                RoomMember roomMember = this.curRoom.GetMemberById(memId);
                if (roomMember != null)
                {
                    roomMember.isSceneLoadComplete = true;
                }
            }
        }
    }
}
