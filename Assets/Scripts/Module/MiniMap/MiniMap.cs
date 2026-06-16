using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using com.gamestudio.cs;

public class MiniMap : MonoBehaviour {

	public Transform PointRoot;
	public Transform selfPoint;
	public Transform MiniMapPointPrefab;
	public static MiniMap Instance;

	private BasePoint[] basePoints;
	private Dictionary<int , Image> MiniPointDic;

	public Material RedMat;
	public Material GreenMat;
	public Material WhiteMat;

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
		MiniPointDic = new Dictionary<int, Image> ();
		RectTransform miniPoint;
		Image miniPointImg;
		foreach(BasePoint basePoint in basePoints)
		{
			MTeleporter teleporter = MapManager.Instance.GetTeleporter (basePoint.index);
			miniPoint = GameObject.Instantiate (MiniMapPointPrefab , transform) as RectTransform;
			miniPointImg = miniPoint.GetComponent<Image> ();
			MiniPointDic [basePoint.index] = miniPointImg;
			miniPoint.transform.localPosition = new Vector3 (basePoint.transform.localPosition.x/0.33f , basePoint.transform.localPosition.z / 0.33f , 0 );
			miniPoint.transform.localRotation = Quaternion.identity;
//			miniPoint.transform.localPosition = basePoint.transform.localPosition / 1.5f;

			if (teleporter.curCampIndex == (int)Camp.Red) 
			{
//					miniPointImg.color = Camp1Color;
				miniPointImg.material = RedMat;

			} else if (teleporter.curCampIndex == (int)Camp.Blue) 
			{
//					miniPointImg.color = Camp2Color;
				miniPointImg.material = GreenMat;
			} else 
			{
				//				miniPointImg.color = Color.white;
				miniPointImg.material = WhiteMat;
			}

		}
		//1.5:100
	}

	private void SetMapPoint(Sprite sprite , MTeleporter teleporter)
	{
		
	}

	public void UpdateMapPoint()
	{
		Image miniPointImg;
		foreach (KeyValuePair<int , Image> kvPair in MiniPointDic) 
		{
			MTeleporter teleporter = MapManager.Instance.GetTeleporter (kvPair.Key);
			miniPointImg = kvPair.Value;
			if(teleporter.curCampIndex == 0)
			{
				
			}
			//颜色
			miniPointImg.material = GetMatByCamp (teleporter.curCampIndex);

			//正在被占领
			if (teleporter.occupyCampIndex != 0 && teleporter.occupyCampIndex != teleporter.curCampIndex) {
				Material orgMat = GetMatByCamp(teleporter.curCampIndex);
				Material changeMat = GetMatByCamp(teleporter.occupyCampIndex);
				PointAim aim = GetPointAnim(miniPointImg);

				aim.Play (orgMat , changeMat);
			} else {
				PointAim aim = GetPointAnim(miniPointImg);
				aim.Stop (GetMatByCamp(teleporter.curCampIndex));
			}

		}

	}

	private PointAim GetPointAnim(Image miniPointImg)
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

	Material GetMatByCamp(int camp)
	{
		//颜色
		if (camp == (int)Camp.Red) 
		{
			return RedMat;
		} else if(camp == (int)Camp.Blue)
		{
			return GreenMat;
		}else
		{
			return WhiteMat;
		}
	}

	// Update is called once per frame
	void Update () 
	{
	}
}
