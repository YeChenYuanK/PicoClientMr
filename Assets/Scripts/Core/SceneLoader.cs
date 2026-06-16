using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    // public SteamVR_LoadLevel steamLoader;

    private static SceneLoader instance;

    public static SceneLoader Instance
    {
        get
        {
            return instance;
        }
    }

    private AsyncOperation async_operation;

    // public OVROverlay cubemapOverlay;
    // public OVROverlay loadingTextQuadOverlay;

    public float distanceFromCamToLoadText;

    public string NextLevelMap;

    void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        StartCoroutine(BootMemSnapshot());
    }

    // 启动 8 秒后打一次内存快照,捕获首个场景(开机场景)的资源占用,真机抓 logcat 分析用。
    private IEnumerator BootMemSnapshot()
    {
        yield return new WaitForSeconds(8f);
        LogMemory("[MEM] boot-snapshot");
        DumpHeavyAssets("[MEM] boot-snapshot");
    }

    // Update is called once per frame
    void Update()
    {

        if(this.async_operation != null)
        {
            if(this.async_operation.isDone)
            {
                // 加载完成
                this.async_operation = null;
                Debug.Log("async_operation isdone");
                // PhotonNetwork.OnSceneLoadedNetworkHandler();
            }
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        // 整场切换时回收一次无引用资源,做内存保护;Additive 叠加加载不处理。
        if (loadSceneMode != LoadSceneMode.Single) return;
        StartCoroutine(UnloadAndCollect(scene.name));
    }

    private IEnumerator UnloadAndCollect(string sceneName)
    {
        LogMemory("[MEM] before-unload scene=" + sceneName);
        yield return Resources.UnloadUnusedAssets();   // 异步,须等完成
        System.GC.Collect();
        LogMemory("[MEM] after-unload  scene=" + sceneName);
        DumpHeavyAssets("[MEM] after-unload scene=" + sceneName);
    }

    // 把当前已加载的贴图/网格/音频按运行时占用从大到小打到日志,定位内存大头。
    public static void DumpHeavyAssets(string tag, int topN = 20)
    {
        DumpHeavy<Texture>(tag, "TEX", topN, o =>
        {
            Texture tx = (Texture)o;
            string fmt = (o is Texture2D t2) ? t2.format.ToString() : tx.dimension.ToString();
            return tx.width + "x" + tx.height + " " + fmt;
        });
        DumpHeavy<Mesh>(tag, "MESH", topN, o => ((Mesh)o).vertexCount + "v");
        DumpHeavy<AudioClip>(tag, "AUDIO", topN, o => ((AudioClip)o).length.ToString("F1") + "s");
    }

    private static void DumpHeavy<T>(string tag, string kind, int topN, System.Func<UnityEngine.Object, string> info)
        where T : UnityEngine.Object
    {
        const float MB = 1024f * 1024f;
        T[] all = Resources.FindObjectsOfTypeAll<T>();
        List<KeyValuePair<long, UnityEngine.Object>> sized = new List<KeyValuePair<long, UnityEngine.Object>>(all.Length);
        long total = 0;
        foreach (UnityEngine.Object o in all)
        {
            long sz = UnityEngine.Profiling.Profiler.GetRuntimeMemorySizeLong(o);
            total += sz;
            sized.Add(new KeyValuePair<long, UnityEngine.Object>(sz, o));
        }
        sized.Sort((a, b) => b.Key.CompareTo(a.Key));
        Debug.Log(string.Format("{0} {1} 总数={2} 总占用={3:F1}MB Top{4}:", tag, kind, all.Length, total / MB, topN));
        int n = Mathf.Min(topN, sized.Count);
        for (int i = 0; i < n; i++)
        {
            Debug.Log(string.Format("  [{0}] {1:F2}MB {2} name={3}",
                kind, sized[i].Key / MB, info(sized[i].Value), sized[i].Value.name));
        }
    }

    public static void LogMemory(string tag)
    {
        long totalReserved = UnityEngine.Profiling.Profiler.GetTotalReservedMemoryLong();
        long totalAllocated = UnityEngine.Profiling.Profiler.GetTotalAllocatedMemoryLong();
        long monoUsed = UnityEngine.Profiling.Profiler.GetMonoUsedSizeLong();
        long gcManaged = System.GC.GetTotalMemory(false);
        const float MB = 1024f * 1024f;
        Debug.Log(string.Format(
            "{0} reserved={1:F1}MB allocated={2:F1}MB mono={3:F1}MB gc={4:F1}MB",
            tag,
            totalReserved / MB,
            totalAllocated / MB,
            monoUsed / MB,
            gcManaged / MB));
    }

    public void DisableOverlay()
    {
        //loadingTextQuadOverlay.enabled = false;
        //cubemapOverlay.enabled = false;
    }



    //根据输入名加载场景  
    public void StartScene(string sceneName)
    {
        Debug.Log("SceneLoader StartScene " + sceneName);
    
        Debug.Log("SceneLoader StartScene " + sceneName + " PhotonNetwork.LoadLevelBefore end");
        this.NextLevelMap = sceneName;
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("LoadingScene");
        /*
        if(VRScreenFade.instance != null)
        {
            VRScreenFade.instance.FadeOut();
            GameObject nopostcmaeraObj = GameObject.Find("Camera-nopost");
            if(nopostcmaeraObj != null)
            {
                nopostcmaeraObj.GetComponent<Camera>().enabled = false;
            }
            this.StartCoroutine(LoadScene1(sceneName));
        } else
        {
            this.StartCoroutine(LoadScene0(sceneName));
        }
        */
        // this.StartCoroutine(LoadScene0(sceneName));
        // LoadingCompositorUtil.ShowLoadingLayer();
    }

    private IEnumerator LoadScene0(string sceneName)
    {
        yield return new WaitForSeconds(0.1f);
        Debug.Log("SceneLoader StartScene " + sceneName + " LoadScene0");
        // this.async_operation = 
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    private IEnumerator LoadScene1(string sceneName)
    {
        yield return new WaitForSeconds(1.2f);
        Debug.Log("SceneLoader StartScene " + sceneName + " LoadScene1");
        this.async_operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
    }

    //异步加载场景  
    IEnumerator LoadScene(string sceneName)
    {

        async_operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
        yield return async_operation;

    }

}
