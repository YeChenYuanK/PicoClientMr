using UnityEngine;
using System.Collections;
using System.Threading;

public class RobotAIManager {

    private static RobotAIManager instance;

    public static RobotAIManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new RobotAIManager();
            }
            return instance;
        }
    }
    
    public RobotAIManager()
    {
    }

    /// <summary>
    /// 是否是AI主机
    /// </summary>
    public bool IsHost { get{ return isHost; } set{ isHost = value;} }

    private bool isHost = false;

    private int robotGenIndex = 0;

    

    /// <summary>
    /// 运行主方法
    /// </summary>
    public void OnTick()
    {
        if (!isHost) return;

        // 创建机器人数据
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public void Init(OperationTigger operationTrigger)
    {
        operationTrigger.AddSysOperTrigger(new CreateRobotHandler());
    }

}
