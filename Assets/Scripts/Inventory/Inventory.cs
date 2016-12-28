using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class Inventory : MonoBehaviourTrans
{
    public static Inventory s_instance;

    public float m_fStandardStoppingDistance = 2.0f;

    [HideInInspector]
    public Weapon[] m_weaponSlots;

    private const int m_iNumOfSlots = 2;
    public ReactiveProperty<int> m_iActiveWeapon;

	// Use this for initialization
	void Awake ()
    {
        s_instance = this;
        m_weaponSlots = new Weapon[2];

        m_iActiveWeapon = new ReactiveProperty<int>(0);
    }

    //This Method is called, when a new weapon is supposed to be picked up
    public void TakeWeapon(Weapon _weapon)
    {
        //If the active weapon slot is empty or both slots are full put the new weapon in your active slot. In the latter case the old weapon is replaced
        if(m_weaponSlots[m_iActiveWeapon.Value] == null || m_weaponSlots[m_iNumOfSlots - 1 - m_iActiveWeapon.Value] != null)
        {
            m_weaponSlots[m_iActiveWeapon.Value] = _weapon;
        }
        //else put it in your nonactive slot (which should be empty)
        else
        {
            m_weaponSlots[m_iNumOfSlots - 1 - m_iActiveWeapon.Value] = _weapon;
 
        }

        m_iActiveWeapon.SetValueAndForceNotify(m_iActiveWeapon.Value);      //This is actually just done to trigger the ReactiveProperty
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
}
