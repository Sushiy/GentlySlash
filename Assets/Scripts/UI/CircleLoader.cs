using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//This displays a turning LoadingIcon on the pausemenu
public class CircleLoader : MonoBehaviour
{
    private Image m_imageCircle;        //image to turn
    private float m_fTurnSpeed = 90.0f;  //Speed at which to turn in degrees per second
    
    void Awake()
    {
        m_imageCircle = GetComponent<Image>();  //Initialize the image
    }

	// Update is called once per frame
	void LateUpdate()
    {
        m_imageCircle.rectTransform.Rotate(0, 0, Time.unscaledDeltaTime * m_fTurnSpeed);    //turn the image with Time.unscaledtime to do it even when the rest of the game is paused
    }

}
