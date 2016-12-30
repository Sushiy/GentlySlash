﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UniRx;
using BehaviourTree;

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Inventory))]
public class AIModel : Model
{
    public Weapon m_weaponHeld;
    public float m_fPlayerDetectionRange = 10.0f;   //Range in Units in which the player is registered
    public float m_fDamageThreshhold = 0.75f;   //percentage of Damage at which AI flee
    public float m_fFleeTime = 8.0f;        //Time for which the AI will flee from the player (seconds)

    private float m_fFleeTimeStart = -Mathf.Infinity;

    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
        m_modelstateCurrent
        .Subscribe(m_modelstateCurrent => StateHasChanged());

        if (m_weaponHeld != null)
            m_inventoryThis.TakeWeapon(m_weaponHeld);
        m_modelOpponent = PlayerModel.s_instance;
    }

    //This is triggered whenever the state of the model changes
    protected void StateHasChanged()
    {
        switch(CurrentState)
        {
            case ModelState.Attacking:
                Movement.MoveToAttack(PlayerModel.s_instance.Movement.Position, Inventory.CombatRange); //If you just changed into attacking state, move to target to attack
                break;
            case ModelState.Fleeing:
                m_fFleeTimeStart = Time.time;   //if you just changed into fleeing state, start the fleetimer
                FleeFromPlayer();
                break;
            case ModelState.Idle:
                Stop();
                break;
        }
    }

    //This is triggered when the agent arrives at his target
    protected override void HasArrived()
    {
        //If you are Fleeing, check if the player is still in your detection range, if so, keep fleeing, if not wait
        if (CurrentState == ModelState.Fleeing)
        {
            FleeFromPlayer();
        }

        //If you are attacking, try to rotate towards your target
        if (CurrentState == ModelState.Attacking)
        {
            CheckRange();
            Movement.RotateTowards(m_modelOpponent.Movement.Position);
        }
    }

    public void Tick()
    {
        if (CurrentState == ModelState.Attacking)
        {
            if (!m_bIsInRange.Value && Movement.Target != PlayerModel.s_instance.Movement.Position)
            {
                Movement.MoveToAttack(PlayerModel.s_instance.Movement.Position, Inventory.CombatRange);
            }
            else
            {
                CheckRange();
            }
        }
        if (CurrentState == ModelState.Fleeing)
        {
            if (IsPlayerInDetectionRange)
            {
                FleeFromPlayer();
            }
        }
    }
    protected override void Die()
    {
        base.Die();
        CancelInvoke();
    }

    public bool IsPlayerInDetectionRange
    {
        get
        {
            return Vector3.Distance(PlayerModel.s_instance.Movement.Position, transform.position) <= m_fPlayerDetectionRange;
        }
    }

    protected void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, m_fPlayerDetectionRange);
    }

    private void FleeFromPlayer()
    {   //http://answers.unity3d.com/questions/868003/navmesh-flee-ai-flee-from-player.html

        float fFleeDistance = m_fPlayerDetectionRange * Random.Range(0.75f, 1.5f);
        float fFleeArc = 90.0f;

        //Temporarily turn AI facing away from player
        Quaternion qTMP = transform.rotation;
        transform.rotation = Quaternion.LookRotation(Movement.Position - m_modelOpponent.Movement.Position);

        //Find a random direction within the given arc around the rotation
        Vector3 v3targetDir = Quaternion.AngleAxis(Random.Range(-fFleeArc / 2, fFleeArc), Vector3.up) * transform.forward;
        //Turn the AI back to allow it to turn around normally
        transform.rotation = qTMP;

        //Calculate a position directly away from the player, that is sufficiently far away (0.75 - 1.5 times the detectionrange)
        Vector3 v3FleeTarget = transform.position + (v3targetDir * fFleeDistance);

        //Now we need to find a point on the navmesh that is as far away in that direction as possible
        NavMeshHit navmeshhit;    // stores the output in a variable called hit

        //This shoots a ray and checks if it hits something before reaching the FleeTarget
        NavMesh.Raycast(transform.position, v3FleeTarget, out navmeshhit, Movement.Agent.areaMask);
        Movement.MoveTowards(navmeshhit.position);
    }

    public float FleeTimeStart
    {
        get
        {
            return m_fFleeTimeStart;
        }

        set
        {
            m_fFleeTimeStart = value;
        }
    }
}
