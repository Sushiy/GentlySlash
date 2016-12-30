using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BehaviourTree
{
    //The Selector activates all children in succession; 
    //They are OR-linked, so if one of them returns true, the Selector returns true, if none do, it returns false
    public class Selector : ChildNode, ParentNode
    {
        ParentNode m_parent;

        List<ChildNode> m_listChildren;
        int m_iCurrentChildIndex;

        // Use this for initialization
        public Selector(ParentNode _parent)
        {
            this.m_parent = _parent;
            this.m_parent.AddChild(this);
            m_listChildren = new List<ChildNode>();
        }

        public void AddChild(ChildNode _child)
        {
            m_listChildren.Add(_child);
        }

        public void ChildDone(ChildNode _child, bool _bChildResult)
        {
            AIBehaviour.s_strDebugBehaviourRun += "\n" + _child.GetType().ToString() + ":" + _bChildResult + "->";
            //if child returns true, return true yourself
            if (_bChildResult)
            {
                m_parent.ChildDone(this, true);
            }

            //else try the next child if there is no next child return false
            else
            {
                m_iCurrentChildIndex++;
                if (m_iCurrentChildIndex < m_listChildren.Count)
                {
                    m_listChildren[m_iCurrentChildIndex].Activate();
                }
                else
                {
                    m_parent.ChildDone(this, false);
                }
            }

        }

        public void Activate()
        {
            //Debug.Log("Selector activated");
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