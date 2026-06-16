using System.Collections;
using UnityEngine;

public class LoadNext : MonoBehaviour
{
    public float time = 2f;

    // Start is called before the first frame update
    void Start()
    {
        this.StartCoroutine(LoadNextDelay());
    }

    private IEnumerator LoadNextDelay()
    {
        yield return new WaitForSeconds(time);
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(SceneLoader.Instance.NextLevelMap);
        Debug.Log("already load next level map : " + SceneLoader.Instance.NextLevelMap);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
