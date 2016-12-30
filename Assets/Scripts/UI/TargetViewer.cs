using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class TargetViewer : MonoBehaviourTrans
{
    public GameObject m_goTarget;
    public Model m_modelThis;
	// Use this for initialization
	void Start ()
    {
        PlayerModel.s_instance.m_modelstateCurrent
        .Subscribe(m_modelstateCurrent => ToggleTarget());
        m_goTarget.SetActive(false);
	}

    void ToggleTarget()
    {
        m_goTarget.SetActive(PlayerModel.s_instance.Opponent == m_modelThis && PlayerModel.s_instance.CurrentState == ModelState.Attacking && m_modelThis.CurrentState != ModelState.Dead);
    }
}
