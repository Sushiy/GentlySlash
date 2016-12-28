using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviourTrans
{
    PlayerMovement m_playerMovementThis;

    private void Start()
    {
        m_playerMovementThis = GetComponent<PlayerMovement>();
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
                if(rchitClick.collider.gameObject.layer == 8)
                    m_playerMovementThis.SetTarget(rchitClick.point);
            }
        }
    }
}
