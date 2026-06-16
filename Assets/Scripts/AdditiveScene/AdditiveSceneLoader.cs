using UnityEngine;

using System.Collections.Generic;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

[ExecuteInEditMode]
public class AdditiveSceneLoader : MonoBehaviour
{
    // Setup array for Scene objects and a list to store their paths
    public Object[] scenesToLoad;
    private List<string> sceneList = new List<string>();

    // Button labels
    private string loadButton;
    const string loadAllScenesLabel = "Additvely Load All Scenes";
    const string unloadAllScenesLabel = "Unload All Scenes";
    const string noObjectsFoundLabel = "No Scene objects found";

    // Setup full scene Paths to the Scene list on enable. Also set lightmapping delegates
    void OnEnable()
    {
        loadButton = unloadAllScenesLabel;

        sceneList.Clear();
        if (scenesToLoad.Length == 0) //If no Scenes have been added, post a message and return false
        {
            Debug.Log(noObjectsFoundLabel);
        }
        else // If we do have Scenes in our public array, iterate through and add full file paths to our Scene list
        {
#if UNITY_EDITOR
            for (int i = 0; i < scenesToLoad.Length; i++)
            {
                sceneList.Add(AssetDatabase.GetAssetPath(scenesToLoad[i]));
            }
#endif
        }
    }

    // Functions to return button labels to UI
    public string GetLoadButtonLabel()
    {
        return loadButton;
    }

    // Receives a Scene list index which is then loaded additvely
    public void SceneToggle(int sceneNumber)
    {
        if (Application.isPlaying && sceneNumber <= sceneList.Count)
        {
            Scene sceneToToggle = SceneManager.GetSceneByPath(sceneList[sceneNumber]);
            if (!sceneToToggle.isLoaded) // If Scene is not loaded, load additvely
            {
                SceneManager.LoadScene(sceneList[sceneNumber], LoadSceneMode.Additive);
            }

            else  // Otherwise, if Scene is loaded, unload
            {
                SceneManager.UnloadSceneAsync(sceneList[sceneNumber]);
            }
        }
    }

    // Iterate through Scene list and if any sceneObjects are not loaded use SceneManager to load additively during play
    public void ToggleAllScenesDuringPlay()
    {
        bool allScenesLoaded = true;
        if (Application.isPlaying)
        {
            for (int i = 0; i < sceneList.Count; i++)
            {
                if (!SceneManager.GetSceneByPath(sceneList[i]).isLoaded) // If any Scene is found to not be loaded, addively load
                {
                    SceneManager.LoadScene(sceneList[i], LoadSceneMode.Additive);
                    allScenesLoaded = false;
                }
                if (allScenesLoaded == true) // Otherwise, if all all Scenes are loaded, then unload instead
                {
                    SceneManager.UnloadSceneAsync(sceneList[i]);
                    allScenesLoaded = true;
                }
            }
        }
    }

    // Iterates through Scene name array and uses EditorSceneManager to load during editor time
    public void LoadAllScenesInEditor()
    {
#if UNITY_EDITOR
        if (Application.isEditor && !Application.isPlaying)
        {
            for (int i = 0; i < sceneList.Count; i++)
            {
                if (!SceneManager.GetSceneByPath(sceneList[i]).isLoaded) // If any Scene is found to not be loaded, addively load
                {
                    EditorSceneManager.OpenScene(sceneList[i], OpenSceneMode.Additive);
                    loadButton = unloadAllScenesLabel;
                }
                else // Otherwise, if all all Scenes are loaded, then unload instead
                {
                    EditorSceneManager.CloseScene(SceneManager.GetSceneByPath(sceneList[i]), true);
                    loadButton = loadAllScenesLabel;
                }
            }
        }
#endif
    }
} 