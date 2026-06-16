using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using LekeNet.Room;
using com.gamestudio.tank;
using System.Net.Sockets;
using System.Net;
using System.IO;
using UnityEngine.SceneManagement;
using com.gamestudio.sys;
using com.gamestudio.room;
using com.gamestudio.cs;

namespace LekeNet
{
    public class TcpLogic
    {
        private RoomClient roomClient;
        private string puid;
        private int roomid;

        public RoomClient RoomClient
        {
            get
            {
                return roomClient;
            }

            set
            {
                roomClient = value;
            }
        }

        public TcpLogic()
        {
            this.InitNetHandler();
        }

        public void InitNetHandler()
        {
            TcpContext.Instance.RegisterNetHandler((int)SysOpCode.LOGIN_S, OnLogin);
            TcpContext.Instance.RegisterNetHandler((int)SysOpCode.CREATE_ROOM_S, OnCreateRoom);
            TcpContext.Instance.RegisterNetHandler((int)SysOpCode.JOIN_ROOM_S, OnJoinRoom);
            TcpContext.Instance.RegisterNetHandler((int)SysOpCode.GAME_START_S, OnGameStart);
            TcpContext.Instance.RegisterNetHandler((int)SysOpCode.GET_ROOM_INFO_S, OnRoomInfo);
            TcpContext.Instance.RegisterNetHandler((int)SysOpCode.TIME_SYNC, OnTimeSync);
            TcpContext.Instance.RegisterNetHandler((int)SysOpCode.CFG_SYNC, OnCfgSync);
        }

        /// <summary>
        /// 游戏开始
        /// </summary>
        /// <param name="tcpSession"></param>
        /// <param name="packet"></param>
        private void OnGameStart(TcpSession tcpSession, TcpPacket packet)
        {
            Debug.Log("开始游戏");

            //此处是服务器通过TCP传过来的信息 根据此信息再发给服务器 建立UDP连接
            BuildUdpSession buildUdpSession = packet.Deserialize<BuildUdpSession>();
            this.puid = buildUdpSession.puid;
            this.roomid = buildUdpSession.roomid;
            MainFrameCall.Instance.AddCall(OnLoadScene,null);
        }

        private object OnLoadScene(object obj)
        {
            SceneManager.LoadScene("Start");
            return null;
        }

        /// <summary>
        /// 建立UDP连接
        /// 
        /// </summary>
        public void BuildUdpSession(string udpIp, int udpPort, int roomId, string puid)
        {
            BuildUdpSession buildUdpSession = new BuildUdpSession();
            buildUdpSession.puid = this.puid = puid;
            buildUdpSession.roomid = this.roomid = roomId;
            //建立UDP连接
            this.roomClient = new RoomClient(udpIp, udpPort, buildUdpSession);
            ClientLogic.Instance.Init(this.roomClient);
        }

        /// <summary>
        /// 加入房间
        /// </summary>
        /// <returns></returns>
        private void OnJoinRoom(TcpSession tcpSession, TcpPacket packet)
        {
            com.gamestudio.room.RoomInfo roomInfo = packet.Deserialize<com.gamestudio.room.RoomInfo>();
            Debug.Log("加入房间成功");

            BuildUdpSession(roomInfo.roomIp, roomInfo.roomPort, roomInfo.roomId, NetHelper.GetMac());
            Debug.Log("发送 BuildUdpSession 创建UDP连接1");
        }


        /// <summary>
        /// 创建房间
        /// </summary>
        /// <param name="tcpSession"></param>
        /// <param name="packet"></param>
        private void OnCreateRoom(TcpSession tcpSession, TcpPacket packet)
        {
            string roomContext = "";
            RoomInfos roomInfos = packet.Deserialize<RoomInfos>();
            foreach(com.gamestudio.room.RoomInfo roomInfo in roomInfos.roomInfos)
            {
                roomContext += "房间号:" + roomInfo.roomId + " 房间名:" + roomInfo.roomName+"\r\n"+
                    "房间人数："+roomInfo.currentSize+"/"+roomInfo.maxSize;
                System.Object re = null;
                MainFrameCall.Instance.AddCallSync(new MainFrameCall.FrameCall(AddRoom), roomInfo.roomId, out re);
            }

            System.Object result = null;
            MainFrameCall.Instance.AddCallSync(new MainFrameCall.FrameCall(ShowRooms), roomContext, out result);
        }

        private System.Object AddRoom(System.Object obj)
        {
            int roomid = (int)obj;
            return null;
        }

        private System.Object ShowRooms(System.Object obj)
        {
            string roomContext = obj as string;
            return null;
        } 

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="tcpSession"></param>
        /// <param name="packet"></param>
        private void OnLogin(TcpSession tcpSession, TcpPacket packet)
        {
            Debug.Log("登录成功");

            TcpContext.Instance.TcpSession.SendProto((int)SysOpCode.GET_ROOM_INFO_C, null);
        }


        private void OnRoomInfo(TcpSession tcpSession, TcpPacket packet)
        {
            Debug.Log("请求房间返回");

            RoomInfos roomInfos = packet.Deserialize<RoomInfos>();
            String roomInfoStr = "";
            foreach (com.gamestudio.room.RoomInfo roomInfo in roomInfos.roomInfos)
            {
                roomInfoStr += "房间号:" + roomInfo.roomId + " 房间名:" + roomInfo.roomName + "\r\n" +
                    "房间人数：" + roomInfo.currentSize + "/" + roomInfo.maxSize;
                // System.Object re = null;
                // MainFrameCall.Instance.AddCallSync(new MainFrameCall.FrameCall(AddRoom), roomInfo.roomId, out re);
                
            }
            Debug.Log(roomInfoStr);

            if(roomInfos.roomInfos.Count > 0)
            {
                com.gamestudio.room.JoinRoom joinRoom = new com.gamestudio.room.JoinRoom();
                joinRoom.roomId = roomInfos.roomInfos[0].roomId;
                Debug.Log("发送加入房间请求");
                TcpContext.Instance.TcpSession.SendProto((int)SysOpCode.JOIN_ROOM_C, joinRoom);
            }
        }

        /// <summary>
        /// 设置玩家身高并同步
        /// </summary>
        /// <param name="height">The height.</param>
        public void SetAndSyncHeight(float height)
        {
            HeightSet heightSet = new HeightSet();
            heightSet.height = (int)(height * 100);
            TcpContext.Instance.TcpSession.SendProto((int)SysOpCode.HEIGHT_SET_C, heightSet);
        }

        /// <summary>
        /// 时间同步
        /// </summary>
        /// <returns></returns>
        private void OnTimeSync(TcpSession tcpSession, TcpPacket packet)
        {
            TimeSync timeSync = packet.Deserialize<TimeSync>();
            Debug.Log("time sync ");
            DateUtil.ServerTimeDiff = DateUtil.NowMllSec - timeSync.timestamp;
        }

        /// <summary>
        /// 全局配置同步
        /// </summary>
        /// <returns></returns>
        private void OnCfgSync(TcpSession tcpSession, TcpPacket packet)
        {
            GlobalCfg globalCfg = packet.Deserialize<GlobalCfg>();
            ConfigManager.GlobalCfg = globalCfg;
        }
    }
}
