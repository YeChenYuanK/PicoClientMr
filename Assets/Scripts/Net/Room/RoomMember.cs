using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using com.gamestudio.tank;

namespace LekeNet.Room
{
    // 玩家成员
    public class RoomMember : IComparable<RoomMember>
    {
        // 玩家名称
        public string playerName;

        // 所在阵营
        public int camp;

        // 玩家机器的ip
        public string ip;

        // 是否在线 
        public bool isOnline;

        public int id;

        // 在阵营里面是第几个
        public int campIndex;

        // 场景是否加载完毕
        public bool isSceneLoadComplete;

        public RoomMem ToProto()
        {
            RoomMem roomMem = new RoomMem();
            roomMem.name = playerName;
            roomMem.camp = camp;
            roomMem.ip = ip;
            roomMem.id = this.id;
            roomMem.campIndex = campIndex;
            return roomMem;
        }


        public static RoomMember FromProto(RoomMem roomMem)
        {
            RoomMember roomMember = new RoomMember();
            roomMember.playerName = roomMem.name;
            roomMember.id = roomMem.id;
            roomMember.ip = roomMem.ip;
            roomMember.camp = roomMem.camp;
            roomMember.campIndex = roomMem.campIndex;
            return roomMember;
        }

        public int CompareTo(RoomMember other)
        {
            if(other == null)
            {
                return -1;
            }
            return this.id - other.id;
            
        }
    }
}
