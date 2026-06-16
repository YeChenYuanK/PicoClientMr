using com.gamestudio.cs;
using UnityEngine;
using static DataManager;

public class OtherCharactor : BaseCharacter {

	public CSPlayer _player;
	public GameObject ProtectObj;
	public void ShowProtect(bool show)
	{
		ChangeAnim(false);
		//if (show)
		//	ProtectObj.transform.position = new Vector3(PlayerInfo.SelfPos.x, this.ProtectObj.transform.position.y, PlayerInfo.SelfPos.z);
		ObjectUtil.UpdateObjectActive(ProtectObj, show);
	}


	
	public GameObject soldierMesh1;
	public GameObject soldierMesh2;
	public TextMesh playerNameTextMesh1;
	public TextMesh playerNameTextMesh2;
	public GameObject RedLogo;
	public GameObject BlueLogo;
	public void ShowLogo(bool show)
	{
		RedLogo.SetActive(show);
		BlueLogo.SetActive(show);
	}
	public void OnPlayerNameChange(string name)
	{
		playerNameTextMesh1.text = name;
		playerNameTextMesh2.text = name;
		
	}
	public Animator anim { get; private set; }
	public Animator dieAnim;
	public Animator currAnim { get; private set; }

	public GameObject deadShowFlag;
	public GameObject headshotDeadShowFlag;
	
    public void ShowDieFlag(int part)
	{
		// 爆头击杀
		if ((CharacterPart)part == CharacterPart.HEAD)
			headshotDeadShowFlag.SetActive(true);
		else
			deadShowFlag.SetActive(true);

		ChangeAnim(true);
	}
	public void ChangeAnim(bool isDie)
	{
		ObjectUtil.UpdateObjectActive(dieAnim.gameObject, isDie);
		if(anim!=null)
			ObjectUtil.UpdateObjectActive(anim.gameObject, !isDie);
		if (isDie)
		{
			currAnim = dieAnim;
		}
		else
		{
			currAnim = anim;
		}
	}
	
	public float lastXangle = 0;
	public float aimVelocity = 0;
	public Vector3 moveDirect;
	
	public void UpdateAnim(float XAngle,float Hight)
	{
		if (currAnim == null)
		{
			if (_player.Camp != -1)
				OnChangeCamp(_player.Camp);
			else
				return;
		}
	
		float currAiming = XAngle;

		if (lastXangle == 0)
		{
			lastXangle = XAngle;
		}
		else
		{
			float tempAngle = Mathf.SmoothDampAngle(lastXangle, XAngle, ref aimVelocity, 0.1f);
			lastXangle = currAiming = tempAngle;
		}
		if (XAngle > 180)
		{
			currAiming = XAngle - 360;
		}
		if (currAnim.isActiveAndEnabled)
		{
			
			currAiming = Mathf.Clamp(-currAiming / 45, -1f, 1f);
			currAnim.SetFloat("AimingBleed", currAiming);
			currAnim.SetFloat("Hvalue", moveDirect.x);
			currAnim.SetFloat("Vvalue", moveDirect.z);
			currAnim.SetFloat("Hight", Hight);
		}
	}
    public void ShowName(bool show)
    {
		if (playerNameTextMesh1 != null && playerNameTextMesh2 != null)
		{
			playerNameTextMesh1.gameObject.SetActive(show);
			playerNameTextMesh2.gameObject.SetActive(show);
		}
	}
    public void OnChangeCamp(int camp)
	{
		if (_player.isLocalPlayer) return;
		//if (_player.isServer || camp == GameMng.Instance._playerInfoMng.MySelfCamp)
		//{
			//ShowName(true);
		//}
		//else
			//ShowName(false);
		ShowName(true);
		GameObject activeSkin = null;
		if (camp == (int)PlayerCamp.Red)
		{
			//this.playerNameTextMesh1.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.red);
			//this.playerNameTextMesh2.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.red);
			this.playerNameTextMesh1.color = Color.red;
			this.playerNameTextMesh2.color = Color.red;
			this.soldierMesh1.SetActive(true);
			this.soldierMesh2.SetActive(false);
			activeSkin = this.soldierMesh1;

		}
		else
		{
			//this.playerNameTextMesh1.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.blue);
			//this.playerNameTextMesh2.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.blue);
			this.playerNameTextMesh1.color = Color.blue;
			this.playerNameTextMesh2.color = Color.blue;
			this.soldierMesh1.SetActive(false);
			this.soldierMesh2.SetActive(true);
			activeSkin = this.soldierMesh2;
		}
		anim = activeSkin.GetComponent<Animator>();
		if(!_player.isDie)
			currAnim = activeSkin.GetComponent<Animator>();
		_player.gunController.ChangeGunById(_player.WeaponId);

	}
	public Transform CameraPoint;
	


   



    public void ChangePoint(BasePoint point)
    {
        this.transform.SetParent(point.telePorter.transform);
        this.transform.localPosition = Vector3.zero;
    }

    public override void Rebirth(FightUnit fightUnit)
    {
        base.Rebirth(fightUnit);
        this.gameObject.SetActive(true);
        BasePoint point = TeleporterManager.Instance.FindBasePoint(fightUnit.BirthIndex);
        if (point != null)
        {
            this.transform.SetParent(point.telePorter.transform);
            ChangePoint(point);
        }
    }

   
}
