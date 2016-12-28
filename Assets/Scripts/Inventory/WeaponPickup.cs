using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviourTrans
{
    public Weapon m_weaponThisPickup;
    private void Start()
    {
        if (m_weaponThisPickup != null)
        {
            GameObject.Instantiate(m_weaponThisPickup.m_goWeaponPrefab, transform.position + new Vector3(0,0.2f,0), transform.rotation * Quaternion.Euler(-90, 0, 0), transform);
        }
    }
}
