using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageSprite : MonoBehaviour
{
    public string Key;
    private SpriteRenderer spriteRenderer;
    private Image uiImage;
    public void OnEnable()
    {
        GameMng.Instance.ChangeLanguage += GetSprite;
    }
    public void OnDisable()
    {
        GameMng.Instance.ChangeLanguage -= GetSprite;
    }
    void Start()
    {
        uiImage = GetComponent<Image>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        GetSprite();
    }

    public void GetSprite()
    {
        Sprite sp = GameMng.Instance._prefebMng.GetLanguageSpring(Key);
        if (uiImage != null)
            uiImage.sprite = sp;
        else if (spriteRenderer != null)
            spriteRenderer.sprite = sp;
    }
}
