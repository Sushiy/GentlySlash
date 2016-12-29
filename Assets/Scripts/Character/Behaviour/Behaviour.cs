using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//This is the base of any Behaviour
public class Behaviour : ParentNode  
{
	bool m_bIsRunning = false;
	ChildNode m_childnodeRoot;
	List<TaskNode> m_listtasknodeActive;
	
	float fTimer = 0.0f;
	
    //Standard Constructor
	public Behaviour()
	{
		m_listtasknodeActive = new List<TaskNode>();
		Debug.Log ("Behaviour constructed");
	}

    public void StartBehaviour()
    {
        Debug.Log("Start Behaviour");
        if (m_childnodeRoot == null)
        {
            Debug.Log("Behaviour has no root");
            return;
        }

        m_childnodeRoot.Activate();
        m_bIsRunning = true;
    }

	//Tick is called at fixed Intervals and not at every Frame
	public void Tick() 
	{
        //If the Behaviour is not running or no Tasks are active and it has been at least 0.5 seconds since the last time we tried this
		if((!m_bIsRunning ||m_listtasknodeActive.Count < 1) && Time.time - fTimer > 0.5f)
		{
			StartBehaviour();   //Start the Behaviour again
			fTimer = Time.time;
		}

		for(int i = 0; i< m_listtasknodeActive.Count; ++i)
		{
			m_listtasknodeActive[i].PerformTask();
		}

	}

    //Set the RootNode
	public void AddChild(ChildNode child)
	{
		m_childnodeRoot = child;
	}

    //Callback from your ChildNode
	public void ChildDone(ChildNode child, bool childResult)
	{
		Debug.Log("Behaviour terminated with Result: " + childResult); 
		m_bIsRunning = false;
	}

	public void activateTask(TaskNode t)
	{
		m_listtasknodeActive.Add(t);
	}

	public void deactivateTask(TaskNode t)
	{
		m_listtasknodeActive.Remove(t);
	}


	public List<TaskNode> GetActiveTasks()
	{
		return m_listtasknodeActive;
	}

	public void SetIsRunning(bool b)
	{
		m_bIsRunning = b;
	}

	public bool GetIsRunning()
	{
		return m_bIsRunning;
	}
}
