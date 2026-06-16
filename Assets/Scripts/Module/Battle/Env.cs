using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Env : MonoBehaviour {
    private static Env instance;
    public static Env Instance
    {
        get
        {
            return instance;
        }
    }
    public AudioClip gameOverAudio;
    public AudioClip gameOverWinAudio;
    public AudioClip gameOverFailAudio;
    public AudioClip countDownAudio;
    public AudioClip gameStartAudio;
    public AudioClip firstBloodAudio;
    public AudioClip killTeamAudio;
    public AudioClip BeFinishComboAudio;
    public AudioSource AudioSource_Bgm;
    public AudioSource AudioSource_ENV;
    public AudioSource AudioSource_Room;
    public AudioSource AudioSource_CountDown;
    public AudioSource AudioSource_StarOrEnd;
    public AudioSource AudioSource_FirstBlood;
    public AudioSource AudioSource_TeamKill;
    public AudioSource AudioSource_BeFinishCombo;
    public List<AudioConfig> AudioConfigList;



    private bool hasPlay;

    private void Awake()
    {
        instance = this;
        // DontDestroyOnLoad(this.gameObject);
    }
    /// <summary>
    /// 首杀音效
    /// </summary>
    public void PlayFirstBloodAudio(bool TeamKill)
    {
        AudioSource_FirstBlood.PlayOneShot(firstBloodAudio,1f);
        if (TeamKill)
        {
            //p1 p5 被杀
            PlayKillTeam(true);
        }
       
    }
    public void PlayKillTeam(bool isFirst)
    {
       
        if(isFirst)
            StartCoroutine(PlayKillTeamAudio(firstBloodAudio.length));
        else
            StartCoroutine(PlayKillTeamAudio(0));
    }
    /// <summary>
    /// 播放倒计时
    /// </summary>
    public void PlayCountDownAudio()
    {
        AudioSource_CountDown.PlayOneShot(countDownAudio);
    }
    /// <summary>
    /// 播放游戏开始音效
    /// </summary>
    public void PlayGameStart()
    {
        AudioSource_Room.Stop();
        AudioSource_StarOrEnd.PlayOneShot(gameStartAudio);
        this.StartCoroutine(DelayPlayBgm(gameStartAudio.length));
    }
    public void PlayRoomBGM()
    {
        this.AudioSource_Bgm.Stop();
        if (GameMng.Instance.isServerHost)
        {
            AudioSource_Room.volume = (float)(GameMng.Instance._parameterMng._parameters.BGMvolume / 100f);
        }
        this.AudioSource_Room.Play();
    }
    /// <summary>
    /// 播放游戏背景音
    /// </summary>
    /// <returns></returns>
    public IEnumerator DelayPlayBgm(float wait)
    {
        yield return new WaitForSeconds(wait);
        if (GameMng.Instance.isServerHost)
        {
            AudioSource_Bgm.volume = (float)(GameMng.Instance._parameterMng._parameters.BGMvolume / 100f);
        }
        this.AudioSource_Bgm.Play();
    }

    public void Stopbgm()
    {
      
    }
    /// <summary>
    /// 播放游戏结束音效
    /// </summary>
    public void PlayGameOverAudio(int win)
    {
        if (GameMng.Instance.isServerHost)
        {
            AudioSource_StarOrEnd.PlayOneShot(gameOverAudio);
        }
        else
        {
            if (GameMng.Instance._mySelf.Camp == win)
            {
                AudioSource_StarOrEnd.PlayOneShot(gameOverWinAudio);
            }
            else
            {
                AudioSource_StarOrEnd.PlayOneShot(gameOverFailAudio);
            }
        }
    }
    /// <summary>
    /// 播放被终结连续击杀音效
    /// </summary>
    public void PlayBeFinishKill(float wait)
    {
        StartCoroutine(PlayFinishKill(wait));
    }
    IEnumerator PlayFinishKill(float wait)
    {
        yield return new WaitForSeconds(wait);
        AudioSource_BeFinishCombo.PlayOneShot(BeFinishComboAudio, 1f);
    }
  
    IEnumerator  PlayKillTeamAudio(float wait)
    {
        yield return new WaitForSeconds(wait);
        AudioSource_TeamKill.PlayOneShot(killTeamAudio,1f);
        
       
    }

    /// <summary>
    /// 播放击杀音效
    /// </summary>
    /// <param name="audioType"></param>
    public void PlayEnvEffect(AudioType audioType, bool teamKill)
    {
        var clip = GetClip(audioType);
        if(clip != null)
        {
            this.StartCoroutine(DelayVoice(clip, teamKill));
            
        }  
    }
    private IEnumerator DelayVoice(AudioClip clip,bool TeamKill)
    {
        yield return new WaitForSeconds(1.0f);
        AudioSource_ENV.clip = clip;
        AudioSource_ENV.PlayOneShot(clip, 1);
        if (TeamKill)
        {
        StartCoroutine(PlayKillTeamAudio(clip.length));
        }
    }

    public AudioClip GetClip(AudioType audioType)
    {
        for(int i=0;i < this.AudioConfigList.Count;i++)
        {
            if(this.AudioConfigList[i].audioType == audioType)
            {
                return this.AudioConfigList[i].audioClip;
            }
        }
        return null;
    }

    public bool ContiansAudio(AudioType audioType)
    {
        for (int i = 0; i < this.AudioConfigList.Count; i++)
        {
            if (this.AudioConfigList[i].audioType == audioType)
            {
                return true;
            }
        }
        return false;
    }

    [ContextMenu("Test")]
    public void TestPlayAudio()
    {
       //his.PlayEnvEffect(AudioType.AUDIO_DOUBLE_KILL);
    }
}

