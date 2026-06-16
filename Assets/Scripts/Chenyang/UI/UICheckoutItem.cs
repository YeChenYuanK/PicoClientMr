
using UnityEngine;
using UnityEngine.UI;

public class UICheckoutItem : MonoBehaviour
{
    public Transform Tran_Kills;
    public Transform Tran_HeadShot;
    public Transform Tran_Deads;
    public Transform Tran_Score;
    
    public Text Text_Name;
    public Text Text_Kills;
    public Text Text_HeadShot;
    public Text Text_Deads;
    public Text Text_Score;

    public GameObject isRedMvp;
    public GameObject isBuleMvp;
    
    public Sprite[] numImages; 
    public void SetData(PlayerInfo playerInfo, bool isMvp = false)
    {
        ShowNumber(playerInfo.Kills, Tran_Kills);
        ShowNumber(playerInfo.HeadShots, Tran_HeadShot);
        ShowNumber(playerInfo.Deads, Tran_Deads);
        ShowNumber(GameUtil.CountScore(playerInfo), Tran_Score);
        
        Text_Name.text = playerInfo.PlayerName;
        Text_Kills.text = playerInfo.Kills.ToString();
        Text_HeadShot.text = playerInfo.HeadShots.ToString();
        Text_Deads.text = playerInfo.Deads.ToString();
        Text_Score.text = GameUtil.CountScore(playerInfo).ToString();
        

        isRedMvp.SetActive(false);
        isBuleMvp.SetActive(false);
        if (playerInfo.Camp == 0)
        {
            isRedMvp.SetActive(isMvp);
        }
        else
        {
            isBuleMvp.SetActive(isMvp);
        }
    }
   
    public void ClearData()
    {
        Text_Name.text = "";
        Text_Kills.text = "";
        Text_HeadShot.text = "";
        Text_Deads.text = "";
        Text_Score.text = "";
        isRedMvp.SetActive(false);
        isBuleMvp.SetActive(false);
        
        ShowNumber(-1, Tran_Kills);
        ShowNumber(-1, Tran_HeadShot);
        ShowNumber(-1, Tran_Deads);
        ShowNumber(-1, Tran_Score);
        
        gameObject.SetActive(true);
    }

    void ShowNumber(int num, Transform parent)
    {
        if (numImages == null || numImages.Length == 0 || num == -1)
        {
            for (int i = 0; i < parent.childCount; i++)
            {
                Transform child = parent.GetChild(i);
                child.gameObject.SetActive(false);
            }
            return;
        }

        string numStr = num.ToString();
        int length = numStr.Length;
        int childCount = parent.childCount;

        for (int i = 0; i < childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child == null) continue;

            Image image = child.GetComponent<Image>();
            if (image == null) continue;

            if (i < length)
            {
                int digit = numStr[i] - '0';
                if (digit >= 0 && digit < numImages.Length)
                {
                    image.sprite = numImages[digit];
                    image.gameObject.SetActive(true);
                }
                else
                {
                    image.gameObject.SetActive(false);
                }
            }
            else
            {
                image.gameObject.SetActive(false);
            }
        }
    }
}


