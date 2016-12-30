using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BehaviourTree
{
    //ParallelNode activates all children at the same time; 
    //it returns true if all children have returned true and false if any child returns false;
    public class Parallel : ChildNode, ParentNode
    {
        ParentNode m_parentnode;

        List<ChildNode> m_listChildren;
        List<bool> m_listbReturns;

        // Use this for initialization
        public Parallel(ParentNode _parent)
        {
            this.m_parentnode = _parent;
            this.m_parentnode.AddChild(this);
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
            AIBehaviour.s_strDebugBehaviourRun += "\n" + _child.GetType().ToString() + ":" + _bChildResult + "->";
            //if child returns true check if all children have now returned true
            //if so, return true
            if (_bChildResult)
            {
                int index = m_listChildren.IndexOf(_child);
                m_listbReturns[index] = true;

                //check if all children are true
                bool bAllTrue = true;
                for (int i = 0; i < m_listbReturns.Count; ++i)
                {
                    if (m_listbReturns[i] == false)
                        bAllTrue = false;
                }
                if (bAllTrue)
                {
                    for (int i = 0; i < m_listChildren.Count; ++i)
                    {
                        m_listChildren[i].Deactivate();
                    }
                    m_parentnode.ChildDone(this, true);
                }

            }
            //if child returns false return false yourself
            else
            {
                for (int i = 0; i < m_listChildren.Count; ++i)
                {
                    m_listChildren[i].Deactivate();
                }
                m_parentnode.ChildDone(this, false);
            }

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
                Debug.Log("ParallelNode has no children");
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