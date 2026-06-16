using UnityEngine;
using System.Collections;

public class DebugInputManager : MonoBehaviour {

	public EventHandle eventHandle= new EventHandle ();

	float mouseX, mouseY, mouseZ = 0;
	public Transform cameraTrans;
	public Quaternion lastQuaternion;
	public TeleportLazer lazer;

	public Transform HeadTarget;
	public Transform LeftTarget;
	public Transform RightTarget;
	public GunController gunController;


	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		#if UNITY_EDITOR
		CheckHeadAngle();
		CheckGripHandle();
		#endif
	}

	public void CheckHeadAngle()
	{
		Quaternion rot;
		bool rolled = false;
		if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
		{
			rolled = true;
			mouseZ += Input.GetAxis("Mouse X") * 5;
			mouseZ = Mathf.Clamp(mouseZ, -85, 85);
		}
		else
		{
			mouseX += Input.GetAxis("Mouse X") * 5;
			if (mouseX <= -180)
			{
				mouseX += 360;
			}
			else if (mouseX > 180)
			{
				mouseX -= 360;
			}
			mouseY -= Input.GetAxis("Mouse Y") * 2.4f;
			mouseY = Mathf.Clamp(mouseY, -65, 65);
		}
		rot = Quaternion.Euler(mouseY, mouseX, mouseZ);
		transform.localEulerAngles = new Vector3(rot.eulerAngles.x, rot.eulerAngles.y, rot.eulerAngles.z);
		if(lastQuaternion != null)
		{
			float angle = Quaternion.Angle(transform.rotation, lastQuaternion);
		}
		lastQuaternion = transform.rotation;
		if (Input.GetKeyDown (KeyCode.F)) 
		{
			eventHandle.ExecuteEvent(eventHandle.TriggerPress);
		}
		if (Input.GetKeyUp (KeyCode.F)) 
		{
			eventHandle.ExecuteEvent (eventHandle.TriggerLoss);
		}
		if(Input.GetKey(KeyCode.F))
		{
			eventHandle.ExecuteEvent (eventHandle.TriggerUpdate);
		}
		if (Input.GetKeyDown (KeyCode.Space)) 
		{
			eventHandle.ExecuteEvent (eventHandle.TeleportPress);
		}
		if(Input.GetKeyUp(KeyCode.C))
		{
			eventHandle.ExecuteEvent (eventHandle.KeyCode_C);
		}
		if(Input.GetKeyUp(KeyCode.K))
		{
			eventHandle.ExecuteEvent (eventHandle.KeyCode_K);
		}
		if(Input.GetKeyUp(KeyCode.R))
		{
			eventHandle.ExecuteEvent (eventHandle.KeyCode_R);
		}
        if (Input.GetKeyUp(KeyCode.T))
        {
            eventHandle.ExecuteEvent(eventHandle.KeyCode_T);
        }
    }

	public void CheckGripHandle()
	{
		if(Input.GetKeyDown(KeyCode.LeftShift))
		{
			eventHandle.ExecuteEvent (eventHandle.GripPress);
		}
		if(Input.GetKeyUp(KeyCode.LeftShift))
		{
			eventHandle.ExecuteEvent (eventHandle.GripLoss);
		}
	}
}
