using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This Class Controls the PlayerModel through Userinput
public class PlayerController : MonoBehaviourTrans
{
    // Update is called once per frame
    void Update()
    {
        bool bLeftMouseDown = Input.GetMouseButtonDown(0);  //Is the left MouseButton down
        Vector3 v3MousePos = Input.mousePosition;           //current MousePosition

        RaycastHit rchitClick;  //HitInfo of the Raycast 

        //If the left  Mousebutton is Down
        if (bLeftMouseDown)
        {
            //Do a Raycast through the mouseposition, if you hit something
            if (Physics.Raycast(Camera.main.ScreenPointToRay(v3MousePos), out rchitClick))
            {
                ClickCircleFX.s_instance.PlayParticleSystem(rchitClick.point);  //Play the Click Animation at the hit position
                PlayerModel.s_instance.ClickAction(rchitClick);                 //Notify the PlayerModel, that there has been a Click                   
            }
        }

        //Switch to Weapon 1
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlayerModel.s_instance.Inventory.SwitchToWeapon(0);
        }
        //Switch to Weapon 2
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PlayerModel.s_instance.Inventory.SwitchToWeapon(1);
        }
    }
}
