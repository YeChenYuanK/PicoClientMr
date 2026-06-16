using UnityEngine;
using System.Collections;

public class CharacterSkin : MonoBehaviour 
{

	public SkinnedMeshRenderer renderer;

	public Material head1;
	public Material head2;
	public Material body1;
	public Material body2;

	Camp camp;

	public void Init(Camp camp)
	{
		this.camp = camp;
		if(camp == Camp.Blue)
		{
			renderer.materials = new Material[2]{ body1,  head1};
		}else if(camp == Camp.Red)
		{
			renderer.materials = new Material[2]{body2 ,head2};
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
