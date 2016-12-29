using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

[RequireComponent(typeof(Slider))]
public class HealthView : MonoBehaviourTrans
{
    public Health m_healthThis; //HealthComponent we take our values from 
    Slider m_sliderThis;    //SliderUI Element we post our Value to

    void Start()
    {
        m_sliderThis = GetComponent<Slider>();
        m_sliderThis.maxValue = m_healthThis.m_fMaxHealth;

        m_healthThis.m_fHealth
        .Subscribe(m_fHealth => m_sliderThis.value = m_fHealth);
        m_healthThis.m_bRegenAllowed
        .Subscribe(m_bRegenAllowed => DarkenSlider(m_bRegenAllowed));
    }

    void DarkenSlider(bool b)
    {
        if (!b)
            m_sliderThis.targetGraphic.color = new Color(150.0f/255.0f, 0, 0);
        else
            m_sliderThis.targetGraphic.color = new Color(200.0f / 255.0f, 0, 0); ;
    }
}
