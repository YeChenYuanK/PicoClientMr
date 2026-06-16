using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public enum BleedState
{
	NORMAL,
	DEAD,
	BREATH
}


public class BleedBehaviorByDrawMesh : MonoBehaviour
{
    public static BleedBehaviorByDrawMesh Instance;

	private MeshRenderer m_Renderer;
    private MeshFilter m_Mesh;
    private Material _material = null; 
	public float EdgeSharpness = 1; //>=1 //defines how sharp the resulting alpha map will be
	public BleedState hurtType = BleedState.NORMAL;
	public static float BloodAmount = 0; //0-1 //Set this at runtime
	public float TestingBloodAmount = 0.7f; //0-1 //only in Editor (for testing purposes)
	public static float minBloodAmount = 0; //0-1 //the minimum blood amount. You could optionally increase this (at runtime), the lower the player's HP is, to show the player has low health.

	public float minAlpha = 0; //0-1
	public float maxAlpha = 0.7f; //0-1
	public float distortion = 0f; //refraction: how much the original image is distorted through the blood (value depends on normal map)
	static public bool autoFadeOut = true; //automatically fades out the blood effect (by lowering the BloodAmount value over time)
	static public float autoFadeOutAbsReduc = 0.05f; //absolute reduction per seconde
	public float autoFadeOutRelReduc = 0.5f; //relative reduction per seconde

	public float updateSpeed = 20; // (1 / update duration) (how fast the effect updates to the new BloodAmount value)
	float prevBloodAmount = 0;

	private long lastTime = 0;

	private void Awake()
    {
        

	}
	// Start is called before the first frame update
	void Start()
    {
		Instance = this;
		_material = new Material(Shader.Find("Custom/SinglePassBleed"));
		Texture BloodDamage = Resources.Load<Texture>("Texture/Blood");
		Texture Normals = Resources.Load<Texture>("Texture/Blood_N");
		_material.SetTexture("_BlendTex", BloodDamage);
		//_material.SetTextureOffset("_BlendTex", new Vector2(-.3f,-.3f));
		//_material.SetTextureScale("_BlendTex", new Vector2(1.6f, 1.6f));
		_material.SetTexture("_BumpMap", Normals);
		//_material.SetTextureOffset("_BumpMap", new Vector2(-.3f, -.3f));
		//_material.SetTextureScale("_BumpMap", new Vector2(1.6f, 1.6f));
		_material.SetFloat("_EdgeSharpness", EdgeSharpness);
		lastTime = DateUtil.NowMllSec;
		m_Mesh = gameObject.AddComponent<MeshFilter>();
		m_Renderer = gameObject.AddComponent<MeshRenderer>();
		m_Renderer.material = _material;
		var mesh = new Mesh();
		m_Mesh.mesh = mesh;
		//m_Mesh.mesh = RenderingUtils.fullscreenMesh;

		Vector3[] vertices = new Vector3[4];

		float width = 1f;
		float height = 1f;
		float depth = 1f;

		vertices[0] = new Vector3(-width, -height, depth);
		vertices[1] = new Vector3(width, -height, depth);
		vertices[2] = new Vector3(-width, height, depth);
		vertices[3] = new Vector3(width, height, depth);
		mesh.vertices = vertices;

		int[] tri = new int[6];

		tri[0] = 0;
		tri[1] = 2;
		tri[2] = 1;

		tri[3] = 2;
		tri[4] = 3;
		tri[5] = 1;

		mesh.triangles = tri;

		Vector3[] normals = new Vector3[4];

		normals[0] = -Vector3.forward;
		normals[1] = -Vector3.forward;
		normals[2] = -Vector3.forward;
		normals[3] = -Vector3.forward;

		mesh.normals = normals;

		Vector2[] uv = new Vector2[4];

		uv[0] = new Vector2(0, 0);
		uv[1] = new Vector2(1, 0);
		uv[2] = new Vector2(0, 1);
		uv[3] = new Vector2(1, 1);

		mesh.uv = uv;
	}

    // Update is called once per frame
    void Update()
    {
		if (_material == null)
			return;
		if (hurtType == BleedState.DEAD)
		{
			BloodAmount = 1;
			maxAlpha = 0.7f;
			EdgeSharpness = 0.5f;
		}
		else if (hurtType == BleedState.BREATH)
		{
			BloodAmount = 1f;
			maxAlpha = 0.7f;
			EdgeSharpness = 0.7f;
		}
		else
		{
			maxAlpha = 0.5f;
			//EdgeSharpness = 1f;
			//if (autoFadeOut && BloodAmount > 0)
			//{
				float passTime = (float)(DateUtil.NowMllSec - lastTime) / 1000.0f;
				//if (passTime > 0)
				//Debug.Log("passTime:" + passTime);
				BloodAmount -= autoFadeOutAbsReduc * passTime;

				BloodAmount *= Mathf.Pow(1 - autoFadeOutRelReduc, passTime);
				BloodAmount = Mathf.Max(BloodAmount, 0);
				//Debug.Log("BloodAmount:" + BloodAmount + " , Time.deltaTime:" + Time.deltaTime + ",passTime:" + passTime);
			//}
			//else
			//{
			//	lastTime = DateUtil.NowMllSec;
			//	return;
			//}
		}
		Render();
		lastTime = DateUtil.NowMllSec;
	}

    private void Render()
    {
		//Blit方式需要shader主帖图名称为_MainTex
		float newBlendAmount = Mathf.Clamp01(BloodAmount) * (1 - minBloodAmount) + minBloodAmount;
		newBlendAmount = Mathf.Clamp01(newBlendAmount * (maxAlpha - minAlpha) + minAlpha);
		newBlendAmount = Mathf.Lerp(prevBloodAmount, newBlendAmount, Mathf.Clamp01(updateSpeed * Time.deltaTime));
		if(prevBloodAmount > 0)
        {
			_material.SetFloat("_BlendAmount", newBlendAmount);
			_material.SetFloat("_EdgeSharpness", EdgeSharpness);
        }
		prevBloodAmount = newBlendAmount;
	}
}
