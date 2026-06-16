using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSingleEffect : MonoBehaviour {
    public AudioSource sound;
    public List<AudioClip> clips;

    void Start () {
		
	}
	
	void Update () {
		
	}

    public void Play(float effectTime)
    {
        this.gameObject.SetActive(true);
        this.sound.clip = clips[Random.Range(0, clips.Count)];
        this.sound.Play();
        this.StartCoroutine(SingleEnd(effectTime));
    }

    private IEnumerator SingleEnd(float effectTime)
    {
        yield return new WaitForSeconds(effectTime);
        this.gameObject.SetActive(false);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
}
