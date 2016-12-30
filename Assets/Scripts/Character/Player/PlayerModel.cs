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
    public static PlayerModel s_instance; //singleton instance

    WeaponPickup m_weaponPickUp;    //The weapon you are supposed to pickup. This is only needed for PickingUp State

    float m_fRepathRate = 0.25f;

    protected override void Awake()
    {
        base.Awake();
        //this class is a singleton, so if there is another one already, destroy this one
        if (s_instance != null)
        {
            Debug.Log("Singleton class: " + this.GetType().ToString() + " had another instance on: " + gameObject.ToString() + " which was destroyed.");
            Destroy(this);
            return;
        }
        s_instance = this;   //init singleton instance
    }

    public void ClickAction(RaycastHit _rchitClick)
    {
        CancelInvoke();
        Vector3 v3TargetPos;
        GameObject goClicked = _rchitClick.collider.gameObject;

        //If you clicked an Enemy start attacking him
        if (goClicked.layer == 10 /*Enemy*/)
        {
            v3TargetPos = goClicked.transform.position; //Set the target position to the position of your enemy
            v3TargetPos.y = 0;                          //Set y coordinate of target to 0

            m_modelOpponent = goClicked.GetComponent<Model>();  //Set your opponent
            m_movementThis.MoveToAttack(v3TargetPos, Inventory.CombatRange, m_modelOpponent.Movement.Velocity); //Try to move into a combatposition with your opponent
            ChangeToState(ModelState.Attacking);

            //Subscribe to the health property of Health and kill Model if it is 0
            m_modelOpponent.Health.m_fHealth
            .Where(m_fHealth => m_fHealth <= 0)
            .Subscribe(m_fHealth => ChangeToState(ModelState.Idle));

            InvokeRepeating("RepathToOpponent", m_fRepathRate, m_fRepathRate);  //Invoke the Repath Method 
        }

        //If you clicked on a weapon go and pick it up
        if (goClicked.layer == 9 /*Weapon*/)
        {
            v3TargetPos = goClicked.transform.position; //set the target position to the position of the GO
            v3TargetPos.y = 0;                          //set y coordinate to 0
            m_movementThis.MoveTowards(v3TargetPos);    //Move towards the target
            m_weaponPickUp = goClicked.GetComponent<WeaponPickup>();    //set the weaponpickup target
            ChangeToState(ModelState.PickingUp);    //change into pickupstate
        }
        //If you clicked on the ground, move to the hitlocation
        if (goClicked.layer == 8 /*Ground*/)
        {
            v3TargetPos = _rchitClick.point;        //set target to raycasthit point
            v3TargetPos.y = 0;                      //set y coordinate to 0
            m_movementThis.MoveTowards(v3TargetPos);    //Move to the target
            ChangeToState(ModelState.Walking);      //change into walkingstate
        }
    }

    //This is called when the model changes into the Die state
    protected override void Die()
    {
        base.Die();
        StartCoroutine(MenuController.s_instance.OpenPauseAfterDelay(3.0f));
    }

    //This is triggered when the agent arrives at his target
    protected override void HasArrived()
    {
        //If you are picking up, pickup the weapon, duh!
        if (CurrentState == ModelState.PickingUp)
        {
            m_inventoryThis.TakeWeapon(m_weaponPickUp.m_weaponThis);    //Take the weapon
            Destroy(m_weaponPickUp.gameObject);                         //Destroy the pickupgameobject
            m_weaponPickUp = null;                                      //reset the pickuptarget
            m_modelstateCurrent.Value = ModelState.Idle;                //Become Idle
        }

        //If you are attacking, try to rotate towards your target
        if (CurrentState == ModelState.Attacking)
        {
            Movement.LookAt(m_modelOpponent.Movement.Position);  //Rotate to the target
        }
    }

    //Repath to your Opponent if you aren't in range yet and your opponent has moved
    private void RepathToOpponent()
    {
        //if you are not, and he has moved or you dont have a path, move to him again
        if (!Movement.Agent.hasPath || Movement.Target != m_modelOpponent.Movement.Position)
        {
            Movement.MoveToAttack(m_modelOpponent.Movement.Position, Inventory.CombatRange, m_modelOpponent.Movement.Velocity); //Move to the target
        }
    }
}
