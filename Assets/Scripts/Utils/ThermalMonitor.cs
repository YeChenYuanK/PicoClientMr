using UnityEngine;

/// <summary>
/// 监控PICO头显温度状态，过热时自动降帧保护
/// 挂载到不销毁的GameObject上（如GameMng所在物体）
/// </summary>
public class ThermalMonitor : MonoBehaviour
{
    [Header("帧率配置")]
    [SerializeField] private int normalFrameRate = 72;
    [SerializeField] private int thermalFrameRate = 60;
    
    [Header("监控配置")]
    [SerializeField] private float checkInterval = 5f; // 每5秒检测一次
    
    private float _lastCheckTime;
    private bool _isThermalThrottled = false;
    private AndroidJavaObject _thermalService;
    private bool _isAndroid = false;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        _isAndroid = true;
        InitThermalMonitor();
#endif
        Debug.Log($"[NET-DIAG] ThermalMonitor started. NormalFPS={normalFrameRate}, ThermalFPS={thermalFrameRate}");
    }

    private void Update()
    {
        if (Time.time - _lastCheckTime >= checkInterval)
        {
            _lastCheckTime = Time.time;
            SceneLoader.LogMemory("[MEM] periodic t=" + (int)Time.time + "s");
            if (_isAndroid) CheckThermalStatus();
        }
    }

    private void InitThermalMonitor()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        try
        {
            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                // 获取PowerManager来检查温度状态
                _thermalService = activity.Call<AndroidJavaObject>("getSystemService", "power");
                Debug.Log("[NET-DIAG] ThermalMonitor: PowerManager initialized");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"[NET-DIAG] ThermalMonitor init failed: {e.Message}");
        }
#endif
    }

    private void CheckThermalStatus()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        try
        {
            // Android API 29+ 支持 getCurrentThermalStatus()
            // 返回值: 0=None, 1=Light, 2=Moderate, 3=Severe, 4=Critical, 5=Emergency, 6=Shutdown
            int thermalStatus = _thermalService.Call<int>("getCurrentThermalStatus");
            
            bool shouldThrottle = thermalStatus >= 2; // Moderate及以上降帧
            
            if (shouldThrottle && !_isThermalThrottled)
            {
                // 开始降帧
                _isThermalThrottled = true;
                Application.targetFrameRate = thermalFrameRate;
                Debug.LogWarning($"[NET-DIAG] THERMAL WARNING: status={thermalStatus}, reducing FPS to {thermalFrameRate}");
            }
            else if (!shouldThrottle && _isThermalThrottled)
            {
                // 恢复帧率
                _isThermalThrottled = false;
                Application.targetFrameRate = normalFrameRate;
                Debug.Log($"[NET-DIAG] Thermal recovered: status={thermalStatus}, restoring FPS to {normalFrameRate}");
            }
            
            // 每次检测都记录（只在有变化或过热时记录）
            if (thermalStatus > 0)
            {
                Debug.Log($"[NET-DIAG] Thermal status: {thermalStatus} (0=None,1=Light,2=Moderate,3=Severe,4=Critical)");
            }
        }
        catch (System.Exception e)
        {
            // getCurrentThermalStatus 需要 API 29+，低版本会失败
            // 降级方案：通过电池温度判断
            CheckBatteryTemperature();
        }
#endif
    }

    private void CheckBatteryTemperature()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        try
        {
            using (AndroidJavaObject filter = new AndroidJavaObject("android.content.IntentFilter", "android.intent.action.BATTERY_CHANGED"))
            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                AndroidJavaObject batteryIntent = activity.Call<AndroidJavaObject>("registerReceiver", (AndroidJavaObject)null, filter);
                
                if (batteryIntent != null)
                {
                    // 温度单位是0.1°C
                    int temperature = batteryIntent.Call<int>("getIntExtra", "temperature", 0);
                    float tempCelsius = temperature / 10f;
                    
                    bool shouldThrottle = tempCelsius >= 40f; // 40°C以上降帧
                    
                    if (shouldThrottle && !_isThermalThrottled)
                    {
                        _isThermalThrottled = true;
                        Application.targetFrameRate = thermalFrameRate;
                        Debug.LogWarning($"[NET-DIAG] THERMAL WARNING: battery temp={tempCelsius}°C, reducing FPS to {thermalFrameRate}");
                    }
                    else if (!shouldThrottle && _isThermalThrottled && tempCelsius < 37f) // 降到37°C以下才恢复
                    {
                        _isThermalThrottled = false;
                        Application.targetFrameRate = normalFrameRate;
                        Debug.Log($"[NET-DIAG] Thermal recovered: battery temp={tempCelsius}°C, restoring FPS to {normalFrameRate}");
                    }
                    
                    if (tempCelsius >= 35f)
                    {
                        Debug.Log($"[NET-DIAG] Battery temperature: {tempCelsius}°C");
                    }
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"[NET-DIAG] Battery temperature check failed: {e.Message}");
        }
#endif
    }

    /// <summary>
    /// 获取当前是否处于过热降帧状态
    /// </summary>
    public bool IsThermalThrottled => _isThermalThrottled;
}
