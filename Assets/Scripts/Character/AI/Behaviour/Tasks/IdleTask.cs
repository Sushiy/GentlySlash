using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class IdleTask : TaskNode
    {
        ParentNode m_parent;
        AIBehaviour m_behaviourRoot;

        AIModel m_aimodelSelf;

        public IdleTask(ParentNode _parent, AIBehaviour _behaviourRoot)
        {
            m_parent = _parent;
            m_parent.AddChild(this);
            m_behaviourRoot = _behaviourRoot;
            m_aimodelSelf = m_behaviourRoot.Model;
        }

        public void Activate()
        {
            m_behaviourRoot.activateTask(this);
            m_aimodelSelf.ChangeToState(ModelState.Idle);
        }

        public void Deactivate()
        {
            m_behaviourRoot.deactivateTask(this);
        }

        //Successfully Idle if the player is not in your detectionrange, else your fail
        public void PerformTask()
        {
                m_parent.ChildDone(this, true);
        }
    }
}

