using UnityEngine;
using System.Collections;
using System.Collections.Generic;

class InteractiveObject
{
	//TYPES:
	//0 - Enemies, Attackable Props (RED)
	//1 - NPCs, Signs, Other things you might read off (BLUE)
	//2 - Ladders, Holes, Switches, Things you can pick up (GREEN)
	public int targetType;
	public Transform targetTransform;
	public float distance;
	public string targetName;
	
	
	public InteractiveObject(int newType, Transform newTarget, string newName)
	{
		targetType = newType;
		targetTransform = newTarget;
		targetName = newName;
	}
	public InteractiveObject()
	{
		targetType = 0;
		targetTransform = null;
		targetName = "N/A";
	}
}


public class NaviGuide : MonoBehaviour {

	List<InteractiveObject> targetList = new List<InteractiveObject>();
	InteractiveObject currentTarget = new InteractiveObject();
	int currentTargetID = 0;
	bool targetMarkerExists;
	GameObject targetMarker;
	
	
	// Use this for initialization
	void Start () 
	{
		targetMarkerExists = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Update the distance of each item in the list
		if(targetMarkerExists && currentTarget == null)
		{
			switchTarget();
			Destroy(targetMarker);
			targetMarkerExists = false;
		}
		if(targetList.Count > 0)
		{
			foreach(InteractiveObject iteratedTarget in targetList)
			{
				iteratedTarget.distance = Vector3.Distance(transform.position, iteratedTarget.targetTransform.position);
				if(iteratedTarget.distance > 5.0f)
				{
					switchTarget();
					targetList.Remove(iteratedTarget);
				}
				
			}
			if(currentTargetID > targetList.Count)
			{
				switchTarget();
			}
			else
			{
				currentTarget = targetList[currentTargetID];
			}
			//Debug.Log ("Current Target: " + currentTarget.targetName + " (ID: " + currentTargetID + "/" + (targetList.Count - 1) + ")");
			
			if(!targetMarkerExists)
			{
				targetMarker = Instantiate(Resources.Load ("Graphical/NaviMarker")) as GameObject;
				targetMarkerExists = true;
			}
			else
			{
				targetMarker.transform.position = currentTarget.targetTransform.position;
				targetMarker.transform.position += new Vector3(0.0f, 0.7f, 0.0f);
			}
		}
		else
		{
			currentTargetID = 0; //Make sure that next time there is a target in the list, the list starts from the top.
			currentTarget = null;
			//Debug.Log ("No Targets");
			if(targetMarkerExists)
			{
				Destroy(targetMarker);
				targetMarkerExists = false;
			}
		}
		
	}
	
	void OnTriggerEnter(Collider other) 
	{
		if(other.tag == "IOEnemy")
		{
			if(targetList.Exists(InteractiveObject => InteractiveObject.targetName == other.name))
			{
			}
			else
			{
			InteractiveObject newEnemy = new InteractiveObject(0, other.transform, other.name);
			targetList.Add(newEnemy);
			Debug.Log ("Adding new Enemy to Navi-List - Type: " + newEnemy.targetType + " Name: " + newEnemy.targetName);
			}
		}
		if(other.tag == "IONPC")
		{
			InteractiveObject newNPC = new InteractiveObject(1, other.transform, other.name);
			targetList.Add (newNPC);
			Debug.Log ("Adding new NPC to Navi-List - Type: " + newNPC.targetType + " Name: " + newNPC.targetName);
		}
		if(other.tag == "IO")
		{
			InteractiveObject newObject = new InteractiveObject(2, other.transform, other.name);
			targetList.Add (newObject);
			Debug.Log ("Adding new NPC to Navi-List - Type: " + newObject.targetType + " Name: " + newObject.targetName);
		}
    }
	
	public void switchTarget()
	{
		if(targetList.Count == 0)
		{
			return;
		}
		else
		{
			if((currentTargetID + 1) > targetList.Count - 1)
			{
				currentTargetID = 0;
			}
			else
			{
				currentTargetID++;
			}
		}
	}
	
	public void killTarget(string targetName)
	{
		foreach(InteractiveObject iteratedTarget in targetList)
		{
			if(iteratedTarget.targetName == targetName)
			{
				Debug.Log ("Removing " + targetName + " from list.");
				switchTarget();
				targetList.Remove(iteratedTarget);
			}
				
		}
	}
}


