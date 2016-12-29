using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class AttackTask : TaskNode
    {
        ParentNode m_parent;
        AIBehaviour m_behaviourRoot;

        AIModel m_aimodelSelf;

        public AttackTask(ParentNode _parent, AIBehaviour _behaviourRoot)
        {
            m_parent = _parent;
            m_parent.AddChild(this);
            m_behaviourRoot = _behaviourRoot;
            m_aimodelSelf = m_behaviourRoot.Model;
        }

        public void Activate()
        {
            m_behaviourRoot.activateTask(this);
            m_aimodelSelf.ChangeToState(ModelState.Attacking);
        }

        public void Deactivate()
        {
            m_behaviourRoot.deactivateTask(this);
        }

        //Follow the Player
        public void PerformTask()
        {
            //If the Player leaves your DetectionRange Or the player is dead
            //Return false
            if (!m_aimodelSelf.IsPlayerInDetectionRange() || PlayerModel.s_instance.CurrentState == ModelState.Dead)
            {
                m_parent.ChildDone(this, false);
            }
            else
            {
                m_parent.ChildDone(this, true);
            }
        }
    }
}