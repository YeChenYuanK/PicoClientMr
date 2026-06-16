using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.gamestudio.tank;

namespace LekeNet.Room
{
    public class Room : EventDispatcher
    {
        // 房间的房主的IP 作为每个房间的标识
        private string hostIp;

        // 当前房间状态
        private RoomStatus roomStatus;

        // 房间名字
        private string roomName;

        // 房间密码
        private string password = "";

        // 成员列表
        private List<RoomMember> memberList;

        // 房间大小
        private int size;

        // 房间类型
        private RoomType roomType;

        public Room()
        {
            roomStatus = RoomStatus.PREPARE;
            memberList = new List<RoomMember>();
            size = 5;
        }

        // 创建房间
        public static Room CreateRoom(RoomType roomType, string ip, string roomName, int limitSize)
        {
            Room room = new Room();
            room.roomType = roomType;
            room.roomName = roomName;
            room.size = limitSize;
            room.hostIp = ip;
            return room;
        }

        public List<RoomMember> MemberList
        {
            get { return memberList; }
        }

        public string HostIp
        {
            get { return hostIp; }
            set { hostIp = value; }
        }

        public string PassWord
        {
            get { return password; }
            set { password = value; }
        }

        public string RoomName
        {
            get { return roomName; }
            set { roomName = value; }
        }

        public RoomType RoomType
        {
            get
            {
                return roomType;
            }

            set
            {
                roomType = value;
            }
        }

        public RoomStatus RoomStatus { get { return this.roomStatus; } }

        public void ChangeRoomStatus(RoomStatus roomStatus)
        {
            this.roomStatus = roomStatus;
        }

        public void StartFight()
        {
            this.roomStatus = RoomStatus.FIGHTING;
        }

        public bool IsNeedPassWord()
        {
            return !"".Equals(password);
        }

        public RoomHost ToMsg()
        {
            RoomHost roomHost = new RoomHost();
            roomHost.ip = hostIp;
            roomHost.isNeedPassWord = this.IsNeedPassWord();
            roomHost.name = this.roomName;
            roomHost.roomType = this.roomType;
            roomHost.roomStatus = this.roomStatus;
            roomHost.roomSize = this.size;
            foreach (RoomMember member in this.memberList)
            {
                roomHost.members.Add(member.ToProto());
            }

            return roomHost;
        }

        public bool IsFull()
        {
            return this.memberList.Count >= this.size;
        }

        public RoomMember AddMember(string fromIp, string name, int camp)
        {
            RoomMember existMember = null;
            foreach (RoomMember rm in this.memberList)
            {
                if (rm.ip.Equals(fromIp))
                {
                    existMember = rm;
                    break;
                }
            }

            if (existMember != null)
            {
                this.memberList.Remove(existMember);
            }

            RoomMember roomMember = new RoomMember();
            roomMember.id = FindEmptyId();
            roomMember.ip = fromIp;
            roomMember.camp = camp;
            if (name == null || name.Equals(""))
            {
                roomMember.playerName = "player" + (this.memberList.Count + 1);
            }
            else
            {
                roomMember.playerName = name;
            }

            this.memberList.Add(roomMember);

            RefreshCampIndex();

            return roomMember;
        }

        private void RefreshCampIndex()
        {
            Dictionary<int, int> campIndexDict = new Dictionary<int, int>();
            foreach(RoomMember roomMember in this.memberList)
            {
                if(campIndexDict.ContainsKey(roomMember.camp))
                {
                    campIndexDict[roomMember.camp] = campIndexDict[roomMember.camp] + 1;
                } else
                {
                    campIndexDict[roomMember.camp] = 1;
                }

                roomMember.campIndex = campIndexDict[roomMember.camp];
            }
        }

        private int FindEmptyId()
        {
            if(memberList.Count == 0)
            {
                return 1;
            }
            memberList.Sort();
            for(int i = 0; i < memberList.Count; i++)
            {
                RoomMember rm = memberList[i];
                if(rm.id != i + 1)
                {
                    return i + 1;
                }
            }

            return memberList.Count + 1;
        }

        public static Room FromRoomHost(RoomHost roomHost)
        {
            Room room = new Room();
            room.HostIp = roomHost.ip;
            room.RoomName = roomHost.name;
            room.roomStatus = roomHost.roomStatus;
            room.roomType = roomHost.roomType;
            room.size = roomHost.roomSize;
            foreach (RoomMem roomMem in roomHost.members)
            {
                room.AddMember(roomMem.ip, roomMem.name, roomMem.camp);
            }
            return room;
        }

        public void AddMember(RoomMember roomMember)
        {
            RoomMember existMember = null;
            foreach (RoomMember rm in this.memberList)
            {
                if (rm.ip.Equals(roomMember.ip))
                {
                    existMember = rm;
                    break;
                }
            }

            if (existMember != null)
            {
                this.memberList.Remove(existMember);
            }

            this.memberList.Add(roomMember);

            RefreshCampIndex();
        }

        public RoomMember GetMemberByIp(string ip)
        {
            foreach (RoomMember rm in this.memberList)
            {
                if (rm.ip.Equals(ip))
                {
                    return rm;
                }
            }
            return null;
        }

        public RoomMember GetMemberById(int id)
        {
            foreach (RoomMember rm in this.memberList)
            {
                if (rm.id == id)
                {
                    return rm;
                }
            }
            return null;
        }

        public RoomMember RemoveById(int id)
        {
            RoomMember existMember = null;
            foreach (RoomMember rm in this.memberList)
            {
                if (rm.id == id)
                {
                    existMember = rm;
                    break;
                }
            }

            if (existMember != null)
            {
                this.memberList.Remove(existMember);

                RefreshCampIndex();
                return existMember;
            }

            return null;
        }

        public string RoomTypeStr
        {
            get
            {
                switch(this.roomType)
                {
                    case RoomType.SINGLE_PVE:
                        return "单人pve";
                    case RoomType.MULTI_PVE:
                        return "多人pve";
                    case RoomType.PVP:
                        return "PVP";
                }
                return "无";
            }
        }

        public int Size
        {
            get
            {
                return size;
            }

            set
            {
                size = value;
            }
        }


        public string RoomStatusStr
        {
            get
            {
                switch (this.roomStatus)
                {
                    case RoomStatus.FIGHTING:
                        return "战斗中";
                    case RoomStatus.PREPARE:
                        return "准备中";
                }
                return "无";
            }
        }

        public override string ToString()
        {
            return "RoomType : " + RoomTypeStr + "， HostIp : " + hostIp + ", size : " + size + " , RoomStatus : " + RoomStatusStr;
        }
    }
}
