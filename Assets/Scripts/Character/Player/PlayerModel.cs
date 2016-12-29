using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UniRx;

public enum PlayerState
{
    Idle = 0,
    Walking,
    PickingUp,
    Attacking,
    Dead
}

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Inventory))]
public class PlayerModel : MonoBehaviourTrans
{
    public static PlayerModel s_instance;

    #region Component References
    Movement m_movementThis;
    Health m_healthThis;
    Inventory m_inventoryThis;
    #endregion

    public ReactiveProperty<PlayerState> m_playerstateCurrent = new ReactiveProperty<PlayerState>(PlayerState.Idle);    //Current State of the Player, starts Idle

    WeaponPickup m_weaponPickUp;    //The weapon you are supposed to pickup. This is only needed for PickingUp State

    EnemyModel m_enemyCurrent;            //The Enemy you are currently trying to attack
    public ReactiveProperty<bool> m_bIsInRange = new ReactiveProperty<bool>(false);        //Is the Player in CombatRange

    float time;
    void Awake()
    {
        s_instance = this;
        m_movementThis = GetComponent<Movement>();
        m_healthThis = GetComponent<Health>();
        m_inventoryThis = GetComponent<Inventory>();
    }

    void Start()
    {
        m_movementThis.m_bHasArrived
        .Where(m_bHasArrived => m_bHasArrived == true)
        .Subscribe(m_bHasArrived => HasArrived());

        m_healthThis.m_fHealth
        .Where(m_fHealth => m_fHealth <= 0)
        .Subscribe(m_fHealth => Die());
    }

    public void ClickAction(RaycastHit _rchitClick)
    {
        Vector3 v3TargetPos;
        GameObject goClicked = _rchitClick.collider.gameObject;
        if (goClicked.layer == 10 /*Enemy*/)
        {
            v3TargetPos = goClicked.transform.position;
            v3TargetPos.y = 0;
            m_enemyCurrent = goClicked.GetComponent<EnemyModel>();
            m_movementThis.MoveToAttack(v3TargetPos, Inventory.CombatRange);
            ChangeToState(PlayerState.Attacking);
            CheckRange();
        }

        if (goClicked.layer == 9 /*Weapon*/)
        {
            v3TargetPos = goClicked.transform.position;
            v3TargetPos.y = 0;
            m_movementThis.MoveTowards(v3TargetPos);
            m_weaponPickUp = goClicked.GetComponent<WeaponPickup>();
            ChangeToState(PlayerState.PickingUp);
        }
        if (goClicked.layer == 8 /*Ground*/)
        {
            v3TargetPos = _rchitClick.point;
            v3TargetPos.y = 0;
            m_movementThis.MoveTowards(v3TargetPos);
            ChangeToState(PlayerState.Walking);
        }

    }

    private void ChangeToState(PlayerState _playerstate)
    {
        m_playerstateCurrent.Value = _playerstate;
    }

    private void Die()
    {
        ChangeToState(PlayerState.Dead);
        Health.m_bRegenAllowed.Value = false;
    }


    public void HasArrived()
    {
        if (CurrentState == PlayerState.PickingUp)
        {
            m_inventoryThis.TakeWeapon(m_weaponPickUp.m_weaponThis);
            Destroy(m_weaponPickUp.gameObject);
            m_weaponPickUp = null;
            m_playerstateCurrent.Value = PlayerState.Idle;
        }

        if (CurrentState == PlayerState.Attacking)
        {
            CheckRange();
        }
    }


    private void CheckRange()
    {
        m_bIsInRange.Value = (transform.position - m_enemyCurrent.Movement.Position).magnitude <= Inventory.CombatRange;
    }

    //Public Properties
    public PlayerState CurrentState
    {
        get
        {
            return m_playerstateCurrent.Value;
        }
    }

    public Health Health
    {
        get
        {
            return m_healthThis;
        }
    }

    public Movement Movement
    {
        get
        {
            return m_movementThis;
        }
    }

    public Inventory Inventory
    {
        get
        {
            return m_inventoryThis;
        }
    }

    public bool IsDead
    {
        get
        {
            return (Health.m_fHealth.Value <= 0) ? true : false;
        }
    }

    //Animation EventReceiver
    public void HitEvent()
    {
        Debug.Log("Hit at:" + (Time.time - time));
        time = Time.time;
        CheckRange();
        if(m_bIsInRange.Value)
        {
            m_enemyCurrent.Health.TakeDamage(Inventory.ActiveWeapon.m_fDamage);
        }
    }

    public void MotionEndEvent()
    {

    }
}
