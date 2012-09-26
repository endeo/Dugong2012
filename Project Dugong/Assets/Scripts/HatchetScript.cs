//2012 - Martin Smith
//HatchetScript.cs
//For use of the Hatchet in-game weapon.
//Controls interaction with destructible objects and enemies.
//I love you Laura :)

using UnityEngine;
using System.Collections;

public class HatchetScript : MonoBehaviour 
{
	//Global Variables
	static bool isAttacking;
	
	
	//Initialization
	void Start () 
	{
		isAttacking = false;
	}
	
	//Collision for Various Objects
	void OnTriggerEnter(Collider other) 
	{
		if(other.name == "Pot01")
		{
			if(isAttacking)
			{
				other.GetComponent<PhysPotScript>().BreakPot();
			}
		}
		
	}
	
	public void initAttack()
	{
		StartCoroutine(delayAttack());
	}
	
	IEnumerator delayAttack()
	{
		if(isAttacking == false)
		{
			print ("Attacking");
			isAttacking = true;
			yield return new WaitForSeconds(0.48f);
			isAttacking = false;
			print ("Stop Attack");
		}
	}
	
}
