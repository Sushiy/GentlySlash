using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerMovement : MonoBehaviourTrans 
{
	NavMeshAgent m_navmeshagentThis;
	Animator m_animatorThis;

	Vector3 m_v3Target;

	bool m_bHasArrived = false;

    public float m_fMovementSpeed = 3.5f; //3.5f is the standard speed for NavMeshAgents
    public float m_fTurningSpeed = 120.0f; //120.0f is the standard turningSpeed

	// Use this for initialization
	void Awake() 
	{
		m_navmeshagentThis = GetComponent<NavMeshAgent>();
        if (m_navmeshagentThis != null)
        {
            m_navmeshagentThis.speed = m_fMovementSpeed;
            m_navmeshagentThis.angularSpeed = m_fTurningSpeed;
        }
        m_animatorThis = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update() 
	{
        Debug.DrawRay(transform.position, transform.forward, Color.red);
        
        if(m_animatorThis != null)
        {
            m_animatorThis.SetBool("bMoving", m_navmeshagentThis.velocity.magnitude > 0);
            m_animatorThis.SetFloat("fVelocity", m_navmeshagentThis.velocity.magnitude / m_fMovementSpeed);
        }
	}

    void LateUpdate()
    {
        //Check if the NavMeshAgent is Null or not on the Mesh
        if (m_navmeshagentThis == null)
        {
            Debug.LogError("PlayerMovement could not find a NavMeshAgent");
            return;
        }

        if (!m_navmeshagentThis.isOnNavMesh)
        {
            Debug.LogError("The NavMeshAgent is not on the NavMesh");
            return;
        }

        //Get remaining Pathdistance
        float fDist = m_navmeshagentThis.remainingDistance;

        //If the agent is still moving, has still some Distance to go or is still calculating his path, he has not arrived yet set. Also set the lookDirection to the velocity direction
        if (m_navmeshagentThis.velocity.magnitude > 0 || fDist > float.Epsilon || m_navmeshagentThis.pathPending)
        {
            m_bHasArrived = false;
        }
        //The Agent has arrived if the remaining distance is almost 0 and its path is complete
        else if (fDist <= float.Epsilon && m_navmeshagentThis.pathStatus == NavMeshPathStatus.PathComplete)
        {
            m_bHasArrived = true;
        }
    }

    public bool HasArrived
	{
        get
        {
            return m_bHasArrived;
        }
	}

	public Vector3 target
	{
        get
        {
            return m_v3Target;
        }
	}

	public void SetTarget(RaycastHit _rchitClick)
	{
        m_bHasArrived = false;
        m_v3Target = _rchitClick.point;
		m_navmeshagentThis.SetDestination (m_v3Target);
        GameObject goClicked = _rchitClick.collider.gameObject;
        if (goClicked.layer == 10 /*Enemy*/)
        {
            m_navmeshagentThis.stoppingDistance = Inventory.s_instance.CombatRange;
        }
    }

    public Vector3 position
    {
        get
        {
            return transform.position;
        }
    }
}
