using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScreen : MonoBehaviour
{


    public Env env;
    public List<Sprite> numSprites;
    public SpriteRenderer m1;
    public SpriteRenderer m2;
    public SpriteRenderer s1;
    public SpriteRenderer s2;
    private bool hasPlayCountDownAudio;
    private bool hasPlayGameOverAudio;
    private int Frame;
    void Start()
    {
        // 默认显示总游戏时长的时间
        //  ShowLeftTime(SystemData.GameTime);
    }

    void Update()
    {

        if (Frame < 30)
        {
            Frame++;
            return;
        }

        int leftTime = GameMng.Instance.GameTime;

        if (leftTime <= 0 && !hasPlayGameOverAudio)
        {
            if (Env.Instance != null)
            {
                hasPlayGameOverAudio = true;
                if (GameMng.Instance.isServerHost)
                {
                    Env.Instance.PlayGameOverAudio(-1);
                }
            }

        }
        ShowLeftTime(Mathf.Max(leftTime, 0));
        if (Env.Instance != null)
        {
            if (leftTime <= 10 && !hasPlayCountDownAudio)
            {
                Env.Instance.PlayCountDownAudio();
                hasPlayCountDownAudio = true;
            }
        }

    }

    private void ShowLeftTime(int leftSec)
    {
        int minutes = leftSec / 60;
        int seconds = leftSec % 60;
        int m1num = minutes / 10;
        int m2num = minutes % 10;
        m1.sprite = numSprites[m1num];
        m2.sprite = numSprites[m2num];
        int s1num = seconds / 10;
        int s2num = seconds % 10;
        s1.sprite = numSprites[s1num];
        s2.sprite = numSprites[s2num];
    }
}
