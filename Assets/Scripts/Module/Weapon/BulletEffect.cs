using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEffect : MonoBehaviour {

    public List<BulletEffectWave> waveList;

    private BulletEffectWave curWave;

    private float NextWaveTime = 0;

    private bool isPlaying;

    void Start () {
        HideAll();
        PlayEffect();
    }

    void HideAll()
    {
        for(int i =0;i < waveList.Count;i++)
        {
            waveList[i].HideAll();
        }
    }

    public void PlayEffect()
    {
        if(isPlaying)
        {
            return;
        }
        if (curWave == null)
        {
            curWave = waveList[Random.Range(0, waveList.Count)];
            curWave.BeginEffect();
            isPlaying = true;
        }
    }

    public void StopEffect()
    {
        if (curWave != null)
        {
            curWave.EndEffect();
        }
    }
	
	void Update () {
		if(curWave != null)
        {
            if (curWave.IsComplete())
            {
                curWave.EndEffect();
                curWave = null;
                isPlaying = false;
                // NextWaveTime = Time.time + Random.Range(0.5f, 1.5f);
            }
        } else
        {
            /*
            if(NextWaveTime <= Time.time)
            {
                PlayEffect();
            }
            */
        }

	}
}
