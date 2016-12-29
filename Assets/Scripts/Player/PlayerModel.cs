using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public enum PlayerState
{
    Idle = 0,
    Walking,
    PickingUp,
    Attacking
}

public class PlayerModel : MonoBehaviourTrans
{
    public static PlayerModel s_instance;
    PlayerMovement m_playerMovement;

    Animator m_animatorThis;

    public float m_fMaxHealth = 100.0f; // Maximum Health value, Health will replenish over time up to this amount
    public float m_fRegenRate = 1.0f;   // Rate at which the player will regenerate Health Per Second
    public ReactiveProperty<float> m_fHealth;   //Current health value

    PlayerState m_playerstateCurrent = PlayerState.Idle;    //Current State of the Player, starts Idle

    WeaponPickup m_weaponPickUp;    //The weapon you are supposed to pickup. This is only needed for PickingUp State

    void Awake()
    {
        s_instance = this;
        m_fHealth = new ReactiveProperty<float>(m_fMaxHealth);
        m_playerMovement = GetComponent<PlayerMovement>();
        m_animatorThis = GetComponent<Animator>();
    }

    void Start()
    {
        m_playerMovement.m_bHasArrived
        .Subscribe(m_bHasArrived => HasArrivedChanged());
    }

	// Update is called once per frame
	void Update ()
    {
        m_fHealth.Value += 1.0f * Time.deltaTime;
    }

    public void HasArrivedChanged()
    {
        if (m_playerMovement.HasArrived)
        {

            if (CurrentState == PlayerState.PickingUp)
            {
                Inventory.s_instance.TakeWeapon(m_weaponPickUp.m_weaponThis);
                Destroy(m_weaponPickUp.gameObject);
                m_weaponPickUp = null;
                m_playerstateCurrent = PlayerState.Idle;
            }

            if (CurrentState == PlayerState.Attacking)
            {
                m_animatorThis.SetBool("bAttacking", true);
            }
        }
    }

    public void ClickAction(RaycastHit _rchitClick)
    {
        m_animatorThis.SetBool("bAttacking", false);
        Vector3 v3TargetPos;
        GameObject goClicked = _rchitClick.collider.gameObject;
        if (goClicked.layer == 10 /*Enemy*/)
        {
            v3TargetPos = goClicked.transform.position;
            v3TargetPos.y = 0;
            m_playerMovement.SetCombatTarget(v3TargetPos, Inventory.s_instance.CombatRange);
            m_playerstateCurrent = PlayerState.Attacking;
        }

        if (goClicked.layer == 9 /*Weapon*/)
        {
            v3TargetPos = goClicked.transform.position;
            v3TargetPos.y = 0;
            m_playerMovement.SetTarget(v3TargetPos, 0.0f);
            m_weaponPickUp = goClicked.GetComponent<WeaponPickup>();
            m_playerstateCurrent = PlayerState.PickingUp;
        }
        if (goClicked.layer == 8 /*Ground*/)
        {
            v3TargetPos = _rchitClick.point;
            v3TargetPos.y = 0;
            m_playerMovement.SetTarget(v3TargetPos, 0.0f);
            m_playerstateCurrent = PlayerState.Walking;
        }

    }

    public PlayerState CurrentState
    {
        get
        {
            return m_playerstateCurrent;
        }
    }

    public void TakeDamage(int _iDamage)
    {
        m_fHealth.Value = Mathf.Max(m_fHealth.Value - _iDamage, 0);
    }
}
