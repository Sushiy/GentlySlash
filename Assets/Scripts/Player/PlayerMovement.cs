﻿using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using UniRx;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerMovement : MonoBehaviourTrans 
{
	NavMeshAgent m_navmeshagentThis;
	Animator m_animatorThis;

	Vector3 m_v3Target;

	public ReactiveProperty<bool> m_bHasArrived = new ReactiveProperty<bool>(false);

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
        Debug.DrawRay(transform.position, transform.forward * m_navmeshagentThis.stoppingDistance, Color.red);
        
        if(m_animatorThis != null)
        {
            m_animatorThis.SetBool("bMoving", m_navmeshagentThis.velocity.magnitude > 0);
            m_animatorThis.SetFloat("fVelocity", m_navmeshagentThis.velocity.magnitude / m_fMovementSpeed);
        }
	}

    void LateUpdate()
    {
        Debug.DrawLine(transform.position, m_v3Target);

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
            m_bHasArrived.Value = false;
        }
        //The Agent has arrived if the remaining distance is almost 0 and its path is complete
        else if (fDist <= float.Epsilon && m_navmeshagentThis.pathStatus == NavMeshPathStatus.PathComplete)
        {
            m_bHasArrived.Value = true;
        }
    }

    public bool HasArrived
	{
        get
        {
            return m_bHasArrived.Value;
        }
	}

	public Vector3 target
	{
        get
        {
            return m_v3Target;
        }
	}

	public void SetTarget(Vector3 _v3Target, float _fStoppingDistance)
    {
        m_bHasArrived.Value = false;
        m_navmeshagentThis.stoppingDistance = _fStoppingDistance;
        m_v3Target = _v3Target;
		m_navmeshagentThis.SetDestination (m_v3Target);      
    }

    public void SetCombatTarget(Vector3 _v3Target, float _fStoppingDistance)
    {
        Vector3 v3CombatTarget = _v3Target + (transform.position - _v3Target).normalized * _fStoppingDistance;
        SetTarget(v3CombatTarget, 0.0f);
    }

    public Vector3 position
    {
        get
        {
            return transform.position;
        }
    }
}
