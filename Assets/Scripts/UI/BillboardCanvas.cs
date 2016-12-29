using UnityEngine;
using System.Collections;

public class BillboardCanvas : MonoBehaviour 
{
    //This rotates the Object to always face the Camera
	void LateUpdate()
	{
		Quaternion q = Quaternion.LookRotation (Camera.main.transform.forward);
        transform.rotation = q;
	}
}
