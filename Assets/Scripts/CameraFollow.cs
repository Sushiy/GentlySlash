using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviourTrans
{
    [SerializeField]
    private PlayerMovement m_playerMovement;

	// Use this for initialization
	void Start ()
    {
        if (m_playerMovement == null)
        {
            Debug.LogError("PlayerMovement was not Found by CameraFollow");
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.position = m_playerMovement.position;
	}
}
