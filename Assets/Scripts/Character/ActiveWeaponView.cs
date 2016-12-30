using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

//This simple script displays the active weapon in the Hand of the Character
public class ActiveWeaponView : MonoBehaviourTrans
{
    [SerializeField]
    Inventory m_inventoryThis;  //The inventory to reference

    string m_strWeaponName;     //The name of the weapon (this is saved in order to not switch into the same weapon over and over)
    GameObject m_goActiveWeapon;    //This will store the gameobject of the spawned weapon

	// Use this for initialization
	void Start ()
    {
        if (m_inventoryThis == null)
        {
            Debug.LogError("ActiveWeaponView of " + gameObject.name + " could not find an Inventory");
            return;
        }

        //Subscribe to the active weapon of the inventory
        m_inventoryThis.m_iActiveWeapon
        .Subscribe(m_iActiveWeapon => DisplayActiveWeapon());	
	}

    void DisplayActiveWeapon()
    {
        //if there is an active weapon in the inventory and it is not the same as the on the character is holding
        if (m_inventoryThis.ActiveWeapon != null && m_inventoryThis.ActiveWeapon.m_strName != m_strWeaponName)
        {
            GameObject tmp = m_goActiveWeapon;  //save out the old weapon
            Destroy(tmp);                       //destroy the old weapon
            m_strWeaponName = m_inventoryThis.ActiveWeapon.m_strName;   //set the new weapon name
            m_goActiveWeapon = GameObject.Instantiate(m_inventoryThis.ActiveWeapon.m_goWeaponPrefab, transform.position, transform.rotation, transform);    //spawn the new weapon
        }
    }
}
