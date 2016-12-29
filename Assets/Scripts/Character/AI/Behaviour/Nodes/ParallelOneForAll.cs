using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BehaviourTree
{
    //This is a special Parallelclass that stops all its children as soon as one is done;
    //It then returns that result
    public class ParallelOneForAll : ChildNode, ParentNode
    {
        ParentNode m_parent;

        List<ChildNode> m_listChildren;
        List<bool> m_listbReturns;

        // Use this for initialization
        public ParallelOneForAll(ParentNode _parent)
        {
            this.m_parent = _parent;
            this.m_parent.AddChild(this);
            m_listChildren = new List<ChildNode>();
            m_listbReturns = new List<bool>();
        }

        public void AddChild(ChildNode _child)
        {
            m_listChildren.Add(_child);
            m_listbReturns.Add(new bool());
        }

        public void ChildDone(ChildNode _child, bool _bChildResult)
        {
            for (int i = 0; i < m_listChildren.Count; i++)
            {
                m_listChildren[i].Deactivate();
            }
            m_parent.ChildDone(this, _bChildResult);

        }

        public void Activate()
        {
            if (m_listChildren.Count > 0)
            {
                for (int i = 0; i < m_listChildren.Count; ++i)
                {
                    m_listbReturns[i] = false;
                    m_listChildren[i].Activate();
                }
            }
            else
            {
                Debug.Log("Parallel: no children");
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