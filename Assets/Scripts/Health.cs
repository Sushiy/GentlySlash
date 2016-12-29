using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

[System.Serializable]
public class Health : MonoBehaviourTrans
{
    public float m_fMaxHealth = 100.0f; // Maximum Health value, Health will replenish over time up to this amount
    public float m_fRegenRate = 1.0f;   // Rate at which the player will regenerate Health Per Second
    public ReactiveProperty<float> m_fHealth;   //Current health value
    

    void Awake()
    {
        m_fHealth = new ReactiveProperty<float>(m_fMaxHealth);
    }
    
    void Update()
    {
        m_fHealth.Value += 1.0f * Time.deltaTime;
    }

    public void TakeDamage(int _iDamage)
    {
        m_fHealth.Value = Mathf.Max(m_fHealth.Value - _iDamage, 0);
    }
}
