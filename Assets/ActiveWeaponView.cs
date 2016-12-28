using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class ActiveWeaponView : MonoBehaviourTrans
{
    string m_strWeaponName;
    GameObject m_goActiveWeapon;
	// Use this for initialization
	void Start ()
    {
        Inventory.s_instance.m_iActiveWeapon
        .Subscribe(m_iActiveWeapon => DisplayActiveWeapon());	
	}

    void DisplayActiveWeapon()
    {
        if (Inventory.s_instance.ActiveWeapon != null && Inventory.s_instance.ActiveWeapon.m_strName != m_strWeaponName)
        {
            GameObject tmp = m_goActiveWeapon;
            Destroy(tmp);
            m_strWeaponName = Inventory.s_instance.ActiveWeapon.m_strName;
            m_goActiveWeapon = GameObject.Instantiate(Inventory.s_instance.ActiveWeapon.m_goWeaponPrefab, transform.position, transform.rotation, transform);
        }
    }
}
