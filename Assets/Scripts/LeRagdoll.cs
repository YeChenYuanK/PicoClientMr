using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LeRagdoll : MonoBehaviour {
    bool IsRagdoll = false;
    public List<RagdollPart> ragdollBones;
    public List<RagdollPart> dontTrigger;

    public Rigidbody headRigid;
    public Rigidbody bodyRigid;

    public Transform GunTrasn;
    private Animator animator;
    private Rigidbody rigidBody;
    private Collider iCollider;
    private NavMeshAgent Agent;
    private NavMeshObstacle Obstacle;
    public bool IsDie
    {
        get
        {
            return IsRagdoll;
        }
    }
    // Use this for initialization
    void Awake () 
	{
        rigidBody = GetComponent<Rigidbody>();
        iCollider = GetComponent<Collider>();
        animator = GetComponent<Animator>();
        Agent = GetComponent<NavMeshAgent>();
        Obstacle = GetComponent<NavMeshObstacle>();
    }

    public bool CurrState = false;
    

    // Update is called once per frame
    void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            DoRagdoll();
        }
        if(CurrState != IsRagdoll)
        {
            ChangeState(CurrState);
        }

        //animator.SetBool("IsCrouching", IsCrouching);
        //animator.SetBool("IsMove", IsMove);
        //animator.SetBool("IsAttack", IsAttack);
        //animator.SetBool("IsFall", IsFall);
        ////if(IsMove)
        ////{
        //animator.SetFloat("MoveBlend", Mathf.Clamp(moveSpeed / 5, 0, 1));
    }

    public void DoRagdoll()
    {
        Ray currentRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        int mask = LayerMask.GetMask("ColliderLayer", "BodyPart");
        RaycastHit rhit;
        if (Physics.Raycast(currentRay, out rhit, 120.0f,1 << 11))
        {
            RagdollPart ragdollPart = rhit.collider.GetComponent<RagdollPart>();
            if(ragdollPart != null)
            {
                if(ragdollPart.RootRagroll.gameObject != gameObject)
                {
                    return;
                }
                Die(ragdollPart.name , currentRay.direction);
                //ChangeState(true);
                //if(ragdollPart.partType == com.gamestudio.cs.CharacterPart.HEAD)
                //{
                //    ragdollPart.rigidBody.velocity = currentRay.direction * 16.0f;
                //}else
                //{
                //    ragdollPart.rigidBody.velocity = currentRay.direction * 8.0f;
                //}
            }
        }
    }

    void OnColliderEnter()
    {

    }

    public void Die(string partName , Vector3 direction)
    {
        //Debug.Log("Die");
        if (GunTrasn != null)
        {
            GunTrasn.GetComponent<Rigidbody>().isKinematic = false;
            GunTrasn.GetComponent<Collider>().isTrigger = false;
            GunTrasn.parent = null;
            GunTrasn.gameObject.AddComponent<AutoRemove>();
        }

        ChangeState(true);
        //Debug.Log(partName);
        RagdollPart ragdollPart;
        for (int i = 0; i < ragdollBones.Count; i++)
        {
            ragdollPart = ragdollBones[i];
            if (ragdollPart.name == partName)
            {
                if (ragdollPart.partType == com.gamestudio.cs.CharacterPart.HEAD)
                {
                    //ragdollPart.rigidBody.AddForce(direction.normalized * 10);
                    headRigid.velocity = direction.normalized * 8f;// * 2;
                    bodyRigid.velocity = direction.normalized * 6f;
                }
                else
                {
                    //ragdollPart.rigidBody.AddForce(direction.normalized);
                    bodyRigid.velocity = direction * 8f;// * 2;
                }
            }else
            {
                ragdollPart.rigidBody.velocity = direction.normalized * 4;
            }
        }
    }

    public void ChangeState(bool IsRagdoll)
    {
        if (IsRagdoll != this.IsRagdoll)
        {
            CurrState = IsRagdoll;
            if(this.rigidBody != null)
            {
                this.iCollider.enabled = this.animator.enabled = !IsRagdoll;
                this.iCollider.isTrigger = this.rigidBody.isKinematic = IsRagdoll;
            }else
            {
                this.animator.enabled =  !IsRagdoll;
                if(Agent != null)
                {
                    Agent.enabled = !IsRagdoll;
                }
                //Destroy(Obstacle);
                //this.animator.enabled = !IsRagdoll;
                //Destroy(Agent);
            }
            this.IsRagdoll = IsRagdoll;
            for (int i = 0; i < ragdollBones.Count; i++)
            {
                ragdollBones[i].ChangeState(IsRagdoll);
            }
            //Invoke("DontCollider", 3f);
        }
    }

    void DontCollider()
    {
        for (int i = 0; i < ragdollBones.Count; i++)
        {
            ragdollBones[i].ChangeState(false);
        }
    }

}
