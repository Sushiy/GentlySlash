using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using BehaviourTree;

public class AIController : MonoBehaviour
{
    public static float m_fTickRate = 0.25f;
    AIModel m_aimodelThis;
    AIBehaviour m_aibehaviourThis;

    // Use this for initialization
    void Start()
    {
        m_aimodelThis = GetComponent<AIModel>();
        m_aibehaviourThis = new AIBehaviour(m_aimodelThis);
        {
            Selector selectorIdleOrAction = new Selector(m_aibehaviourThis);
            {
                Sequence sequenceConditionsOrIdle = new Sequence(selectorIdleOrAction);
                {
                    //if the player is dead => idle
                    //if the player is not dead and not in range => idle
                    //if the payer is not dead and in range => idle
                    Selector selectorDeadOrNotInRange = new Selector(sequenceConditionsOrIdle);
                        Condition_PlayerDead conditionDead = new Condition_PlayerDead(selectorDeadOrNotInRange, m_aibehaviourThis);
                    Inverter inverterNotInRange = new Inverter(selectorDeadOrNotInRange);
                        Condition_InDetectionRange conditionInRange = new Condition_InDetectionRange(inverterNotInRange, m_aibehaviourThis);
                    IdleTask idleTask = new IdleTask(sequenceConditionsOrIdle, m_aibehaviourThis);
                }
                Selector selectorFleeOrFight = new Selector(selectorIdleOrAction);
                {
                    Sequence sequenceConditionsThenFlee = new Sequence(selectorFleeOrFight);
                    {
                        Selector selectorTimerOrHealth = new Selector(sequenceConditionsThenFlee);
                        {
                            Condition_FleeTimerRunning conditionFleeDelay = new Condition_FleeTimerRunning(selectorTimerOrHealth, m_aibehaviourThis);
                            Condition_HealthLow conditionHealthHighEnough = new Condition_HealthLow(selectorTimerOrHealth, m_aibehaviourThis);
                        }
                        FleeTask fleetask = new FleeTask(sequenceConditionsThenFlee, m_aibehaviourThis);
                    }
                    AttackTask attackTask = new AttackTask(selectorFleeOrFight, m_aibehaviourThis);
                }
            }
        }   
        m_aibehaviourThis.StartBehaviour();
        InvokeRepeating("UpdateAIBehaviour", m_fTickRate, m_fTickRate);

        m_aimodelThis.m_modelstateCurrent
        .Where(m_modelstateCurrent => m_modelstateCurrent == ModelState.Dead)
        .Subscribe(m_modelstateCurrent => CancelInvoke());
    }

    protected void UpdateAIBehaviour()
    {
        m_aibehaviourThis.Tick();
        m_aimodelThis.Tick();       
    }
}
