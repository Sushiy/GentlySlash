using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviourTrans
{
    PlayerModel m_playerModelThis;

    private void Start()
    {
        m_playerModelThis = GetComponent<PlayerModel>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit rchitClick;
        bool bLeftMouseDown = Input.GetMouseButtonDown(0);
        Vector3 v3MousePos = Input.mousePosition;
        if (bLeftMouseDown)
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(v3MousePos), out rchitClick))
            {
                ClickCircleFX.s_instance.PlayParticleSystem(rchitClick.point); 
                m_playerModelThis.ClickAction(rchitClick);
                    
            }
        }

        if(Input.GetKeyDown(KeyCode.I))
        {
            PlayerModel.s_instance.TakeDamage(10);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Inventory.s_instance.SwitchToWeapon(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Inventory.s_instance.SwitchToWeapon(1);
        }
    }
}
