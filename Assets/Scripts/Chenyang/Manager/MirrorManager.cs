using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Mirror.Discovery;

public class MirrorManager : MonoBehaviour
{
  
    public NetworkDiscovery networkDiscovery;
    readonly Dictionary<long, ServerResponse> discoveredServers = new Dictionary<long, ServerResponse>();
   
#if UNITY_EDITOR
    void OnValidate()
    {
        if (networkDiscovery == null)
        {
            networkDiscovery = GetComponent<NetworkDiscovery>();
            UnityEditor.Events.UnityEventTools.AddPersistentListener(networkDiscovery.OnServerFound, OnDiscoveredServer);
            UnityEditor.Undo.RecordObjects(new Object[] { this, networkDiscovery }, "Set NetworkDiscovery");
        }
    }
#endif
    public IEnumerator HostServerOrClient(bool isHost)
    {
        if (isHost)
        {
            Debug.Log("[NET-DIAG] HostServerOrClient: starting as HOST (server)");
            discoveredServers.Clear();
            NetworkManager.singleton.StartServer();
            networkDiscovery.AdvertiseServer();
        }
        else
        {
            Debug.Log("[NET-DIAG] HostServerOrClient: starting as CLIENT (discovery)");
            discoveredServers.Clear();
            networkDiscovery.StartDiscovery();
            yield return Client_FindServer_Connect();
        }
        yield break;
    }

    public void OnDiscoveredServer(ServerResponse info)
    {
        // Note that you can check the versioning to decide if you can connect to the server or not using this method
        discoveredServers[info.serverId] = info;
    }
    IEnumerator Client_FindServer_Connect()
    {
        while (true)
        {
            if (discoveredServers.Count > 0)
            {
                foreach (ServerResponse sr in discoveredServers.Values)
                {
                    networkDiscovery.StopDiscovery();
                    NetworkManager.singleton.StartClient(sr.uri);
                   yield break;
                }
            }
            yield return null;
        }
      
    }
   
}
