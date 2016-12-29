using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BehaviourTree
{
    public class FleeTask : TaskNode
    {
        ParentNode m_parent;
        AIBehaviour m_behaviourRoot;

        AIModel m_aimodelSelf;

        public FleeTask(ParentNode _parent, AIBehaviour _behaviourRoot)
        {
            m_parent = _parent;
            m_parent.AddChild(this);
            m_behaviourRoot = _behaviourRoot;
            m_aimodelSelf = m_behaviourRoot.Model;
        }

        public void Activate()
        {
            m_behaviourRoot.activateTask(this);
            m_aimodelSelf.Movement.MoveToAttack(PlayerModel.s_instance.Movement.Position, m_aimodelSelf.Inventory.CombatRange);
            m_aimodelSelf.ChangeToState(ModelState.Fleeing);
        }

        public void Deactivate()
        {
            m_behaviourRoot.deactivateTask(this);
        }

        //Follow the Player
        public void PerformTask()
        {
            m_parent.ChildDone(this, true);
        }
    }
}
