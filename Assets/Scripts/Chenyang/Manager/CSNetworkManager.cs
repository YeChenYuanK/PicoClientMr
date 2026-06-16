using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class CSNetworkManager : NetworkManager
{
    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        Debug.Log($"[NET-DIAG] OnServerConnect: connectionId={conn.connectionId}, address={conn.address}, totalConnections={NetworkServer.connections.Count}");
        base.OnServerConnect(conn);
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        Debug.LogWarning($"[NET-DIAG] OnServerDisconnect: connectionId={conn.connectionId}, address={conn.address}, totalConnections={NetworkServer.connections.Count}");
        base.OnServerDisconnect(conn);
    }

    public override void OnClientConnect()
    {
        Debug.Log($"[NET-DIAG] OnClientConnect: connected to server");
        base.OnClientConnect();
    }

    public override void OnClientDisconnect()
    {
        Debug.LogWarning($"[NET-DIAG] OnClientDisconnect: disconnected from server");
        base.OnClientDisconnect();
    }


}
