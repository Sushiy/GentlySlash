using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

[RequireComponent(typeof(Animator))]
public class AnimationViewer : MonoBehaviourTrans
{
    Model m_modelThis;
    Animator m_animatorThis;

	// Use this for initialization
	void Start ()
    {
        m_modelThis = GetComponent<Model>();
        m_animatorThis = GetComponent<Animator>();

        if (m_animatorThis == null)
        {
            Debug.LogError("AnimationViewer of " + gameObject.name + " could not find an Animator");
            return;
        }

        if (m_modelThis == null)
        {
            Debug.LogError("AnimationViewer of " + gameObject.name + " could not find an PlayerModel");
            return;
        }

        m_modelThis.m_modelstateCurrent
        .Subscribe(m_playerstateCurrent => PlayerStateChanged(m_playerstateCurrent));

        m_modelThis.m_bIsInRange
        .Subscribe(m_bIsInRange => IsInRangeChanged(m_bIsInRange));
    }

    private void Update()
    {
        m_animatorThis.SetBool("bMoving", m_modelThis.Movement.Velocity.magnitude > 0);
        m_animatorThis.SetFloat("fVelocity", m_modelThis.Movement.NormalizedVelocityMagnitude);
    }

    void PlayerStateChanged(ModelState _playerstate)
    {
        if (_playerstate != ModelState.Attacking)
            m_animatorThis.SetBool("bAttacking", false);

        if (_playerstate == ModelState.Dead)
            m_animatorThis.SetTrigger("tDied");
    }

    void IsInRangeChanged(bool _bInRange)
    {
        if(_bInRange)
            CheckWeaponType();
        m_animatorThis.SetBool("bAttacking", _bInRange);

    }

    public void CheckWeaponType()
    {
        m_animatorThis.SetBool("bIsUnarmed", m_modelThis.Inventory.ActiveWeapon == null);
        m_animatorThis.SetFloat("fAttackSpeed", 1.0f);

        if (m_modelThis.Inventory.ActiveWeapon != null)
        {
            m_animatorThis.SetBool("bHasPoleWeapon", m_modelThis.Inventory.ActiveWeapon.m_bIsPoleWeapon);
            m_animatorThis.SetFloat("fAttackSpeed", m_modelThis.Inventory.ActiveWeapon.m_fAttackSpeed);
        }
    }
}
