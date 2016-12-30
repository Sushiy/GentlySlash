using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public enum ModelState
{
    Idle = 0,
    Walking,
    PickingUp,
    Attacking,
    Fleeing,
    Dead
}

public class Model : MonoBehaviourTrans
{
    #region Component References
    protected Movement m_movementThis;
    protected Health m_healthThis;
    protected Inventory m_inventoryThis;
    #endregion

    public ReactiveProperty<ModelState> m_modelstateCurrent = new ReactiveProperty<ModelState>(ModelState.Idle);    //Current State of the Player, starts Idle

    protected Model m_modelOpponent;            //The Enemy you are currently trying to attack
    public ReactiveProperty<bool> m_bIsInRange = new ReactiveProperty<bool>(false);        //Is the Player in CombatRange
    [SerializeField]
    protected float m_fUnarmedDamage = 1.0f;
    
    //Initialize all component references
    protected virtual void Awake()
    {
        m_movementThis = GetComponent<Movement>();
        m_healthThis = GetComponent<Health>();
        m_inventoryThis = GetComponent<Inventory>();
    }

    protected virtual void Start()
    {
        //Subscribe to the hasArrived property of Movement and call the corresponding Method if it is true
        m_movementThis.m_bHasArrived
        .Where(m_bHasArrived => m_bHasArrived == true)
        .Subscribe(m_bHasArrived => HasArrived());

        //Subscribe to the IsInRange property of Movement and Stop the Model if it is true
        m_bIsInRange
        .Where(m_bIsInRange => m_bIsInRange == true)
        .Subscribe(m_bIsInRange => Stop());

        //Subscribe to the health property of Health and kill Model if it is 0
        m_healthThis.m_fHealth
        .Where(m_fHealth => m_fHealth <= 0)
        .Subscribe(m_fHealth => Die());

    }

    private void Update()
    {
        CheckRange();
    }

    //Change to the given state
    public void ChangeToState(ModelState _playerstate)
    {
        m_modelstateCurrent.Value = _playerstate;
    }

    //Die by changing to Deadstate and stop regeneration; Maybe Despawn the enemy later
    protected virtual void Die()
    {
        Stop();
        ChangeToState(ModelState.Dead);
        Health.m_bRegenAllowed.Value = false;
    }

    protected virtual void HasArrived()
    {

    }

    //Stop The Model by Moving "to itself" and then check for range
    protected void Stop()
    {
        if(m_modelstateCurrent.Value == ModelState.Attacking)
        {
            Movement.MoveToAttack(m_modelOpponent.Movement.Position, Inventory.CombatRange);
            
        }
    }

    //Check if you are in CombatRange
    protected void CheckRange()
    {
        if(m_modelOpponent != null)
            m_bIsInRange.Value = Vector3.Distance(transform.position, m_modelOpponent.Movement.Position) <= Inventory.CombatRange;
    }

    //Animation EventReceiver

    //This Event is triggered from the attack animation;
    public void HitEvent()
    {
        if (m_bIsInRange.Value && Vector3.Angle(transform.forward, (m_modelOpponent.Movement.Position - transform.position)) <= 45)
        {
            float fDamage = Inventory.ActiveWeapon != null ? Inventory.ActiveWeapon.m_fDamage : m_fUnarmedDamage;
            m_modelOpponent.Health.TakeDamage(fDamage);
        }
    }

    //Turn to the opponent at the end of your swing
    public void MotionEndEvent()
    {
        if (m_bIsInRange.Value)
        {
            Movement.RotateTowards(m_modelOpponent.Movement.Position);
        }
    }

    //public Properties
    public Health Health
    {
        get
        {
            return m_healthThis;
        }
    }

    public Movement Movement
    {
        get
        {
            return m_movementThis;
        }
    }

    public Inventory Inventory
    {
        get
        {
            return m_inventoryThis;
        }
    }

    public ModelState CurrentState
    {
        get
        {
            return m_modelstateCurrent.Value;
        }
    }

    public Model Opponent
    {
        get
        {
            return m_modelOpponent;
        }
    }
}
