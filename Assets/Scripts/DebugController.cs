using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugController : MonoBehaviourTrans
{
	// Update is called once per frame
	void Update ()
    {
        //If "I" is Down, deal Damage to player
        if (Input.GetKeyDown(KeyCode.I))
        {
            PlayerModel.s_instance.Health.TakeDamage(10);
        }
    }
}
