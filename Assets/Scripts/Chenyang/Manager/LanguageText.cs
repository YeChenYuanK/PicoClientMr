using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageText : MonoBehaviour
{
    public string Key;
    private Text text;
    public void OnEnable()
    {
        GameMng.Instance.ChangeLanguage += GetText;
    }
    public void OnDisable()
    {
        GameMng.Instance.ChangeLanguage -= GetText;
    }
    void Start()
    {
        text = GetComponent<Text>();
        GetText();
    }

    public void GetText()
    {
        text.text = GameMng.Instance._prefebMng.GetLanguageString(Key);
    }
}
