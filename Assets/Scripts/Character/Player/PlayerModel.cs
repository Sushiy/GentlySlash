using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UniRx;
[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Inventory))]
public class PlayerModel : Model
{
    public static PlayerModel s_instance;

    WeaponPickup m_weaponPickUp;    //The weapon you are supposed to pickup. This is only needed for PickingUp State

    float m_fRepathRate = 0.5f;

    protected override void Awake()
    {
        base.Awake();
        s_instance = this;
    }

    public void ClickAction(RaycastHit _rchitClick)
    {
        CancelInvoke();
        Vector3 v3TargetPos;
        GameObject goClicked = _rchitClick.collider.gameObject;

        //If you have clicked an Enemy
        if (goClicked.layer == 10 /*Enemy*/)
        {
            v3TargetPos = goClicked.transform.position; //Set the target position to the position of your enemy
            v3TargetPos.y = 0;                          //Set y coordinate of target to 0

            m_modelOpponent = goClicked.GetComponent<Model>();  //Set your opponent
            m_movementThis.MoveToAttack(v3TargetPos, Inventory.CombatRange);
            ChangeToState(ModelState.Attacking);

            //Subscribe to the health property of Health and kill Model if it is 0
            m_modelOpponent.Health.m_fHealth
            .Where(m_fHealth => m_fHealth <= 0)
            .Subscribe(m_fHealth => ChangeToState(ModelState.Idle));
            InvokeRepeating("RepathToOpponent", m_fRepathRate, m_fRepathRate);
            CheckRange();
        }

        if (goClicked.layer == 9 /*Weapon*/)
        {
            v3TargetPos = goClicked.transform.position;
            v3TargetPos.y = 0;
            m_movementThis.MoveTowards(v3TargetPos);
            m_weaponPickUp = goClicked.GetComponent<WeaponPickup>();
            ChangeToState(ModelState.PickingUp);
        }
        if (goClicked.layer == 8 /*Ground*/)
        {
            v3TargetPos = _rchitClick.point;
            v3TargetPos.y = 0;
            m_movementThis.MoveTowards(v3TargetPos);
            ChangeToState(ModelState.Walking);
        }
    }

    //This is triggered when the agent arrives at his target
    protected override void HasArrived()
    {
        //If you are picking up, pickup the weapon, duh!
        if (CurrentState == ModelState.PickingUp)
        {
            m_inventoryThis.TakeWeapon(m_weaponPickUp.m_weaponThis);
            Destroy(m_weaponPickUp.gameObject);
            m_weaponPickUp = null;
            m_modelstateCurrent.Value = ModelState.Idle;
        }

        //If you are attacking, try to rotate towards your target
        if (CurrentState == ModelState.Attacking)
        {
            CheckRange();
            Movement.RotateTowards(m_modelOpponent.Movement.Position);
        }
    }

    //Repath to your Opponent if you aren't in range yet and your opponent has moved
    private void RepathToOpponent()
    {
        CheckRange();
        if (!Movement.HasArrived && Movement.Target != m_modelOpponent.Movement.Position)
        {
            Movement.MoveToAttack(m_modelOpponent.Movement.Position, Inventory.CombatRange);
        }
    }
}
