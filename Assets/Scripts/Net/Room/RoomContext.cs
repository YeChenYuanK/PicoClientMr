using System;
using System.Collections.Generic;
using System.Threading;
using com.gamestudio.tank;
using LekeNet;
using LekeNet.Room;

namespace LekeNet
{
	// 房间上下文，主要负责创建房间相关数据
	public class RoomContext : EventDispatcher
	{

        private UdpModeContext udpContext;

        List<LekeNet.Room.Room> roomList;

        private RoomServer roomServer;

        private RoomClient roomClient;

        public void Init()
        {
            udpContext = new UdpModeContext();
            udpContext.Init();
            this.roomList = new List<Room.Room>();
            InitNetHandle();
        }

        private void InitNetHandle()
        {
            udpContext.RegisterNetHandler(OpCode.ROOM_REQUEST, new NetHandler.PacketHandle(OnRoomRequest));
            udpContext.LocalHost.RegisterNetHandler(OpCode.ROOM_REQ_RET, new NetHandler.PacketHandle(OnRoomReqRet));
        }

		// 扫描内网所有的房间
		public void ScanINetRoom()
        {
            RoomRequest request = new RoomRequest();
            request.id = 1; // id 并无实际含义
            UdpPacket packet = UdpPacket.CreatePacket((int)OpCode.ROOM_REQUEST, request);
            udpContext.Multicast(packet);
		}

        // 处理收到的扫描房间的请求
        private void OnRoomRequest(UdpSession session, UdpPacket packet)
        {
            if(this.roomServer != null)
            {
                // roomserve 不为空 说明自己创建了房间
                session.Write(OpCode.ROOM_REQ_RET, this.roomServer.Room.ToMsg());
            }
        }

        private void OnRoomReqRet(UdpSession session, UdpPacket packet)
        {
            RoomHost roomHost = packet.Deserialize<RoomHost>();
            Room.Room room = Room.Room.FromRoomHost(roomHost);
            this.AddRoom(room);
            this.Dispatch(Event.ValueOf(RoomEvent.ROOM_NEW_CREATE, room));
        }

        private void AddRoom(Room.Room room)
        {
            Room.Room existRoom = null;
            foreach(Room.Room r in this.roomList)
            {
                if(r.HostIp.Equals(room.HostIp))
                {
                    // 已经存在这个房间
                    existRoom = r;
                }
            }
            if(existRoom != null)
            {
                this.roomList.Remove(existRoom);
            }
            this.roomList.Add(room);
        }

        public void Destory()
        {
            if(this.udpContext != null)
            {
                this.udpContext.Destory();
            }

            if(this.roomClient != null)
            {
                this.roomClient.Destory();
            }

            if (this.roomServer != null)
            {
                this.roomServer.Destory();
            }
        }

        public LekeNet.Room.Room GetRoomByIp(string ip)
        {
            foreach(LekeNet.Room.Room room in this.roomList)
            {
                if(room.HostIp.Equals(ip))
                {
                    return room;
                }
            }
            return null;
        }

        public LekeNet.Room.Room GetLocalRoom()
        {
            string localIp = this.udpContext.LocalHost.Ip;
            return this.GetRoomByIp(localIp);
        }

        // 创建房间
        public LekeNet.Room.Room CreateRoom(string roomName, string roomPassWd, int roomSize, RoomType roomType)
        {
            // 先验证自己是否已经有房间
            string localIp = this.udpContext.LocalHost.Ip;
            LekeNet.Room.Room room = this.GetRoomByIp(localIp);
            if(room != null)
            {
                return room;
            }

            room = Room.Room.CreateRoom(roomType, localIp, roomName, roomSize);
            this.CreateRoomServer(localIp, room);
            return room;
        }

        public void CreateRoomServer(string ip, LekeNet.Room.Room room)
        {
            // 创建room server
            this.roomServer = new RoomServer(room);
            this.roomServer.Start(ip);

        }

        public RoomClient CreateRoomClient()
        {
            string localIp = this.udpContext.LocalHost.Ip;
            // this.roomClient = new RoomClient(localIp);

            return this.roomClient;
        }

        public void DestoryRoomServer()
        {
            if(this.roomServer != null)
            {
                this.roomServer.Destory();
            }
            this.roomServer = null;
        }

        public void DestoryRoomClient()
        {
            if (this.roomClient != null)
            {
                this.roomClient.Destory();
            }
            this.roomClient = null;
        }

        public List<Room.Room> AllRooms
        {
            get { return this.roomList; }
        }


        public RoomClient RoomClient
        {
            get { return this.roomClient; }
        }

        public RoomServer RoomServer
        {
            get { return this.roomServer; }
        }

        public UdpModeContext UdpContext {
            get { return this.udpContext; }
        }
    }
}

