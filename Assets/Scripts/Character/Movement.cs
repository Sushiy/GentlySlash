using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using UniRx;

[RequireComponent(typeof(NavMeshAgent))]
public class Movement : MonoBehaviourTrans 
{
    #region Component References
    NavMeshAgent m_navmeshagentThis;
    #endregion
	public ReactiveProperty<bool> m_bHasArrived = new ReactiveProperty<bool>(false); //Has the Agent arrived at his destination
    public float m_fMovementSpeed = 3.5f; //3.5f is the standard speed for NavMeshAgents
    public float m_fTurningSpeed = 120.0f; //120.0f is the standard turningSpeed

    Vector3 m_v3Target; //The last target we wanted to reach
    float fAttackRangeSampleRate = 0.1f;

    // Use this for initialization
    void Awake() 
	{
		m_navmeshagentThis = GetComponent<NavMeshAgent>();
        if (m_navmeshagentThis != null)
        {
            m_navmeshagentThis.speed = m_fMovementSpeed;
            m_navmeshagentThis.angularSpeed = m_fTurningSpeed;
        }
	}
    
    void LateUpdate()
    {
        Debug.DrawLine(transform.position, m_v3Target);
        for(int i = 0; i < m_navmeshagentThis.path.corners.Length-1; i++)
        {
            Debug.DrawLine(m_navmeshagentThis.path.corners[i], m_navmeshagentThis.path.corners[i + 1], Color.red);
        }

        //Check if the NavMeshAgent is Null or not on the Mesh
        if (m_navmeshagentThis == null)
        {
            Debug.LogError("Movement of " + gameObject.name + " could not find a NavMeshAgent");
            return;
        }

        if (!m_navmeshagentThis.isOnNavMesh)
        {
            Debug.LogError("The NavMeshAgent of " + gameObject.name +" is not on the NavMesh");
            return;
        }

        //Get remaining Pathdistance
        float fRemainingDistance = m_navmeshagentThis.remainingDistance;

        //If the agent is still moving, has still some Distance to go or is still calculating his path, he has not arrived yet set. Also set the lookDirection to the velocity direction
        if (m_navmeshagentThis.velocity.magnitude > 0 || fRemainingDistance > float.Epsilon || m_navmeshagentThis.pathPending)
        {
            m_bHasArrived.Value = false;
        }
        //The Agent has arrived if the remaining distance is almost 0
        else if (fRemainingDistance <= float.Epsilon)
        {
            m_bHasArrived.Value = true;
        }
    }

    //Move to the targetposition
    public void MoveTowards(Vector3 _v3Target)
    {
        //m_bHasArrived.Value = false;
        m_v3Target = _v3Target;
        m_navmeshagentThis.SetDestination(m_v3Target);
    }
    
    //Calculate a point within combat range along the path, then move to it
    public void MoveToAttack(Vector3 _v3Target, float _fCombatRange)
    {
        float fActualRange = _fCombatRange - 0.1f;
        if (Vector3.Distance(_v3Target, Position) < fActualRange)
        {
            MoveTowards(Position);
            RotateTowards(_v3Target);
            return;
        }
        NavMeshPath navmeshpath = new NavMeshPath();    //Initialize the NavMeshPath
        Agent.CalculatePath(_v3Target, navmeshpath);    //Calculate a path to the target
        Agent.path = navmeshpath;
        NavMeshHit navmeshhit = new NavMeshHit();       //Initialize NavMeshHit

        Agent.SamplePathPosition(Agent.areaMask, Agent.remainingDistance, out navmeshhit); //sample the last point of the path
        //Go through the path from end and sample each point to check if it is in range of the enemy
        for (float fRange = Agent.remainingDistance - fAttackRangeSampleRate; (navmeshhit.position - _v3Target).magnitude < fActualRange; fRange -= fAttackRangeSampleRate)
        {
            //Sample each point until the distance to the target has been maxed out
            Agent.SamplePathPosition(Agent.areaMask, fRange, out navmeshhit);
        }

        MoveTowards(navmeshhit.position);
    }

    //Rotates the agent towards the given Target
    public void RotateTowards(Vector3 _v3Target)
    {
        Vector3 v3Direction = (_v3Target - transform.position).normalized;
        Quaternion qLookRotation = Quaternion.LookRotation(v3Direction);
        transform.rotation = qLookRotation;
    }

    public NavMeshAgent Agent
    {
        get
        {
            return m_navmeshagentThis;
        }
    }

    public bool HasArrived
	{
        get
        {
            return m_bHasArrived.Value;
        }
	}

	public Vector3 Target
	{
        get
        {
            return m_v3Target;
        }
	}

    public Vector3 Position
    {
        get
        {
            return transform.position;
        }
    }

    public Vector3 Velocity
    {
        get
        {
            return m_navmeshagentThis.velocity;
        }
    }

    //Length of velocityvector, relative to the maximum speed
    public float NormalizedVelocityMagnitude
    {
        get
        {
            return Velocity.magnitude / m_fMovementSpeed;
        }
    }
}
