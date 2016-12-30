using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class PauseMenuView : MonoBehaviourTrans
{
    public GameObject m_goMenuPanel;
	// Use this for initialization
	void Start ()
    {
        MenuController.s_intance.m_bIsPaused
        .Subscribe(m_bIsPaused => m_goMenuPanel.SetActive(m_bIsPaused));
	}
}
