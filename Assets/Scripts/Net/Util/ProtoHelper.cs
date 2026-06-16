using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.gamestudio.cs;
using UnityEngine;

public class ProtoHelper
{
    public static MVector ConvertToProto(Vector3 vec)
    {
        MVector mv = new MVector();

        mv.x = vec.x;
        mv.y = vec.y;
        mv.z = vec.z;
        return mv;
    }

    public static Vector3 ConvertFromProto(MVector mvec)
    {
        return new Vector3(mvec.x, mvec.y, mvec.z);
    }

    public static MQuaternion ConvertToProto(Quaternion qua)
    {
        MQuaternion mq = new MQuaternion();

        mq.x = qua.x;
        mq.y = qua.y;
        mq.z = qua.z;
        mq.w = qua.w;
        return mq;
    }

    public static Quaternion ConvertFromProto(MQuaternion mqua)
    {
        return new Quaternion(mqua.x, mqua.y, mqua.z, mqua.w);
    }
}

