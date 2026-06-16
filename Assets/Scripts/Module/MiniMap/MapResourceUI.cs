using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using com.gamestudio.cs;

public class MapResourceUI: MonoBehaviour {

	public Transform PointRoot;
	public Transform selfPoint;
	public Transform MiniMapPointPrefab;
	public static MapResourceUI Instance;

	private BasePoint[] basePoints;
	private Dictionary<int , SpriteRenderer> MiniPointDic;

	void Awake()
	{
		Instance = this;
	}

	// Use this for initialization
	void Start () {
		PointRoot = GameObject.Find ("PointRoot").transform;
		InitMapPoint ();
	}

	public void InitMapPoint()
	{
		//z:-42 55 97

		basePoints = PointRoot.transform.GetComponentsInChildren<BasePoint>();
		MiniPointDic = new Dictionary<int, SpriteRenderer> ();
		Transform miniPoint;
		SpriteRenderer miniPointImg;
		Animator pointAnim;
		foreach(BasePoint basePoint in basePoints)
		{
			MTeleporter teleporter = MapManager.Instance.GetTeleporter (basePoint.index);
			miniPoint = GameObject.Instantiate (MiniMapPointPrefab , transform) as Transform;
			miniPointImg = miniPoint.GetComponent<SpriteRenderer> ();
			pointAnim = miniPoint.GetComponent<Animator> ();
			MiniPointDic [basePoint.index] = miniPointImg;
			miniPoint.transform.localPosition = new Vector3 (basePoint.transform.localPosition.x / 10 , basePoint.transform.localPosition.z / 10 , 0 );
			miniPoint.transform.localRotation = Quaternion.identity;
//			miniPoint.transform.localPosition = basePoint.transform.localPosition / 1.5f;

			pointAnim.CrossFade(((Camp)teleporter.index).ToString() , 0);
		}
		//1.5:100
	}

	public void UpdateMapPoint()
	{
		SpriteRenderer miniPointImg;
		Animator pointAim;
		foreach (KeyValuePair<int , SpriteRenderer> kvPair in MiniPointDic) 
		{
			MTeleporter teleporter = MapManager.Instance.GetTeleporter (kvPair.Key);
			miniPointImg = kvPair.Value;
			pointAim = kvPair.Value.GetComponent<Animator> ();
			pointAim.CrossFade (((Camp)teleporter.curCampIndex).ToString() , 0);


			//正在被占领
			if (teleporter.occupyCampIndex != 0 && teleporter.occupyCampIndex != teleporter.curCampIndex) {
				pointAim.CrossFade(((Camp)teleporter.curCampIndex + "2" + (Camp)teleporter.occupyCampIndex).ToString() , 0);
			} 

		}

	}

	private PointAim GetPointAnim(SpriteRenderer miniPointImg)
	{
		PointAim aim;
		if (miniPointImg.GetComponent<PointAim> () != null) 
		{
			aim = miniPointImg.GetComponent<PointAim> ();
		}else
		{
			aim = miniPointImg.gameObject.AddComponent<PointAim> ();
		}
		return aim;
	}


	// Update is called once per frame
	void Update () 
	{
		
	}
}
