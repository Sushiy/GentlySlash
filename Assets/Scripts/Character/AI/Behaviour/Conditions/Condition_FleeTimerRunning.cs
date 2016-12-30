using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class Condition_FleeTimerRunning : ChildNode
    {

        ParentNode m_parent;
        AIModel m_aimodelSelf;

        public Condition_FleeTimerRunning(ParentNode _parent, AIBehaviour _behaviourRoot)
        {
            m_parent = _parent;
            m_parent.AddChild(this);
            m_aimodelSelf = _behaviourRoot.Model;
        }

        public void Activate()
        {
            //Debug.Log("FleeDelay is:" + (Time.time - m_aimodelSelf.FleeTimeStart) + "/" + m_aimodelSelf.m_fFleeTime);
            if ((Time.time - m_aimodelSelf.FleeTimeStart) <= m_aimodelSelf.m_fFleeTime)
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