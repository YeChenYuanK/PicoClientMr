using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConst
{
    public static float InfantryAttackFormulaByDistance(float distance)
    {
        return 0.05f+(-40 * Mathf.Log(distance) + 160 - distance / 2) / 100;
    }

    public static float RocketAttackDamageByDistance(float damage , float distance)
    {
        return (100 - distance * 12) / 100 * damage;
    }

}
/// <summary>
/// 敌军类型
/// </summary>
public enum EnemyType
{
    /// <summary>
    /// 步兵
    /// </summary>
    INFANTRY = 1,
    /// <summary>
    /// 火箭兵
    /// </summary>
    ROCKET_SOLDIER,
    /// <summary>
    /// 自爆步兵
    /// </summary>
    INFANTRY_BLEW,
    /// <summary>
    /// 刀兵
    /// </summary>
    SWORD
}

public enum Road
{
    /// <summary>
    /// 左路
    /// </summary>
    LEFT = 1,
    /// <summary>
    /// 右路
    /// </summary>
    RIGHT = 2,
    /// <summary>
    /// 中路
    /// </summary>
    MIDDLE = 3
}

public enum HurtDirection
{
    NONE,
    LEFT,
    RIGHT,
    FRONT,
    DEAD,
    GAME_OVER,
    GAME_OVER_WIN
}

public enum RebirthType
{
    /// <summary>
    /// 原地复活
    /// </summary>
    FORMER_PLACE,
    /// <summary>
    /// 复活点复活
    /// </summary>
    REBIRTH_POINT
}

public enum AudioType
{
    AUDIO_HURT_NORMAL = 1,
    AUDIO_HURT_HEAD = 2,
    AUDIO_DEATH_NORMAL = 3,
    AUDIO_DEAD_HEAD = 4,
    AUDIO_DEATH_HEAD = 5,
    AUDIO_RESURGENCE = 6,
    AUDIO_DOUBLE_KILL = 7,
    AUDIO_TRI_KILL = 8,
    AUDIO_ULTRA_KILL = 9,
    AUDIO_MONSTER_KILL = 10,
    AUDIO_GOD_LIKE = 11,
    AUDIO_HOLLYSHIT = 12,
    AUDIO_UNSTOPABLE = 13,
    AUDIO_RANMPAGE = 14,
     AUDIO_FIRSTBLOOD=15,
     AUDIO_ENDCOMBOKILL=16,
    AUDIO_DEAD_NORMAL = 17,
    NONE,
}

[System.Serializable]
public class AudioConfig
{
    public AudioType audioType;
    public AudioClip audioClip;
}