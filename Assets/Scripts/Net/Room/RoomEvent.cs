using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace LekeNet.Room
{
    public class RoomEvent
    {
        // 有新房间创建
        public const string ROOM_NEW_CREATE = "RM_CREATE";
        // 房间有人加入
        public const string ROOM_MEMBER_ADD = "RM_MEM_ADD";
        // 房间有人离开
        public const string ROOM_MEMBER_LEAVE = "RM_MEM_REMOVE";
        // 游戏操作
        public const string OPERATION = "OPERATION";
        // 房间加入成功
        public const string ROOM_JOIN = "RM_JOINT";
        // 新成员加入服务器
        public const string NEW_MEM_ADD_SER = "MEM_ADD_SER";
        // 失去与服务器的连接
        public const string LOST_SERVER_CONNECTION = "LOST_SERVER_CONNECTION";
        // 房间状态发生改变
        public const string ROOM_STATUS_CHANGE = "ROOM_STATUS_CHANGE";
        // 服务器等待客户端场景加载完毕
        public const string FIGHT_START_COMPLETE = "FIGHT_START_COMPLETE";
        // 系统操作（非游戏操作）
        public const string SYS_OPERATION = "SYS_OPERATION";
        //玩家被出房间
        public const string KICK_OFF = "KICK_OFF";
    }
}
