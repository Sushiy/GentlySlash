using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;


//This simple script toggles the targeticon above the enemies head by checking if the player is currently attacking the enemy
public class TargetViewer : MonoBehaviourTrans
{
    public GameObject m_goTargetIcon;   //targeticon over the AI's head
    public Model m_modelThis;           //Own Modelclass

	// Use this for initialization
	void Start ()
    {
        //Subscribe to the players statechanges
        PlayerModel.s_instance.m_modelstateCurrent
        .Subscribe(m_modelstateCurrent => ToggleTarget());
        m_goTargetIcon.SetActive(false);
	}

    void ToggleTarget()
    {
        //If the players target is you and he is currently in attackstate and you are not dead => show the targeticon
        m_goTargetIcon.SetActive(PlayerModel.s_instance.Opponent == m_modelThis && PlayerModel.s_instance.CurrentState == ModelState.Attacking && m_modelThis.CurrentState != ModelState.Dead);
    }
}
