using UnityEngine;
using System.Collections;

public class MathUtil
{


    public static float angle_360_Z(Vector3 from_, Vector3 to_)
    {

        Vector3 v3 = Vector3.Cross(from_, to_);
        float angle = 0;
        if (v3.z > 0)
        {
            angle = Vector3.Angle(from_, to_);
        }
        else
        {
            angle = 360 - Vector3.Angle(from_, to_);
        }
        return angle % 360;
    }

    public static float angle_360_XY(Vector3 from_, Vector3 to_)
    {

        Vector3 v3 = Vector3.Cross(from_, to_);

        if (v3.z > 0)
        {
            return Vector3.Angle(from_, to_);
        }
        else
        {
            return 360 - Vector3.Angle(from_, to_);
        }
    }

    public static float angle(Vector3 from_, Vector3 to_)
    {
        float ang = angle_360_XY(from_, to_);
        if (ang > 180)
        {
            ang = 360 - ang;
        }
        return ang;
    }


    public static float angle_360(Vector3 from_, Vector3 to_)
    {

        Vector3 v3 = Vector3.Cross(from_, to_);

        if (v3.z > 0)
        {
            return Vector3.Angle(from_, to_);
        }
        else
        {
            return 360 - Vector3.Angle(from_, to_);
        }
    }
    


    // unclamped version of Lerp, to allow value to exceed the from-to range
    public static float ULerp(float from, float to, float value)
    {
        return (1.0f - value) * from + value * to;
    }


    // simple function to add a curved bias towards 1 for a value in the 0-1 range
    public static float CurveFactor(float factor)
    {
        return 1 - (1 - factor) * (1 - factor);
    }

    //撞向墙壁贴边向量
    public static Vector3 ReboundVectorByWall(Vector3 position, Vector3 normal, string tagName)
    {
        float x2 = Mathf.Abs(normal.z);
        if (normal.x < 0)
        {
            x2 = -x2;
        }
        float z2 = Mathf.Abs(normal.x);
        float y2 = 0;
        RaycastHit hit;
        Vector3 hitPoint = position;
        if (Physics.Raycast(hitPoint + new Vector3(normal.x , 0,0), Vector3.down, out hit, 100))
        {
            y2 = Mathf.Abs(hit.normal.y);
        }
        else
        {
            return Vector3.zero;
        }
        return new Vector3(x2, Mathf.Abs(hit.normal.z), z2).normalized;
    }

    //延X轴旋转后的位置差值
    public static Vector3 RotationNextPointByX(Transform target , Transform obj , float len , float offAng )
    {
        Vector3 nextPosition;
        //float len = 1.1f;
        float ang = target.rotation.eulerAngles.x + offAng;
        if (target.rotation.eulerAngles.x > 180)
        {
            ang = target.rotation.eulerAngles.x - 360 + offAng;
        }
        float y2 = target.position.y + len * Mathf.Sin(ang * Mathf.Deg2Rad);
        float z2 = target.position.z - len * Mathf.Cos(ang * Mathf.Deg2Rad);
        //四元数转欧拉角的阈值校验
        //if ((target.rotation.eulerAngles.y == 180 && target.rotation.eulerAngles.z == 180)
        //    || (target.rotation.eulerAngles.x > 90 && target.rotation.eulerAngles.x < 240))
        //{
        //    y2 = obj.position.y + len * Mathf.Sin(-ang * Mathf.Deg2Rad);
        //}
        nextPosition = new Vector3(obj.position.x, y2, z2);

        //return newPosition;

        //得到相机坐标点2 Vector3(x1,y2,z2)
        //玩家原点2 = 玩家原点1 + （Vector3(x1,y2,z2) - Vector3(x1,y1,z1)）
        y2 = target.position.y + len * Mathf.Sin(offAng * Mathf.Deg2Rad);// + target.localPosition.y;
        z2 = target.position.z - len * Mathf.Cos(offAng * Mathf.Deg2Rad);// + target.localPosition.z;
        //nextPosition = (nextPosition - new Vector3(obj.transform.position.x, y2, z2));
        nextPosition = (nextPosition - new Vector3(obj.transform.position.x, y2, z2));
        return nextPosition;
    }

    public static float RotationToEularX(Quaternion rotation)
    {
        return Mathf.Asin(2 * (rotation.x * rotation.w - rotation.z * rotation.y)) * Mathf.Rad2Deg;
    }

    public static float RotationToEularY(Quaternion rotation)
    {
        float v1 = 2 * (rotation.y * rotation.w + rotation.x * rotation.z);
        float v2 = (1 - 2 * (rotation.x * rotation.x + rotation.y * rotation.y));
        return Mathf.Atan2(v1, v2) * Mathf.Rad2Deg;
    }

    public static float RotationToEularZ(Quaternion rotation)
    {
        float v1 = 2 * (rotation.z * rotation.w + rotation.y * rotation.x);
        float v2 = (1 - 2 * (rotation.x * rotation.x + rotation.z * rotation.z));
        return Mathf.Atan2(v1, v2) * Mathf.Rad2Deg;
        
    }



    //float格式化保留7位有效位，防止小数位溢出坐标错乱
    public static float FloatFormat(float num)
    {
        return float.Parse(num.ToString("f7"));
    }

}
