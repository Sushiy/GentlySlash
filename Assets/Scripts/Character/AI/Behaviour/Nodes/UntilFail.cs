using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BehaviourTree
{
    //UntilFail reactivates it child over and over until it fails;
    //then it returns true;
    public class UntilFail : ChildNode, ParentNode
    {
        ParentNode m_parent;
        ChildNode m_child;

        public UntilFail(ParentNode _parent)
        {
            this.m_parent = _parent;
            this.m_parent.AddChild(this);
        }

        public void AddChild(ChildNode _child)
        {
            this.m_child = _child;
        }

        public void ChildDone(ChildNode _child, bool _bChildResult)
        {
            AIBehaviour.s_strDebugBehaviourRun += "\n" + _child.GetType().ToString() + ":" + _bChildResult + "->";
            //if child returns true activate it again
            if (_bChildResult)
            {
                _child.Activate();
            }

            //else return true
            else
            {
                m_parent.ChildDone(this, true);
            }
        }

        public void Activate()
        {
            if (m_child != null)
            {
                m_child.Activate();
            }
        }
        public void Deactivate()
        {
            m_child.Deactivate();
        }
    }
}