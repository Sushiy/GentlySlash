using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CircleLoader : MonoBehaviour
{
    private Image m_imageCircle;
    private float m_fLerpTime = 0.0f;
    private float m_fLerpSpeed = 1.0f;
    
    void Awake()
    {
        m_imageCircle = GetComponent<Image>();
    }

	// Update is called once per frame
	void LateUpdate()
    {
        m_imageCircle.rectTransform.Rotate(0, 0, Time.unscaledDeltaTime * m_fLerpSpeed * 90.0f);
    }

}
