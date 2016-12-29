using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class ActiveWeaponView : MonoBehaviourTrans
{
    [SerializeField]
    Inventory m_inventoryThis;

    string m_strWeaponName;
    GameObject m_goActiveWeapon;
	// Use this for initialization
	void Start ()
    {
        if (m_inventoryThis == null)
        {
            Debug.LogError("ActiveWeaponView of " + gameObject.name + " could not find an Inventory");
            return;
        }
        m_inventoryThis.m_iActiveWeapon
        .Subscribe(m_iActiveWeapon => DisplayActiveWeapon());	
	}

    void DisplayActiveWeapon()
    {
        if (m_inventoryThis.ActiveWeapon != null && m_inventoryThis.ActiveWeapon.m_strName != m_strWeaponName)
        {
            GameObject tmp = m_goActiveWeapon;
            Destroy(tmp);
            m_strWeaponName = m_inventoryThis.ActiveWeapon.m_strName;
            m_goActiveWeapon = GameObject.Instantiate(m_inventoryThis.ActiveWeapon.m_goWeaponPrefab, transform.position, transform.rotation, transform);
        }
    }
}
