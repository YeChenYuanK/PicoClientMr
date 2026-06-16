using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ReviveCount : MonoBehaviour {
	
	public Text count;
	
    void Start () 
	{

    }

	void Update () 
	{
	
	}

    public void ShowNum(int i)
    {
        if(i > 9 || i < 0)
        {
            return;
        }

		count.text = i.ToString ();
    }

}
