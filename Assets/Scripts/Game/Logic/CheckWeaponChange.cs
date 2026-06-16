using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckWeaponChange : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        WeaponSelectItem item = other.gameObject.GetComponent<WeaponSelectItem>();
        if (item != null)
        {
            int targetWeaponId = item.WeaponId;
            CSPlayer player =GameMng.Instance._mySelf;
            if (player != null)
            {
                player.CmdSetWeaponid(targetWeaponId);
               
                Debug.Log("Ö÷½ÇÇÐ»»Ç¹Ö§ £º " + targetWeaponId);
            }
        }
    }
}
