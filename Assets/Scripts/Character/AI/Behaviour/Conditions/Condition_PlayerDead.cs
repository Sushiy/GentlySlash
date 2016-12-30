using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class Condition_PlayerDead : ChildNode
    {

        ParentNode m_parent;

        public Condition_PlayerDead(ParentNode _parent, AIBehaviour _behaviourRoot)
        {
            m_parent = _parent;
            m_parent.AddChild(this);
        }

        public void Activate()
        {
            //Debug.Log("Is the Player Dead? " + (PlayerModel.s_instance.CurrentState == ModelState.Dead));
            if (PlayerModel.s_instance.CurrentState == ModelState.Dead)
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