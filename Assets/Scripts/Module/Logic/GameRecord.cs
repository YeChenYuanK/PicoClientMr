using com.leke.redSea;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProtoBuf;
using System.IO;

public class GameRecord : BaseManager {

    public GameRecordInfo gameRecordInfo = new GameRecordInfo();

    private MemoryStream buff = new MemoryStream();

    public GameRecord(GameMng mng) : base(mng)
    {
    }

    public void Clear()
    {
        gameRecordInfo.playerRecords.Clear();
    }

    /*
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            buff.Position = 0;
            Serializer.Serialize(buff, gameRecordInfo);
            long len = buff.Position;
            byte[] data = new byte[len];
            buff.Position = 0;
            buff.Read(data, 0, (int)len);
            stream.SendNext(data);
        }
        else
        {
            byte[] data = (byte[])stream.ReceiveNext();
            buff.Position = 0;
            buff.Write(data, 0, data.Length);
            this.gameRecordInfo = Serializer.Deserialize<GameRecordInfo>(buff);
        }
    }
    */
    public byte[] GenRecordData()
    {
        buff.Position = 0;
        Serializer.Serialize(buff, gameRecordInfo);
        long len = buff.Position;
        byte[] data = new byte[len];
        buff.Position = 0;
        buff.Read(data, 0, (int)len);
        return data;
    }

    public void ParseRecordData(byte[] data)
    {
        buff.Position = 0;
        buff.Write(data, 0, data.Length);
        buff.Position = 0;
        this.gameRecordInfo = Serializer.Deserialize<GameRecordInfo>(buff);
    }

    public PlayerRecord GetPlayerRecord(int index)
    {
        for(int i=0;i<gameRecordInfo.playerRecords.Count;i++)
        {
            if(gameRecordInfo.playerRecords[i].Index == index)
            {
                return gameRecordInfo.playerRecords[i];
            }
        }
        return null;
    }

}
