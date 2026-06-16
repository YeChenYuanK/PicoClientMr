using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEffectWave : MonoBehaviour
{
    public List<BulletSingleEffect> effects;

    public int Count;
    public int Complete;
    public float SpaceTime;
    public float NextEffectTime;
    public float EffectTime = 1.0f;
    public float NextWaveTime;
    List<BulletSingleEffect> usingEffect = new List<BulletSingleEffect>();
    private bool isEffect;
    public void HideAll()
    {
        for (int i = 0; i < effects.Count; i++)
        {
            effects[i].Hide();
        }
    }

    public void RefreshProps()
    {
        if(Random.Range(0.0f,1.0f) < 0.2f)
        {
            Count = Random.Range(1, effects.Count + 1);
        } else
        {
            Count = Random.Range(effects.Count-1, effects.Count + 1);
        }
        Complete = 0;
        SpaceTime = Random.Range(0.1f, 0.5f);
        usingEffect.Clear();
        usingEffect.AddRange(effects);
    }

    public bool IsComplete()
    {
        return Complete >= Count;
    }

    public void BeginEffect()
    {
        RefreshProps();
        this.gameObject.SetActive(true);
        this.NextEffectTime = Time.time;
        this.isEffect = true;
    }

    public void EndEffect()
    {
        this.gameObject.SetActive(false);
        this.NextEffectTime = 0;
        this.isEffect = false;
        HideAll();
    }

    private void PlayEffect()
    {
        if (usingEffect.Count <= 0) return;
        BulletSingleEffect effect = usingEffect[Random.Range(0, usingEffect.Count)];
        usingEffect.Remove(effect);
        effect.Play(EffectTime);
        this.StartCoroutine(SingleEnd());
    }

    private IEnumerator SingleEnd()
    {
        yield return new WaitForSeconds(EffectTime);
        Complete++;
    }

    private void Update()
    {
        if(isEffect && this.NextEffectTime <= Time.time)
        {
            PlayEffect();
            this.NextEffectTime = Random.Range(0.2f, 0.5f) + Time.time;
        }
    }
}
