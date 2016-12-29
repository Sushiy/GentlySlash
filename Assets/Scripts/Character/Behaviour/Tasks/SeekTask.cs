using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekTask : TaskNode
{
    ParentNode m_parent;
    Behaviour m_behaviourRoot;

    EnemyModel m_enemymodel;

    public SeekTask(ParentNode _parent, Behaviour _behaviourRoot)
    {
        m_parent = _parent;
        m_behaviourRoot = _behaviourRoot;
    }

    public void Activate()
    {
        m_behaviourRoot.activateTask(this);
        m_enemymodel.Movement.MoveToAttack(PlayerModel.s_instance.Movement.Position, m_enemymodel.Inventory.CombatRange);
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
        if (!m_enemymodel.IsPlayerInDetectionRange() || PlayerModel.s_instance.IsDead)
        {
            m_parent.ChildDone(this, false);
        }

        if(m_enemymodel.Movement.HasArrived)
        {

        }
    }
}
