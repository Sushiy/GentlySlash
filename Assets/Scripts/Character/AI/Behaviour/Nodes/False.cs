using UnityEngine;
using System.Collections;

namespace BehaviourTree
{
    //The FalseNode always returns False
    public class False : ChildNode, ParentNode
    {

        ParentNode m_parent;
        ChildNode m_child;

        public False(ParentNode _parent)
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
            m_parent.ChildDone(this, false);
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

