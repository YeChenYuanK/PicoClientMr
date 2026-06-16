using UnityEngine;
using System.Collections;

public class BaseNode : MonoBehaviour {

    public int id = -1;

    void OnDrawGizmos()
    {
#if UNITY_EDITOR
        Gizmos.DrawIcon(transform.position, "Node.tif");
#endif
    }

    // Use this for initialization
    void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
