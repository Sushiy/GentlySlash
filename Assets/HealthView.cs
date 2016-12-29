using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

[RequireComponent(typeof(Slider))]
public class HealthView : MonoBehaviourTrans
{
    Slider m_sliderThis;

    void Start()
    {
        m_sliderThis = GetComponent<Slider>();
        m_sliderThis.maxValue = PlayerModel.s_instance.m_fMaxHealth;

        PlayerModel.s_instance.m_fHealth
        .Subscribe(m_fHealth => m_sliderThis.value = m_fHealth);
    }
}
