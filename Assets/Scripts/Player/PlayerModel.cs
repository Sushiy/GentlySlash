using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Idle = 0,
    Walking,
    PickingUp,
    Attacking
}

public class PlayerModel : MonoBehaviourTrans
{
    PlayerMovement m_playerMovement;

    public int m_iMaxHealth = 100;
    int m_iHealth;

    PlayerState m_playerstateCurrent = PlayerState.Idle;

    WeaponPickup m_weaponPickUp;

	// Use this for initialization
	void Start ()
    {
        m_playerMovement = GetComponent<PlayerMovement>();
        m_iHealth = m_iMaxHealth;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(CurrentState == PlayerState.PickingUp && m_playerMovement.HasArrived)
        {
            Inventory.s_instance.TakeWeapon(m_weaponPickUp.m_weaponThis);
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
        else if(goClicked.layer == 9 /*Weapon*/)
        {
            v3TargetPos = goClicked.transform.position;
            v3TargetPos.y = 0;
            m_playerMovement.SetTarget(v3TargetPos, 0.0f);
            m_weaponPickUp = goClicked.GetComponent<WeaponPickup>();
            m_playerstateCurrent = PlayerState.PickingUp;
        }
        

    }

    public PlayerState CurrentState
    {
        get
        {
            return m_playerstateCurrent;
        }
    }
}
