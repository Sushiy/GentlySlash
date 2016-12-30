using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Simple spawner script that spawns the weapon it gets on itself
public class WeaponPickup : MonoBehaviourTrans
{
    public Weapon m_weaponThis;

    private void Start()
    {
        if (m_weaponThis != null)
        {
            //If there is a weapon given, spawn it at your position with offset rotation and y-coordinate
            GameObject.Instantiate(m_weaponThis.m_goWeaponPrefab, transform.position + new Vector3(0,0.1f,0), transform.rotation * Quaternion.Euler(-90, 0, 0), transform);
        }
    }
}
