using UnityEngine;
using System.Collections;

namespace BehaviourTree
{
    //The TrueNode always returns true
    public class True : ChildNode, ParentNode
    {
        ParentNode m_parent;
        ChildNode m_child;

        public True(ParentNode _parent)
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
            m_parent.ChildDone(this, true);
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
