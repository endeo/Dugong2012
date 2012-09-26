using UnityEngine;
using System.Collections;

public class LiftableObject : MonoBehaviour {
	
	bool onFloor;
	
	public GameObject glowEffect;
	public GameObject targetPlayer;
	
	// Use this for initialization
	void Start () 
	{
		onFloor = true;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(onFloor)
		{
			if(transform.FindChild("KeyGlow(Clone)") == null)
			{
				GameObject newGlow;
				newGlow = Instantiate(glowEffect, transform.position, transform.rotation) as GameObject;
				newGlow.transform.parent = transform;
			}
			transform.Find("LargeKey01").animation.Play("idle");
		}
		else
		{
			transform.Find("LargeKey01").animation.Play("idle");
			//transform.Find("LargeKey01").animation.Stop("idle");
			if(transform.parent != targetPlayer.transform.FindChild("You/YouAscendedArms/RShoulder/RArm"))
			{
				transform.parent = targetPlayer.transform.FindChild("You/YouAscendedArms/RShoulder/RArm");
			}
			if(transform.FindChild("KeyGlow(Clone)") != null)
			{
				Destroy(transform.FindChild("KeyGlow(Clone)").gameObject);
			}
		}
			
	}
	
	public void toggleObject()
	{
		if(onFloor)
		{
			onFloor = false;
			//transform.Find("LargeKey01").animation.Stop();
			
		}
		else
		{
			onFloor = true;
		}
	}
}
