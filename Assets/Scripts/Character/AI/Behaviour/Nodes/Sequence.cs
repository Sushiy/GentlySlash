using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BehaviourTree
{
    //The Sequence activates all Children in succession;
    //Returns true when all children return true; fails otherwise
    public class Sequence : ChildNode, ParentNode
    {
        ParentNode m_parent;
        List<ChildNode> m_listChildren;
        int m_iCurrentChildIndex;

        // Use this for initialization
        public Sequence(ParentNode _parent)
        {
            this.m_parent = _parent;
            this.m_parent.AddChild(this);
            //Debug.Log("sequence constructed");
            m_listChildren = new List<ChildNode>();
        }

        public void AddChild(ChildNode _child)
        {
            m_listChildren.Add(_child);
        }

        public void ChildDone(ChildNode _child, bool _bChildResult)
        {
            AIBehaviour.s_strDebugBehaviourRun += _child.GetType().ToString() + ":" + _bChildResult + "->";
            //if child returns true, try the next child
            //if there is no next child return true to parent
            if (_bChildResult)
            {
                m_iCurrentChildIndex++;
                if (m_iCurrentChildIndex < m_listChildren.Count)
                {
                    m_listChildren[m_iCurrentChildIndex].Activate();
                }
                else
                {
                    m_parent.ChildDone(this, true);
                }

            }

            //if child returns false return false yourself
            else
            {
                m_parent.ChildDone(this, false);
            }

        }

        public void Activate()
        {
            //Debug.Log("Sequence activated");
            if (m_listChildren.Count > 0)
            {
                m_iCurrentChildIndex = 0;
                m_listChildren[m_iCurrentChildIndex].Activate();
            }
        }

        public void Deactivate()
        {
            for (int i = 0; i < m_listChildren.Count; ++i)
            {
                m_listChildren[i].Deactivate();
            }
        }
    }
}