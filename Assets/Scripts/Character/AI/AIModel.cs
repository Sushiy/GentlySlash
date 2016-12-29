using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using BehaviourTree;

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Inventory))]
public class AIModel : Model
{
    public Weapon m_weaponHeld;
    public float m_fPlayerDetectionRange = 10.0f;   //Range in Units in which the player is registered
    public float m_fDamageBeforeFlee = 0.25f;   //percentage of Damage the AI can take before Fleeing
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
                break;
        }
    }

    //This is triggered when the agent arrives at his target
    protected override void HasArrived()
    {
        //If you are Fleeing, check if the player is still in your detection range, if so, keep fleeing, if not wait
        if (CurrentState == ModelState.Fleeing)
        {

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
        }
    }
    protected override void Die()
    {
        base.Die();
        CancelInvoke();
    }

    public bool IsPlayerInDetectionRange()
    {
        if (Vector3.Distance(PlayerModel.s_instance.Movement.Position, transform.position) <= m_fPlayerDetectionRange)
        {
            return true;
        }
        return false;
    }

    protected void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, m_fPlayerDetectionRange);
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
