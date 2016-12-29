using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

[RequireComponent(typeof(Animator))]
public class AnimationViewer : MonoBehaviourTrans
{
    PlayerModel m_playermodelThis;
    Animator m_animatorThis;

	// Use this for initialization
	void Start ()
    {
        m_playermodelThis = GetComponent<PlayerModel>();
        m_animatorThis = GetComponent<Animator>();

        if (m_animatorThis == null)
        {
            Debug.LogError("AnimationViewer of " + gameObject.name + " could not find an Animator");
            return;
        }

        if (m_playermodelThis == null)
        {
            Debug.LogError("AnimationViewer of " + gameObject.name + " could not find an PlayerModel");
            return;
        }

        m_playermodelThis.m_playerstateCurrent
        .Subscribe(m_playerstateCurrent => PlayerStateChanged(m_playerstateCurrent));

        m_playermodelThis.m_bIsInRange
        .Subscribe(m_bIsInRange => IsInRangeChanged(m_bIsInRange));
    }

    private void Update()
    {
        m_animatorThis.SetBool("bMoving", m_playermodelThis.Movement.Velocity.magnitude > 0);
        m_animatorThis.SetFloat("fVelocity", m_playermodelThis.Movement.NormalizedVelocityMagnitude);
    }

    void PlayerStateChanged(PlayerState playerstate)
    {
        if (playerstate != PlayerState.Attacking)
            m_animatorThis.SetBool("bAttacking", false);
    }

    void IsInRangeChanged(bool _bInRange)
    {
        if(_bInRange)
            CheckWeaponType();
        m_animatorThis.SetBool("bAttacking", _bInRange);

    }

    public void CheckWeaponType()
    {
        m_animatorThis.SetBool("bIsUnarmed", m_playermodelThis.Inventory.ActiveWeapon == null);
        m_animatorThis.SetFloat("fAttackSpeed", 1.0f);

        if (m_playermodelThis.Inventory.ActiveWeapon != null)
        {
            m_animatorThis.SetBool("bHasPoleWeapon", m_playermodelThis.Inventory.ActiveWeapon.m_bIsPoleWeapon);
            m_animatorThis.SetFloat("fAttackSpeed", m_playermodelThis.Inventory.ActiveWeapon.m_fAttackSpeed);
        }
    }
}
