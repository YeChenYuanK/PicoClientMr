using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PointAim : MonoBehaviour 
{
	bool status = false;
	Material orgMat ;
	Material changeMat;
	Image image;

	Image PointImage
	{
		get 
		{
			if (image == null) 
			{
				image = GetComponent<Image> ();
			}
			return image;
		}
	}

	public void Play(Material orgMat , Material changeMat)
	{
		status = true;
		this.orgMat = orgMat;
		this.changeMat = changeMat;
	}

	public void Stop(Material orgMat)
	{
		status = false;
		if (orgMat != PointImage.material) 
		{
			PointImage.material = orgMat;
		}
	}

	// Use this for initialization
	void Start () 
	{
	}



	int pointer = 1;
	int index = 0;

	// Update is called once per frame
	void Update () 
	{
		if(status)
		{
			if(Mathf.Abs(index) >= 50)
			{
//				Debug.Log ("pointer:" + pointer + " , index:" + index);
				if (index > 0) 
				{
					image.material = orgMat;
				} else 
				{
					image.material = changeMat;
				}
				pointer *= -1;
				index = 0;
			}
//			Debug.Log ("pointer:" + pointer + " , index:" + index);
			index += pointer;
		}
	}
}
