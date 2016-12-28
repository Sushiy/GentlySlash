﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

[RequireComponent(typeof(Slider))]
public class HealthView : MonoBehaviour
{
    Slider m_sliderThis;

    void Start()
    {
        m_sliderThis = GetComponent<Slider>();
        m_sliderThis.maxValue = PlayerModel.s_instance.m_iMaxHealth;

        PlayerModel.s_instance.m_iHealth
        .Subscribe(m_iHealth => m_sliderThis.value = m_iHealth);
    }
}
