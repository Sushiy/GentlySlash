using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class Inventory : MonoBehaviourTrans
{
    public float m_fStandardStoppingDistance = 2.0f;
    
    private Weapon[] m_weaponSlots;
    public int m_iNumOfSlots = 2;
    public ReactiveProperty<int> m_iActiveWeapon = new ReactiveProperty<int>(0);

	// Use this for initialization
	void Awake ()
    {
        m_weaponSlots = new Weapon[m_iNumOfSlots];
    }

    //This Method is called, when a new weapon is supposed to be picked up
    public void TakeWeapon(Weapon _weapon)
    {
        int iNonActiveWeapon = Mathf.Clamp(m_iNumOfSlots - 1 - m_iActiveWeapon.Value, 0, m_iNumOfSlots-1);
        //If the active weapon slot is empty or both slots are full put the new weapon in your active slot. In the latter case the old weapon is replaced
        if (m_weaponSlots[m_iActiveWeapon.Value] == null || m_weaponSlots[iNonActiveWeapon] != null)
        {
            m_weaponSlots[m_iActiveWeapon.Value] = _weapon;
            m_iActiveWeapon.SetValueAndForceNotify(m_iActiveWeapon.Value);
        }
        //else put it in your nonactive slot (which should be empty)
        else
        {
            m_weaponSlots[iNonActiveWeapon] = _weapon;

            m_iActiveWeapon.SetValueAndForceNotify(iNonActiveWeapon);
        }
    }

    //Switch to the weapon with the specified index
    public void SwitchToWeapon(int _index)
    {
        //first check if the index is within your allowed number of slots
        if(_index >= 0 && _index < m_iNumOfSlots)
            m_iActiveWeapon.Value = _index;
    }

    public Weapon ActiveWeapon
    {
        get
        {
            return m_weaponSlots[m_iActiveWeapon.Value];
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

    public Weapon[] Weapons
    {
        get
        {
            return m_weaponSlots;
        }
    }
}
