using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviourTrans
{
    public static Inventory s_instance;

    public float m_fStandardStoppingDistance = 2.0f;

    private Weapon[] m_weaponSlots;
    private int m_iNumOfSlots = 2;
    private int m_iActiveWeapon;

	// Use this for initialization
	void Start ()
    {
        s_instance = this;
        m_weaponSlots = new Weapon[2];
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void TakeWeapon(Weapon _weapon)
    {
        if(m_weaponSlots[m_iActiveWeapon] == null || (m_weaponSlots[m_iActiveWeapon] != null && m_weaponSlots[m_iNumOfSlots - m_iActiveWeapon] != null))
        {
            m_weaponSlots[m_iActiveWeapon] = _weapon;
        }
        else
        {
            m_weaponSlots[m_iNumOfSlots - m_iActiveWeapon] = _weapon;
        }
    }

    public void SwitchToWeapon(int _index)
    {
        if(_index >= 0 && _index < m_iNumOfSlots)
            m_iActiveWeapon = _index;
    }

    public Weapon ActiveWeapon
    {
        get
        {
            return m_weaponSlots[m_iActiveWeapon];
        }
    }

    public float CombatRange
    {
        get
        {
            if (ActiveWeapon != null)
                return ActiveWeapon.m_fAttackRange;
            else
                return m_fStandardStoppingDistance;
        }
    }
}
