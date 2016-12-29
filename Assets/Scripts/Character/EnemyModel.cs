using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Inventory))]
public class EnemyModel : MonoBehaviourTrans
{
    #region Component References
    Movement m_movementThis;
    Health m_healthThis;
    Inventory m_inventoryThis;
    #endregion

    public Weapon m_weaponHeld;

    void Awake()
    {
        m_movementThis = GetComponent<Movement>();
        m_healthThis = GetComponent<Health>();
        m_inventoryThis = GetComponent<Inventory>();
    }

    void Start()
    {
        if (m_weaponHeld != null)
            m_inventoryThis.TakeWeapon(m_weaponHeld);
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
