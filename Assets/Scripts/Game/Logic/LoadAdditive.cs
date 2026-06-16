using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 加载额外的Additive的场景
/// </summary>
public class LoadAdditive : MonoBehaviour
{
    public string SceneName;
    
    void Start()
    {
        SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Additive);
        Debug.Log("开始加载 Additive" + SceneName);
    }
    
    void Update()
    {
        
    }
}
