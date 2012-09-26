using UnityEngine;
using System.Collections;

public class PressurePlateScript : MonoBehaviour {
	
	int PlateState = 0;
	
	void OnTriggerEnter(Collider other) 
	{
		if(PlateState == 0)
		{
			Debug.Log("On");
			transform.parent.gameObject.animation.Play("PlateOn");
        	switch(transform.name)
			{
			case "PressureTrigger1":
				Instantiate(Resources.Load ("Mobs/Goblin", typeof(GameObject)), new Vector3(-13.5f, -2.6f, -17.4f), Quaternion.identity);
				break;
			default:
				break;
			}
		}
		PlateState++;
    }
	
	void OnTriggerExit(Collider other) 
	{
        PlateState--;
		if(PlateState == 0)
		{
			Debug.Log("Off");
			transform.parent.gameObject.animation.Play("PlateOff");
		}
    }
	
}
