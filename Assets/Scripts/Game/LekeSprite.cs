using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LangDefine
{
    NONE = 0,
    CN = 1,
    EN = 2
}

public class LekeLang
{
    public static LangDefine GlobalLangeDefine = LangDefine.CN;
}


[ExecuteInEditMode]
public class LekeSprite : MonoBehaviour
{

    public LangDefine langDefine = LangDefine.CN;

    public SpriteRenderer spriteRenderer;

    public Sprite imgCn;
    public Sprite imgEn;

    void Start()
    {
        this.langDefine = LekeLang.GlobalLangeDefine;
        UpdateSpriteRenderer();
    }

    void Update()
    {
        UpdateSpriteRenderer();
    }

    private LangDefine lastLangDefine = LangDefine.NONE;

    void UpdateSpriteRenderer()
    {
        if(lastLangDefine != langDefine)
        {
            lastLangDefine = langDefine;
            if (langDefine == LangDefine.CN)
            {
                spriteRenderer.sprite = imgCn;
            }
            else if (langDefine == LangDefine.EN)
            {
                spriteRenderer.sprite = imgEn;
            }
        }
    }
}
