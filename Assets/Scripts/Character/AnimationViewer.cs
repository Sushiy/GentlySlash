using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

//This class manages the animations for the shared animatorController of Player and AI;
//It is mostly based on the current state of the Model it is observing
[RequireComponent(typeof(Animator))]
public class AnimationViewer : MonoBehaviourTrans
{
    Model m_modelThis;  //The model to take the state from
    Animator m_animatorThis;    //The animator to call to

	// Use this for initialization
	void Start ()
    {
        //Initialize componentreferences
        m_modelThis = GetComponent<Model>();
        m_animatorThis = GetComponent<Animator>();

        //If the animator or the model is not found, stop the Startmethod and throw an error
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

        //Subscribe to the current state of the model and call the corresponding method if any change occurs
        m_modelThis.m_modelstateCurrent
        .Subscribe(m_playerstateCurrent => ModelStateChanged(m_playerstateCurrent));

        //Subscribe to the IsInCombatRange property to know when you are allowed to attack
        m_modelThis.m_bIsInCombatRange
        .Subscribe(m_bIsInCombatRange => IsInCombatRangeChanged(m_bIsInCombatRange));

        //Subscribe to the Active weapon of the Model
        m_modelThis.Inventory.m_iActiveWeapon
        .Subscribe(m_iActiveWeapon => CheckWeaponType());
    }

    private void Update()
    {
        m_animatorThis.SetBool("bMoving", m_modelThis.Movement.Velocity.magnitude > 0); //Always set the bMoving parameter
        m_animatorThis.SetFloat("fVelocity", m_modelThis.Movement.NormalizedVelocityMagnitude); //Always set the fVelocity parameter
    }

    //This is called when the state of the model has changed
    void ModelStateChanged(ModelState _modelstate)
    {
        //if the model is not in attackingState, set bAttacking to false
        if (_modelstate != ModelState.Attacking)
            m_animatorThis.SetBool("bAttacking", false);

        //If the Model is Dead, set the tDied trigger
        if (_modelstate == ModelState.Dead)
            m_animatorThis.SetTrigger("tDied");
    }

    //This is called when isInCombatRange has changed
    void IsInCombatRangeChanged(bool _bInRange)
    {
        m_animatorThis.SetBool("bAttacking", _bInRange);    //set bAttacking to isinCombatRange
    }


    //This checks the weapon the player is currently holding to see if he needs to change animations or attakspeed
    public void CheckWeaponType()
    {
        if (m_modelThis.Inventory.ActiveWeapon == null)
        {
            //Set basevalue for unarmed attacks if you aren't holding a weapon
            m_animatorThis.SetBool("bIsUnarmed", true);
            m_animatorThis.SetBool("bHasPoleWeapon", false);
            m_animatorThis.SetFloat("fAttackSpeed", 1.0f);
        }
        else
        {
            //If you have a weapon, check if its a polearm and set it's attackspeed
            m_animatorThis.SetBool("bHasPoleWeapon", m_modelThis.Inventory.ActiveWeapon.m_bIsPoleWeapon);
            m_animatorThis.SetFloat("fAttackSpeed", m_modelThis.Inventory.ActiveWeapon.m_fAttackSpeed);

        }

    }
}
