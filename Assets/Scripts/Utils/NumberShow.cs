using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberShow : MonoBehaviour {

    public List<Sprite> numberResList;

    public List<SpriteRenderer> showList;

	void Start () {
	}
	
	void Update () {
		
	}

    void Clear()
    {
        for (int i = 0; i < showList.Count; i++)
        {
            showList[i].sprite = null;
        }
    }

    public void ShowNumber(int num)
    {
        Clear();
        string numStr = num.ToString();
        if (numStr.Length > 4) return;
        float begin = 0 - numStr.Length * 1.2f / 2 + 0.5f;

        for(int i=0;i<numStr.Length;i++)
        {
            int resIndex = int.Parse(numStr[i].ToString());
            showList[i].sprite = numberResList[resIndex];
            showList[i].transform.localPosition = new Vector3(begin + i * 1.2f, 0, 0);
        }
    }
}
