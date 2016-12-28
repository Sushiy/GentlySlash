using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickCircleFX : MonoBehaviourTrans
{
    public static ClickCircleFX s_instance;
    ParticleSystem m_psCircle;
	// Use this for initialization
	void Start ()
    {
        s_instance = this;
        m_psCircle = transform.GetChild(0).GetComponent<ParticleSystem>();
        if (m_psCircle == null)
            Debug.LogError("ClickCircleFX didn't find the ParticleSystem");
	}
	
    public void PlayParticleSystem(Vector3 _v3Pos)
    {
        if(m_psCircle != null)
        {
            transform.position = _v3Pos;
            m_psCircle.Play();
        }
    }


}
