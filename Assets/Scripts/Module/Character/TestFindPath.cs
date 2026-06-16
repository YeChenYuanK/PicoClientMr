using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestFindPath : BaseEventBehaviour
{
    
    public NavMeshAgent agent;
    public Transform StartPoint;
    public Transform EndPoint;
    public Transform currPoint;

    public bool IsCrouching = false;
    public bool IsMove = false;
    public bool IsAttack = false;
    public bool IsDie = false;
    //public float runCeo
    //{
    //    get
    //    {

    //    }
    //}

    public Animator animator;

	// Use this for initialization
	void Awake ()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        //stateMachine = new StateMachine(this);

    }

    void Start()
    { 
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        //enable agent updates
        //if(!agent.isStopped)
        //{
        //    agent.isStopped = true;
        //}
        Debug.Log(agent.isStopped);
        agent.updateRotation = true;
        currPoint = EndPoint;
        agent.SetDestination(EndPoint.position);
        yield return StartCoroutine(WaitForDestination());

        StartCoroutine(NextWaypoint());
    }

    IEnumerator WaitForDestination()
    {
        yield return new WaitForEndOfFrame();
        while (agent.pathPending)
            yield return null;
        yield return new WaitForEndOfFrame();

        float remain = agent.remainingDistance;
        while (remain == Mathf.Infinity || remain - agent.stoppingDistance > float.Epsilon
        || agent.pathStatus != NavMeshPathStatus.PathComplete)
        {
            remain = agent.remainingDistance;
            yield return null;
        }

        Debug.LogFormat("--- PathComplete to pos:{0}", currPoint);

        Debug.Log(agent.isStopped);
    }

    IEnumerator NextWaypoint()
    {
        if(currPoint == StartPoint)
        {
            currPoint = EndPoint;
        }else
        {
            currPoint = StartPoint;
        }
        Transform next = currPoint;
        agent.SetDestination(next.position);
        yield return StartCoroutine(WaitForDestination());

        StartCoroutine(NextWaypoint());
    }

    // Update is called once per frame
    void Update ()
    {
        animator.SetBool("IsCrouching", IsCrouching);
        //animator.SetBool("IsMove", IsMove);
        animator.SetBool("IsAttack", IsAttack);
        animator.SetBool("IsDie", IsDie);

        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        Debug.Log(info.normalizedTime);
        //if(IsMove)
        //{
            animator.SetFloat("MoveBlend", Mathf.Clamp(agent.velocity.magnitude/agent.speed , 0 , 1));
        //}

    }
}
