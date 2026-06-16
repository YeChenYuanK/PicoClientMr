using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/BloodOverlay")]
public class BleedBehavior : MonoBehaviour
{
    static public float BloodAmount = 0; //0-1 //Set this at runtime
    public float TestingBloodAmount = 0.5f; //0-1 //only in Editor (for testing purposes)

    static public float minBloodAmount = 0; //0-1 //the minimum blood amount. You could optionally increase this (at runtime), the lower the player's HP is, to show the player has low health.

    public float EdgeSharpness = 1; //>=1 //defines how sharp the resulting alpha map will be
    public float minAlpha = 0; //0-1
    public float maxAlpha = 1; //0-1
    public float distortion = 0.2f; //refraction: how much the original image is distorted through the blood (value depends on normal map)

    static public bool autoFadeOut = true; //automatically fades out the blood effect (by lowering the BloodAmount value over time)
    static public float autoFadeOutAbsReduc = 0.05f; //absolute reduction per seconde
    public float autoFadeOutRelReduc = 0.5f; //relative reduction per seconde

    public float updateSpeed = 20; // (1 / update duration) (how fast the effect updates to the new BloodAmount value)
    float prevBloodAmount = 0;

    public Texture2D DeadImage; 
    public Texture2D GameOverImage;
    public Texture2D GameWinImage;
    //public Texture2D GreenImg; //RGBA
    public Texture2D Normals; //normalmap
    public Shader Shader; //ImageBlendEffect.shader

    private Material _material;

    private Material _materialL;
    private Material _materialR;
    private Material _materialF;
    private Material _materialDead;
    private Material _materialGameOver;
    private Material curMaterial;
    private Material _materialWinOver;

    private void Awake()
    {
        _material = new Material(Shader);
        _material.SetTexture("_BlendTex", DeadImage);
        _material.SetTexture("_BumpMap", Normals);
        _materialL = new Material(Shader);
        _materialL.SetTexture("_BlendTex", Resources.Load<Texture>("Texture/Blood_L"));
        _materialL.SetTexture("_BumpMap", Resources.Load<Texture>("Texture/Blood_N"));
        _materialR = new Material(Shader);
        _materialR.SetTexture("_BlendTex", Resources.Load<Texture>("Texture/Blood_R"));
        _materialR.SetTexture("_BumpMap", Resources.Load<Texture>("Texture/Blood_N"));
        _materialF = new Material(Shader);
        _materialF.SetTexture("_BlendTex", Resources.Load<Texture>("Texture/Blood_F"));
        _materialF.SetTexture("_BumpMap", Resources.Load<Texture>("Texture/Blood_N"));
        _materialDead = new Material(Shader);
        _materialDead.SetTexture("_BlendTex", DeadImage);
        _materialDead.SetTexture("_BumpMap", Normals);
        _materialGameOver = new Material(Shader);
        _materialGameOver.SetTexture("_BlendTex", GameOverImage);
        _materialGameOver.SetTexture("_BumpMap", Normals);
        _materialWinOver = new Material(Shader);
        _materialWinOver.SetTexture("_BlendTex", GameWinImage);
        _materialWinOver.SetTexture("_BumpMap", Normals);
    }

    public void Update()
    {
        if (autoFadeOut && BloodAmount > 0)
        {
            BloodAmount -= autoFadeOutAbsReduc * Time.deltaTime;
            BloodAmount *= Mathf.Pow(1 - autoFadeOutRelReduc, Time.deltaTime);
            BloodAmount = Mathf.Max(BloodAmount, 0);
        }
        /*
        if (autoFadeOut && TestingBloodAmount > 0)
        {
            TestingBloodAmount -= autoFadeOutAbsReduc * Time.deltaTime;
            TestingBloodAmount *= Mathf.Pow(1 - autoFadeOutRelReduc, Time.deltaTime);
            TestingBloodAmount = Mathf.Max(TestingBloodAmount, 0);
        }
        */
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        
        if (!Application.isPlaying)
        {
            if(_material == null)
            {
                _material = new Material(Shader);
            }
            _material.SetTexture("_BlendTex", DeadImage);
            _material.SetTexture("_BumpMap", Normals);
            curMaterial = _material;
            //float newBlendAmount = Mathf.Clamp01(Mathf.Clamp01(TestingBloodAmount) * (maxAlpha - minAlpha) + minAlpha);
            //newBlendAmount = newBlendAmount * (1 - minBloodAmount) + minBloodAmount;
            float newBlendAmount = Mathf.Clamp01(TestingBloodAmount) * (1 - minBloodAmount) + minBloodAmount;
            newBlendAmount = Mathf.Clamp01(newBlendAmount * (maxAlpha - minAlpha) + minAlpha);
            newBlendAmount = Mathf.Lerp(prevBloodAmount, newBlendAmount, Mathf.Clamp01(updateSpeed * Time.deltaTime));
            _material.SetFloat("_BlendAmount", newBlendAmount);
            prevBloodAmount = newBlendAmount;
        }
        else
        {
            if (curMaterial == null)
            {
                curMaterial = _material;
            }
            //float newBlendAmount = Mathf.Clamp01(Mathf.Clamp01(BloodAmount) * (maxAlpha - minAlpha) + minAlpha);
            //newBlendAmount = newBlendAmount * (1 - minBloodAmount) + minBloodAmount;
            float newBlendAmount = Mathf.Clamp01(BloodAmount) * (1 - minBloodAmount) + minBloodAmount;
            newBlendAmount = Mathf.Clamp01(newBlendAmount * (maxAlpha - minAlpha) + minAlpha);
            newBlendAmount = Mathf.Lerp(prevBloodAmount, newBlendAmount, Mathf.Clamp01(updateSpeed * Time.deltaTime));
            curMaterial.SetFloat("_BlendAmount", newBlendAmount);
            prevBloodAmount = newBlendAmount;
        }

        curMaterial.SetFloat("_EdgeSharpness", EdgeSharpness);
        curMaterial.SetFloat("_Distortion", distortion);

        Graphics.Blit(source, destination, curMaterial);
    }

    public void ChangeMaterial(HurtDirection hurtDirect)
    {
        if(hurtDirect == HurtDirection.DEAD)
        {
            curMaterial = _materialDead;
        } else if(hurtDirect == HurtDirection.GAME_OVER)
        {
            curMaterial = _materialGameOver;
        } else if(hurtDirect == HurtDirection.GAME_OVER_WIN)
        {
            curMaterial = _materialWinOver;
        } else
        {
            curMaterial = _materialDead;
        }
    }

}