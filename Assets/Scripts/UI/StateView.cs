using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class StateView : MonoBehaviour
{
    public Model m_model;
    Text m_textThis;
	// Use this for initialization
	void Start ()
    {
        m_textThis = GetComponent<Text>();

        m_model.m_modelstateCurrent
        .Subscribe(m_modelstateCurrent => m_textThis.text = "State of " + m_model.gameObject + ": " + m_modelstateCurrent.ToString());
	}
}
