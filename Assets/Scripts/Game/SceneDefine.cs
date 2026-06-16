using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏内场景定义
/// </summary>
public class SceneDefine
{

    private static string _sceneSzie = "";
    public static string CurGameStartSceneSizeParam
    {
        get
        {
            return _sceneSzie;
        }
        set
        {
            _sceneSzie = value;
            if (value == "8X12")
            {
                SCENE_GAME_01_STORAGE = "01_12x8_Storage";
                SCENE_GAME_02_FACTORY = "02_12x8_Factory";
                SCENE_GAME_03_RUINS = "03_12x8_Ruins";
                SAFE_VER = 5.7f;
                SAFE_HOR = 3.5f;
                Debug.Log($"设置实地场景大小 默认 8 X 12");
            }
            else if (value == "" || value == "7X12" || value.Contains("12X7") || value.Contains("7X12"))
            {
                SAFE_VER = 5.7f;
                SAFE_HOR = 3.2f;
                Debug.Log($"设置实地场景大小 7 X 12");
            }
            else if (value == "6X6" || value.Contains("6X6"))
            {
                SCENE_GAME_00_ROOM = "04_12x7_Room1";
                SAFE_HOR = 3f;
                SAFE_HOR = 3f;
                Debug.Log($"设置实地场景大小 6 X 6");
            }
        }
    }


    public static string SCENE_PREPARE = "00_Prepare";
    public static string SCENE_GAME_00_ROOM = "04_12x8_Room1";
    public static string SCENE_GAME_01_STORAGE = "01_Storage";
    public static string SCENE_GAME_02_FACTORY = "02_Factory";
    public static string SCENE_GAME_03_RUINS = "03_Ruins";
    public static string SCENE_GAMEOVER = "GameOverScene";
    public static float SAFE_HOR = 3.2f;
    public static float SAFE_VER = 5.7f;

}
