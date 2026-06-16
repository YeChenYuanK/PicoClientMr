using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.gamestudio.tank;
using com.gamestudio.cs;
using LekeNet.Room;

public class OperationHandler  {

    private int gameOperCmd;

    public OperationHandler()
    {

    }

    public OperationHandler(int gameOper)
    {
        this.GameOperCmd = (int)gameOper;
    }

    public OperationHandler(GameOper gameOper)
    {
        this.GameOperCmd = (int)(int)gameOper;
    }

    public int GameOperCmd
    {
        get
        {
            return gameOperCmd;
        }

        set
        {
            gameOperCmd = value;
        }
    }

    public virtual bool OnOperation(RoomOperation operation)
    {
        return false;
    }
}

public class OperationTigger {

    private static OperationTigger instance;

    public static OperationTigger Instance {
        get {
            if(instance == null)
            {
                instance = new OperationTigger();
            }
            return instance;
        }
    }

    private Dictionary<SysOper, OperationHandler> sysOperMap = new Dictionary<SysOper, OperationHandler>();
    private Dictionary<GameOper, OperationHandler> operMap = new Dictionary<GameOper, OperationHandler>();

    public void AddTrigger(OperationHandler operationHandler)
    {
        operMap[(GameOper)operationHandler.GameOperCmd] = operationHandler;
    }

    public void AddSysOperTrigger(OperationHandler operationHandler)
    {
        sysOperMap[(SysOper)operationHandler.GameOperCmd] = operationHandler;
    }

    public void OnTrigger(RoomOperation oper)
    {
        if(operMap.ContainsKey((GameOper)oper.operId))
        {
            operMap[(GameOper)oper.operId].OnOperation(oper);
        } else
        {
            Debug.Log("operation not found : " + (GameOper)oper.operId);
        }
    }

    public void OnSysOperTrigger(RoomOperation oper)
    {
        if (sysOperMap.ContainsKey((SysOper)oper.operId))
        {
            sysOperMap[(SysOper)oper.operId].OnOperation(oper);
        }
        else
        {
            Debug.Log("sys operation not found : " + (SysOper)oper.operId);
        }
    }
}
