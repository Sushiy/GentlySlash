using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleTask : TaskNode
{
    ParentNode m_parent;
    Behaviour m_behaviourRoot;

    EnemyModel m_enemymodel;

    public IdleTask(ParentNode _parent, Behaviour _behaviourRoot)
    {
        m_parent = _parent;
        m_behaviourRoot = _behaviourRoot;
    }

    public void Activate()
    {
        m_behaviourRoot.activateTask(this);
    }

    public void Deactivate()
    {
        m_behaviourRoot.deactivateTask(this);
    }
    
    //Successfully Idle if the player is not in your detectionrange, else your fail
    public void PerformTask()
    {
        if(m_enemymodel.IsPlayerInDetectionRange())
        {
            m_parent.ChildDone(this, false);
        }
        else
        {
            m_parent.ChildDone(this, true);
        }
    }
}
