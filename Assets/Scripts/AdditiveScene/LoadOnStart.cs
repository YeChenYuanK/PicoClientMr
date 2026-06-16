using UnityEngine;

using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

using UnityEngine.SceneManagement;

[ExecuteInEditMode]
public class LoadOnStart : MonoBehaviour
{
    // Setup array for Scene objects and a list to store their paths
    public Object[] scenesToLoad;
    List<string> sceneList = new List<string>();

    // Setup full scene Paths to the Scene list on enable. Also set lightmapping delegates
    void Start()
    {
#if UNITY_EDITOR
        BuildSceneList();
#endif
        if (Application.isEditor)
        {
#if UNITY_EDITOR
            LoadSceneListInEditor();
#endif
        }
        else
        {
            LoadSceneListDuringPlay();
        }
    }

    void BuildSceneList()
    {
        if (scenesToLoad.Length == 0) // If no Scenes have been added, post a message and return false
        {
            Debug.Log("No Scene objects found");
        }
        else // If we do have Scenes in our public array, iterate through and add full file paths to our Scene list
        {
            sceneList.Clear();
            for (int i = 0; i < scenesToLoad.Length; i++)
            {
#if UNITY_EDITOR
                sceneList.Add(AssetDatabase.GetAssetPath(scenesToLoad[i]));
#endif
            }
        }
    }

    void LoadSceneListInEditor()
    {
        if (!Application.isPlaying)
        {
            for (int i = 0; i < scenesToLoad.Length; i++)
            {
#if UNITY_EDITOR
                EditorSceneManager.OpenScene(sceneList[i], OpenSceneMode.Additive);
#endif
            }
        }
    }

    void LoadSceneListDuringPlay()
    {
        if (Application.isPlaying)
        {
            for (int i = 0; i < scenesToLoad.Length; i++)
            {
                SceneManager.LoadScene(sceneList[i], LoadSceneMode.Additive);
            }
        }
    }
} 