using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private GameMng _GameMng;
    public GameMng Mng
    {
        get
        {
            if (_GameMng == null)
                _GameMng = GameMng.Instance;
            return _GameMng;
        }
    }
    public GameObject UICamera;
    public void OnInit()
    {
        if (Mng.isServerHost)
            UICamera.gameObject.SetActive(true);
        else
            Destroy(UICamera.gameObject);
    }
}
