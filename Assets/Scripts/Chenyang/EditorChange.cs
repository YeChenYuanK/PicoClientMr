using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteAlways]
public class EditorChange : MonoBehaviour
{
    float Y = 5;
    public void Start()
    {
        GameOverItem[] ITEMS = GetComponentsInChildren<GameOverItem>();
        for (int i = 0; i < ITEMS.Length; i++)
        {
            Vector3 POS = ITEMS[i].transform.localPosition;
            POS .y= Y;
            ITEMS[i].transform.localPosition = POS;
            Y -= 2.5f;
            ITEMS[i].transform.SetAsLastSibling();
        }
    }
}
