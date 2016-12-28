using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviourTrans
{
    public static Inventory s_instance;

    private Weapon[] m_weaponSlots;

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

    public void PickupWeapon(Weapon _weapon)
    {
        if(ActiveWeapon == null)
        {
            m_weaponSlots[m_iActiveWeapon] = _weapon;
        }

        else
        {

        }
    }

    public void SwitchToWeapon(int _index)
    {
        if(_index >= 0 && _index < m_weaponSlots.Length)
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
                return 0;
        }
    }
}
