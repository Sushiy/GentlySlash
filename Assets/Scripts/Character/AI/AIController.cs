using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class AIController : MonoBehaviour
{
    public static float m_fTickRate = 0.5f;
    AIModel m_aimodelThis;
    AIBehaviour m_aibehaviourThis;

    // Use this for initialization
    void Start()
    {
        m_aimodelThis = GetComponent<AIModel>();
        m_aibehaviourThis = new AIBehaviour(m_aimodelThis);
        {
            Selector selectorDead = new Selector(m_aibehaviourThis);
            {
                AreYouDead conditionDead = new AreYouDead(selectorDead, m_aibehaviourThis);
                Selector selectorFlee = new Selector(selectorDead);
                {
                    Selector selectorFlee2 = new Selector(selectorFlee);
                    {
                        Sequence sequenceFlee = new Sequence(selectorFlee2);
                        {
                            IsFleeDelayOver conditionFleeDelay = new IsFleeDelayOver(sequenceFlee, m_aibehaviourThis);
                            IsHealthOverThreshhold conditionHealthHighEnough = new IsHealthOverThreshhold(sequenceFlee, m_aibehaviourThis);
                        }
                        FleeTask fleetask = new FleeTask(selectorFlee2, m_aibehaviourThis);
                    }
                    
                    Selector selectorIdleOrAttack = new Selector(selectorFlee);
                    {
                        IdleTask idleTask = new IdleTask(selectorIdleOrAttack, m_aibehaviourThis);
                        AttackTask attackTask = new AttackTask(selectorIdleOrAttack, m_aibehaviourThis);
                    }
                }
            }

        }
        m_aibehaviourThis.StartBehaviour();
        InvokeRepeating("UpdateAIBehaviour", m_fTickRate, m_fTickRate);
    }


    protected void UpdateAIBehaviour()
    {
        m_aibehaviourThis.Tick();
        m_aimodelThis.Tick();       
    }
}
