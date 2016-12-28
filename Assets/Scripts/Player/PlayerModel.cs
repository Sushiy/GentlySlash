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

    public int m_iMaxHealth = 100;
    public ReactiveProperty<int> m_iHealth;

    PlayerState m_playerstateCurrent = PlayerState.Idle;

    WeaponPickup m_weaponPickUp;

    void Awake()
    {
        s_instance = this;
        m_iHealth = new ReactiveProperty<int>(m_iMaxHealth);
    }

	// Use this for initialization
	void Start ()
    {
        m_playerMovement = GetComponent<PlayerMovement>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(CurrentState == PlayerState.PickingUp && m_playerMovement.HasArrived)
        {
            Inventory.s_instance.TakeWeapon(m_weaponPickUp.m_weaponThis);
            m_playerstateCurrent = PlayerState.Idle;
        }

        if(CurrentState == PlayerState.Attacking && m_playerMovement.HasArrived)
        {
            
        }
    }

    public void ClickAction(RaycastHit _rchitClick)
    {
        Vector3 v3TargetPos;
        GameObject goClicked = _rchitClick.collider.gameObject;
        if (goClicked.layer == 10 /*Enemy*/)
        {
            v3TargetPos = goClicked.transform.position;
            v3TargetPos.y = 0;
            m_playerMovement.SetTarget(v3TargetPos, Inventory.s_instance.CombatRange);
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
        m_iHealth.Value = Mathf.Max(m_iHealth.Value - _iDamage, 0);
    }
}
