using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviourTrans
{
    public Weapon m_weaponThis;

    private void Start()
    {
        if (m_weaponThis != null)
        {
            GameObject.Instantiate(m_weaponThis.m_goWeaponPrefab, transform.position + new Vector3(0,0.2f,0), transform.rotation * Quaternion.Euler(-90, 0, 0), transform);
        }
    }
}
