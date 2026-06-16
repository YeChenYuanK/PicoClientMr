using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LekeNet
{
    // 玩家信息获取
    public class PlayerInfo
    {
        private static string name;

        public static string Name
        {
            set { name = value;}
            get { return "a"; }
        }

        private static int roomUnitId;

        public static int RoomUnitId
        {
            set { roomUnitId = value; }
            get { return roomUnitId; }
        }
    }

}
