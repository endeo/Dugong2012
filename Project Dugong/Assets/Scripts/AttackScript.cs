using UnityEngine;
using System.Collections;

public class AttackScript : MonoBehaviour {
	
	public GameObject currentWeapon;
	
	// Use this for initialization
	void Start () 
	{
		
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetMouseButton(0))
		{
			
			
				
				animation.Play("SimpleAttack");
				//isAttacking = true;
				currentWeapon.GetComponent<HatchetScript>().initAttack();
				
			
		}
	}
	
}
