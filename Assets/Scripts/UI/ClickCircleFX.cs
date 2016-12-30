using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script sets the click particleeffect to the clickposition and plays it
public class ClickCircleFX : MonoBehaviourTrans
{
    public static ClickCircleFX s_instance; //singleton instance

    ParticleSystem m_psClick;  //Click particlesystem

	// Use this for initialization
	void Start ()
    {
        //this class is a singleton, so if there is another one already, destroy this one
        if (s_instance != null)
        {
            Debug.Log("Singleton class: " + this.GetType().ToString() + " had another instance on: " + gameObject.ToString() + " which was destroyed.");
            Destroy(this);
            return;
        }
        s_instance = this;   //init singleton instance

        //Get the Particlesystem from your child
        m_psClick = transform.GetChild(0).GetComponent<ParticleSystem>();
        if (m_psClick == null)
            Debug.LogError("ClickCircleFX didn't find the ParticleSystem");
	}
	
    //Play the particlesystem at the give position
    public void PlayParticleSystem(Vector3 _v3Pos)
    {
        if(m_psClick != null)
        {
            _v3Pos.y = 0;
            transform.position = _v3Pos;
            m_psClick.Play();
        }
    }


}
