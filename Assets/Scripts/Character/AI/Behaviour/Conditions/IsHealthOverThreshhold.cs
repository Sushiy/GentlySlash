using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class IsHealthOverThreshhold : ChildNode
    {
        ParentNode m_parent;
        AIModel m_aimodelSelf;

        public IsHealthOverThreshhold(ParentNode _parent, AIBehaviour _behaviourRoot)
        {
            m_parent = _parent;
            m_parent.AddChild(this);
            m_aimodelSelf = _behaviourRoot.Model;
        }

        public void Activate()
        {
            Debug.Log("Are you Fit enough?" + (m_aimodelSelf.Health.m_fHealth.Value > m_aimodelSelf.m_fDamageBeforeFlee * m_aimodelSelf.Health.m_fHealth.Value));
            if (m_aimodelSelf.Health.m_fHealth.Value > m_aimodelSelf.m_fDamageBeforeFlee * m_aimodelSelf.Health.m_fHealth.Value)
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