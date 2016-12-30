using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BehaviourTree
{
    //This is the base of any Behaviour
    public class AIBehaviour : ParentNode
    {
        public static string s_strDebugBehaviourRun = "Behaviour Run: "; //A string variable that gets written by each parent when a child is done; Useful for checking the run of a behaviour when debugging
        bool m_bIsRunning = false;  //is the behaviour currently running?
        ChildNode m_childnodeRoot;  //The rootnode of the behaviour; the first node to be activated
        List<TaskNode> m_listtasknodeActive;    //All active tasknodes

        AIModel m_aimodelSelf;

        //Standard Constructor
        public AIBehaviour(AIModel _aimodel)
        {
            m_aimodelSelf = _aimodel;
            m_listtasknodeActive = new List<TaskNode>();
        }

        //Start the behaviour by activating its childnode
        public void StartBehaviour()
        {
            if (m_childnodeRoot == null)
            {
                return;
            }
            s_strDebugBehaviourRun = "Behaviour Durchlauf: ";
            m_childnodeRoot.Activate();
            m_bIsRunning = true;
        }

        //Tick is called at fixed Intervals and not at every Frame
        public void Tick()
        {
            //If the Behaviour is not running or no Tasks are active
            if ((!m_bIsRunning || m_listtasknodeActive.Count < 1))
            {
                StartBehaviour();   //Start the Behaviour again
            }
            //Otherwise Perform all Running Tasks
            for (int i = 0; i < m_listtasknodeActive.Count; ++i)
            {
                m_listtasknodeActive[i].PerformTask();
            }

        }

        //Set the RootNode
        public void AddChild(ChildNode _child)
        {
            m_childnodeRoot = _child;
        }

        //Callback from your ChildNode
        public void ChildDone(ChildNode _child, bool _bChildResult)
        {
            s_strDebugBehaviourRun += "\n" + _child.GetType().ToString() + ":" + _bChildResult + "|";
            //Debug.Log(s_strDebugBehaviourRun); //This would print the debugString to the console;
            m_bIsRunning = false;
        }

        public void activateTask(TaskNode _task)
        {
            m_listtasknodeActive.Add(_task);
        }

        public void deactivateTask(TaskNode _task)
        {
            m_listtasknodeActive.Remove(_task);
        }

        public List<TaskNode> GetActiveTasks()
        {
            return m_listtasknodeActive;
        }

        public void SetIsRunning(bool _b)
        {
            m_bIsRunning = _b;
        }

        public bool GetIsRunning()
        {
            return m_bIsRunning;
        }

        public AIModel Model
        {
            get
            {
                return m_aimodelSelf;
            }
        }
    }
}