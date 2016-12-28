using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class ActiveWeaponView : MonoBehaviour {

    GameObject activeWeaponPrefab;
	// Use this for initialization
	void Start ()
    {
        Inventory.s_instance.m_iActiveWeapon
        .Do(x => DisplayActiveWeapon());	
	}

    void DisplayActiveWeapon()
    {
        if (Inventory.s_instance.ActiveWeapon != null)
            activeWeaponPrefab = Inventory.s_instance.ActiveWeapon.m_goWeaponPrefab;
    }
}
