using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class PauseMenuView : MonoBehaviourTrans
{
    public GameObject m_goMenuPanel;    //The MenuPanel

	void Start ()
    {
        //Subscribe to The isPaused of the menumodel to activate and deactivate the panel
        MenuController.s_instance.m_menumodel.m_bIsPaused
        .Subscribe(m_bIsPaused => m_goMenuPanel.SetActive(m_bIsPaused));
	}
}
