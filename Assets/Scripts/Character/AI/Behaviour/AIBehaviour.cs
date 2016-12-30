using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BehaviourTree
{
    //This is the base of any Behaviour
    public class AIBehaviour : ParentNode
    {
        public static string s_strBehaviourRun = "Behaviour Durchlauf: "; 
        bool m_bIsRunning = false;
        ChildNode m_childnodeRoot;
        List<TaskNode> m_listtasknodeActive;

        float fTimer = 0.0f;

        AIModel m_aimodelSelf;

        //Standard Constructor
        public AIBehaviour(AIModel _aimodel)
        {
            m_aimodelSelf = _aimodel;
            m_listtasknodeActive = new List<TaskNode>();
        }

        public void StartBehaviour()
        {
            if (m_childnodeRoot == null)
            {
                return;
            }
            s_strBehaviourRun = "Behaviour Durchlauf: ";
            m_childnodeRoot.Activate();
            m_bIsRunning = true;
        }

        //Tick is called at fixed Intervals and not at every Frame
        public void Tick()
        {
            //If the Behaviour is not running or no Tasks are active and it has been at least 0.5 seconds since the last time we tried this
            if ((!m_bIsRunning || m_listtasknodeActive.Count < 1) && Time.time - fTimer > 0.5f)
            {
                StartBehaviour();   //Start the Behaviour again
                fTimer = Time.time;
            }

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
            s_strBehaviourRun += "\n" + _child.GetType().ToString() + ":" + _bChildResult + "|";
            //Debug.Log(s_strBehaviourRun);
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