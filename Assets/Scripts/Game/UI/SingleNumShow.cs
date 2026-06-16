using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SingleNumShow : MonoBehaviour {

    public Sprite s0;
    public Sprite s1;
    public Sprite s2;
    public Sprite s3;
    public Sprite s4;
    public Sprite s5;
    public Sprite s6;
    public Sprite s7;
    public Sprite s8;
    public Sprite s9;

    private Image image;

    // Use this for initialization
    void Start () {
        image = this.transform.GetComponent<Image>();

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ShowNum(int i)
    {
        if(i > 9 || i < 0)
        {
            return;
        }

        image.sprite = GetSprite(i);
    }

    private Sprite GetSprite(int index)
    {
        switch(index)
        {
            case 0:
                return s0;
            case 1:
                return s1;
            case 2:
                return s2;
            case 3:
                return s3;
            case 4:
                return s4;
            case 5:
                return s5;
            case 6:
                return s6;
            case 7:
                return s7;
            case 8:
                return s8;
            case 9:
                return s9;
        }
        return s0;
    }
}
