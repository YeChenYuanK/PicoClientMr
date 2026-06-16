using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using com.gamestudio.tank;
using LekeNet.Util;
using ProtoBuf;

namespace LekeNet.Room
{
    public class RoomOperation
    {
        // 发送者Id
        private int senderId;
        // 操作命令
        public int operId;
        // 操作数据
        public byte[] operData;

        public static RoomOperation ValueOf(Operation operation)
        {
            RoomOperation roomOperation = new RoomOperation();
            roomOperation.operId = operation.operCmd;
            roomOperation.operData = operation.operData;
            return roomOperation;
        }

        public Operation ToProto()
        {
            Operation oper = new Operation();
            oper.operCmd = this.operId;
            oper.operData = this.operData;
            return oper;
        }

        public T GetData<T>()
        {
            byte[] data = this.operData;
            MemoryStream stream = new MemoryStream();
            stream.Write(data, 0, data.Length);
            stream.Position = 0;
            try
            {
                return Serializer.Deserialize<T>(stream);
            }catch(Exception e)
            {
                UnityEngine.Debug.LogError(e);
                return default(T);
            }
            
        }

        public int SenderId
        {
            get
            {
                return senderId;
            }

            set
            {
                senderId = value;
            }
        }

    }
}
