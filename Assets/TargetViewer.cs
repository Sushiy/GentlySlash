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
        .Where(m_modelstateCurrent => m_modelstateCurrent == ModelState.Attacking)
        .Subscribe(m_modelstateCurrent => ToggleTarget());
	}

    void ToggleTarget()
    {
        m_goTarget.SetActive(PlayerModel.s_instance.Opponent == m_modelThis && m_modelThis.CurrentState != ModelState.Dead);
    }
}
