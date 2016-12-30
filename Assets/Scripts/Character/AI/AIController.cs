using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using BehaviourTree;

//This class controls the AIBehaviour and creates the Behaviourtree
public class AIController : MonoBehaviour
{
    public static float m_fTickRate = 0.25f;    //the rate at which the AI is updated; in seconds
    AIModel m_aimodelThis;                      //The AImodel to be controlled
    AIBehaviour m_aibehaviourThis;              //The Behaviour to be used for this AI

    // Use this for initialization
    void Start()
    {
        m_aimodelThis = GetComponent<AIModel>();    //Initialize the model


#pragma warning disable 0219
        //From here on the Behaviourtree is constructed; For more info on this you can find a graphic of the tree: 
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
#pragma warning restore 0219
        m_aibehaviourThis.StartBehaviour(); //After constructing the Behaviour, start it
        InvokeRepeating("UpdateAIBehaviour", m_fTickRate, m_fTickRate); //Start regularly updating the AI after one "tick"

        //Subscribe to the Model to see if it is Dead in order to stop ticking
        m_aimodelThis.m_modelstateCurrent
        .Where(m_modelstateCurrent => m_modelstateCurrent == ModelState.Dead)
        .Subscribe(m_modelstateCurrent => CancelInvoke());
    }

    protected void UpdateAIBehaviour()
    {
        m_aibehaviourThis.Tick();   //Update the behaviour
        m_aimodelThis.Tick();       //Update the model
    }
}
