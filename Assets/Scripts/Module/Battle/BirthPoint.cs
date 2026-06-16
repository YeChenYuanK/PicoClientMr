using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirthPoint : MonoBehaviour {
    /// <summary>
    /// 出生点编号
    /// </summary>
    public int Index;

    /// <summary>
    /// 点所属阵营
    /// </summary>
    public int Camp;

    public SectorProgressBar progressBar;
    public Transform Medicine;

    public void ShowRebirthProgress(float p)
    {
        progressBar.angleDegree = p * progressBar.segments;
        ObjectUtil.UpdateObjectActive(Medicine, false);
    }

    public void HideProgress()
    {
        progressBar.angleDegree = 0;
        ObjectUtil.UpdateObjectActive(Medicine, true);
    }


    void Start () {
		
	}
	
	void Update () {
		
	}
}
