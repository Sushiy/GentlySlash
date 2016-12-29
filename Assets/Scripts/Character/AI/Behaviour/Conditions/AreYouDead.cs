using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class AreYouDead : ChildNode
    {

        ParentNode m_parent;
        AIModel m_aimodelSelf;

        public AreYouDead(ParentNode _parent, AIBehaviour _behaviourRoot)
        {
            m_parent = _parent;
            m_parent.AddChild(this);
            m_aimodelSelf = _behaviourRoot.Model;
        }

        public void Activate()
        {
            Debug.Log("AreYouDead? " + (m_aimodelSelf.CurrentState == ModelState.Dead));
            if (m_aimodelSelf.CurrentState == ModelState.Dead)
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