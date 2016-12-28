using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "weaponName", menuName = "Weapon", order = 1)]
public class Weapon : ScriptableObject
{
    public string m_strName;        //Name of the Weapon;
    public float m_fAttackRange;    //Attackrange in Units
    public float m_fDamage;         //Damage done per hit
    public float m_fAttackSpeed;    //Attacks per Second
    public GameObject m_goWeaponPrefab;  //The Model used for this Weapon
    public Sprite m_spriteUI;       //Sprite to be used in UI
}
