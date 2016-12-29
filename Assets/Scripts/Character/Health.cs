using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

[System.Serializable]
public class Health : MonoBehaviourTrans
{
    public float m_fMaxHealth = 100.0f; // Maximum Health value, Health will replenish over time up to this amount
    public float m_fRegenRate = 1.0f;   // Rate at which the player will regenerate Health Per Second
    public float m_fRegenDelay = 1.0f; // Regeneration will be delayed by this time in seconds after you took damage
    public ReactiveProperty<bool> m_bRegenAllowed = new ReactiveProperty<bool>(true); //Is the health allowed to regenerate?
    public ReactiveProperty<float> m_fHealth;   //Current health value

    void Awake()
    {
        m_fHealth = new ReactiveProperty<float>(m_fMaxHealth);
    }
    
    void Update()
    {
        if(m_bRegenAllowed.Value)
            m_fHealth.Value += m_fRegenRate * Time.deltaTime;
    }

    public void TakeDamage(float _fDamage)
    {
        m_fHealth.Value = Mathf.Max(m_fHealth.Value - _fDamage, 0);
        StopAllCoroutines();
        StartCoroutine(DelayRegeneration());
    }

    public IEnumerator DelayRegeneration()
    {
        m_bRegenAllowed.Value = false;
        yield return new WaitForSeconds(m_fRegenDelay);
        m_bRegenAllowed.Value = true;
    }
}
