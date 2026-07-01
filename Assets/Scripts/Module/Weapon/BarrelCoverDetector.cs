using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// 枪管掩体穿透检测器。
/// 挂在枪的 GameObject 上，配合 BaseGun.Fire() 使用。
///
/// 检测策略（覆盖5种情况）：
///   A. 擦边       → 放行
///   B. 一端穿入墙  → 阻挡（CheckSphere）
///   C. 整把枪埋进墙 → 阻挡（CheckSphere）
///   D. 枪口贴墙表面 → 放行（0.001f 不命中表面）
///   E. 横穿薄墙    → 阻挡（Linecast）
///
/// 预制体配置：
///   1. _barrelBox: 枪上挂一个 BoxCollider，覆盖枪管区域，Z轴（蓝箭头）朝枪口
///   2. barrierLayer: Inspector 勾选掩体层（如 spc），不配则自动设为 spc
///   3. barrierBlockUI: "请后退" UI 提示（可选）
/// </summary>
public class BarrelCoverDetector : MonoBehaviour
{
    [Header("掩体穿透检测")]
    public BoxCollider _barrelBox;              // 枪管检测盒（Z轴朝枪口）
    public LayerMask barrierLayer;              // 掩体物理层
    public GameObject barrierBlockUI;           // "请后退" UI 提示

    private bool _barrelBoxMissing;
    private bool _isBlocked;

    // 可视化调试（仅 Editor）
#if UNITY_EDITOR
    private bool _debugHit;
    private Vector3 _debugStart;
    private Vector3 _debugEnd;
    private int _debugHitLevel;                 // 0=无, 1=端点在墙内, 2=线段穿墙
    private string _debugHitInfo;
#endif

    void Start()
    {
        // barrierLayer 未配置时自动设为 spc
        if (barrierLayer.value == 0)
        {
            barrierLayer = LayerMask.GetMask("spc");
            Debug.Log($"[BarrelCover] {gameObject.name}: barrierLayer 自动设为 spc");
        }
    }

    /// <summary>
    /// 检测枪管是否深入掩体。
    /// 在 BaseGun.Fire() 开火前调用，返回 true 则禁止开火。
    /// </summary>
    /// <returns>true = 被掩体阻挡，应禁止开火</returns>
    public bool IsBlocked()
    {
        if (_barrelBox == null)
        {
            if (!_barrelBoxMissing)
            {
                Debug.LogWarning($"[BarrelCover] {gameObject.name}: 未找到枪管检测盒 _barrelBox，掩体检测已跳过");
                _barrelBoxMissing = true;
            }
            return false;
        }

        Transform t = _barrelBox.transform;
        float halfZ = _barrelBox.size.z * 0.5f;

        // 枪管盒两端世界坐标（+Z = 枪口端，-Z = 枪尾端）
        Vector3 startPoint = t.TransformPoint(_barrelBox.center + Vector3.forward * halfZ);
        Vector3 endPoint   = t.TransformPoint(_barrelBox.center - Vector3.forward * halfZ);

#if UNITY_EDITOR
        _debugStart = startPoint;
        _debugEnd = endPoint;
#endif

        // 1. 任一端在墙内部 → 阻挡（0.001f 保证贴表面不算）
        if (Physics.CheckSphere(startPoint, 0.001f, barrierLayer))
        {
            Debug.Log($"[BarrelCover] 枪口端在墙内! pos={startPoint}");
#if UNITY_EDITOR
            _debugHit = true;
            _debugHitLevel = 1;
            _debugHitInfo = "枪口端穿入";
#endif
            return true;
        }
        if (Physics.CheckSphere(endPoint, 0.001f, barrierLayer))
        {
            Debug.Log($"[BarrelCover] 枪尾端在墙内! pos={endPoint}");
#if UNITY_EDITOR
            _debugHit = true;
            _debugHitLevel = 1;
            _debugHitInfo = "枪尾端穿入";
#endif
            return true;
        }

        // 2. 线段穿墙 → 阻挡（两端在外但中间穿过薄墙）
        if (Physics.Linecast(startPoint, endPoint, barrierLayer))
        {
            Debug.Log($"[BarrelCover] 枪管线段穿墙! start={startPoint}, end={endPoint}");
#if UNITY_EDITOR
            _debugHit = true;
            _debugHitLevel = 2;
            _debugHitInfo = "线段穿墙";
#endif
            return true;
        }

#if UNITY_EDITOR
        _debugHit = false;
        _debugHitLevel = 0;
        _debugHitInfo = null;
#endif
        return false;
    }

    /// <summary>
    /// 更新阻挡 UI 状态。在 BaseGun.Fire() 中调用。
    /// </summary>
    public void UpdateBlockUI(bool blocked)
    {
        if (blocked == _isBlocked) return;
        _isBlocked = blocked;

        if (barrierBlockUI != null)
        {
            barrierBlockUI.SetActive(blocked);
        }

        if (blocked)
            Debug.Log($"[BarrelCover] 阻挡激活!");
        else
            Debug.Log($"[BarrelCover] 阻挡解除");
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        if (_barrelBox == null) return;

        Vector3 start = _debugStart;
        Vector3 end = _debugEnd;
        bool hit = _debugHit;

        // 枪管线段
        Gizmos.color = hit ? Color.red : Color.green;
        Gizmos.DrawLine(start, end);

        // 两端小球
        Gizmos.color = hit ? Color.red : Color.green;
        Gizmos.DrawWireSphere(start, 0.02f);
        Gizmos.DrawWireSphere(end, 0.02f);

        // 标签
        string label;
        if (_debugHitLevel == 0)
            label = "安全";
        else
            label = $"阻挡 L{_debugHitLevel}: {_debugHitInfo}";

        var style = new GUIStyle { fontSize = 13, fontStyle = FontStyle.Bold };
        style.normal.textColor = hit ? Color.red : Color.green;
        Handles.Label(start + Vector3.up * 0.05f, label, style);
    }
#endif
}
