using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimeView : MonoBehaviour
{
    Text m_textThis;

    private void Awake()
    {
        m_textThis = GetComponent<Text>();
    }

    private void Update()
    {
        m_textThis.text = Time.unscaledTime.ToString("F1");
    }
}
