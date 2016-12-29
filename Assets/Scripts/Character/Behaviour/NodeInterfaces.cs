using UnityEngine;
using System.Collections;

//Childnode is activated and deactivated from above
public interface ChildNode
{
	void Activate();	
	void Deactivate();
}

//ParentNode has children
public interface ParentNode
{
	void AddChild(ChildNode _child);
	void ChildDone(ChildNode _child, bool _bChildResult);
}

//TaskNode can perform a task
public interface TaskNode : ChildNode
{
	void PerformTask();
}


