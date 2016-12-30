using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class Condition_HealthLow : ChildNode
    {
        ParentNode m_parent;
        AIModel m_aimodelSelf;

        public Condition_HealthLow(ParentNode _parent, AIBehaviour _behaviourRoot)
        {
            m_parent = _parent;
            m_parent.AddChild(this);
            m_aimodelSelf = _behaviourRoot.Model;
        }

        public void Activate()
        {
            //Debug.Log("Is your Health Low?" + m_aimodelSelf.Health.m_fHealth.Value + "/" + (m_aimodelSelf.m_fDamageThreshhold * m_aimodelSelf.Health.m_fMaxHealth));
            if (m_aimodelSelf.Health.m_fHealth.Value < m_aimodelSelf.m_fDamageThreshhold * m_aimodelSelf.Health.m_fMaxHealth)
            {
                m_parent.ChildDone(this, true);
            }
            else
            {
                m_parent.ChildDone(this, false);
            }
        }

        public void Deactivate()
        {

        }
    }
}