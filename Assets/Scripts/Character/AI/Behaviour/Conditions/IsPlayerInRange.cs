﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class IsPlayerInRange : ChildNode
    {
        ParentNode m_parent;
        AIModel m_aimodelSelf;

        public IsPlayerInRange(ParentNode _parent, AIBehaviour _behaviourRoot)
        {
            m_parent = _parent;
            m_parent.AddChild(this);
            m_aimodelSelf = _behaviourRoot.Model;
        }

        public void Activate()
        {
            if (m_aimodelSelf.IsPlayerInDetectionRange())
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
