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
    float m_fAttackRangeSampleRate = 0.1f;      //The Rate at which we check along the path to find a point in combat range
    float m_fVelocityAdjustTimeStep = 1.0f;     //The Timestep in seconds which you would like to look into the future to adjust to the opponents velocity when targeting

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
        for(int i = 0; i < Agent.path.corners.Length-1; i++)
        {
            Debug.DrawLine(Agent.path.corners[i], Agent.path.corners[i + 1], Color.red);
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
        if (fRemainingDistance > float.Epsilon || m_navmeshagentThis.pathPending)
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
        m_v3Target = _v3Target;
        m_navmeshagentThis.SetDestination(m_v3Target);
    }

    //We want to calculate a point within combat range along the path, then move to it; By doing so, the agent adapts to its weapons combatrange;
    //When Calculating the original path, we take into consideration the opponents velocity;
    public void MoveToAttack(Vector3 _v3Target, float _fCombatRange, Vector3 _v3TargetVelocity)
    {
        //Calculate the Actual combat Range by decreasing it a bit (Otherwise the points found would end up at 3.50047 of 3.5 or similar, resulting in stopping out of range)
        float fActualRange = _fCombatRange - 0.1f;
        Vector3 v3ActualTarget = _v3Target + _v3TargetVelocity * Time.deltaTime * m_fVelocityAdjustTimeStep;    //The targetpoint adjusted by the opponents speed
        
        //If the Distance to your target is lower than your actual range, Stop and return
        if (Vector3.Distance(v3ActualTarget, Position) < fActualRange)
        {
            Stop();
            return;
        }

        NavMeshPath navmeshpath = new NavMeshPath();    //Initialize the NavMeshPath
        Agent.CalculatePath(v3ActualTarget, navmeshpath);    //Calculate a path to the target
        Agent.path = navmeshpath;                       //Set the agent on the new path
        
        
        int iCornerCount = Agent.path.corners.Length;   //The number of corners in the temporary path
        int iLastIndexInRange = iCornerCount - 1;       //the last index of the path(the endpoint) is definitely in range
        Vector3 v3BestSample = Agent.path.corners[iCornerCount - 1];    //The position that is closest to the specified range

        //Go through the path from end and sample each corner to check if it is in range of the target
        for (int i = iCornerCount - 2; i > 0; i--)
        {
            //if it is in range, set it to be the last index that is within the range
            if(Vector3.Distance(Agent.path.corners[i], v3ActualTarget) < fActualRange)
            {
                iLastIndexInRange = i;
            }
            //if it is outside of our range stop the routine
            else
            {
                break;
            }
        }

        //We have now calculated the corner furthest away (on the path) from the endpoint that is still in range
        //Now we will go through that corners segment and try to find a point on that segment that is even closer to the range given

        v3BestSample = Agent.path.corners[iLastIndexInRange]; //The lastindexinrange is now the point closest to the player
        //If the Index we found was not the first point on our path, the the desired point is on the segment between lastIndexInRange and lastIndexInRange - 1
        if (iLastIndexInRange != 1)
        {
            //Now let's check this segment step by step; Starting from the "lastpointinrange"; we want to find the point on the segment that is in range and closest to the combatrange
            Vector3 v3Segment = Agent.path.corners[iLastIndexInRange - 1] - Agent.path.corners[iLastIndexInRange]; //Get the relevant segment
            for (float f = 0; f < 1.0f; f -= m_fAttackRangeSampleRate)
            {
                Vector3 v3Sample = Agent.path.corners[iLastIndexInRange] + v3Segment * f;   //sample the new point
                //if it is in range, its our best sample yet
                if(Vector3.Distance(v3ActualTarget, v3Sample) <= fActualRange)
                {
                    v3BestSample = v3Sample;
                }
                //if it isn't the best sample is actually the best sample and we can stop this
                else
                {
                    break;
                }
            }
        }
        //Move towards the best point in range we have found
        MoveTowards(v3BestSample);
    }

    //Rotates the agent towards the given Target
    public void LookAt(Vector3 _v3Target)
    {
        //If you are not trying to look at yourself
        if(_v3Target != transform.position)
        {
            Vector3 v3Direction = (_v3Target - transform.position).normalized;  //Calculate the lookdirection
            Quaternion qLookRotation = Quaternion.LookRotation(v3Direction);    //Get the Lookrotation
            transform.rotation = qLookRotation;                                 //Snap the player to the lookrotation
        }
    }

    //Stop The Model by Moving "to itself" and then check for range
    public void Stop()
    {
        Vector3 v3OldTarget = m_v3Target;
        MoveTowards(Position);      //Move towards yourself which means: STOP!
        LookAt(v3OldTarget);  //Rotate to the target
    }

    //*************PUBLIC PROPERTIES**************************

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
