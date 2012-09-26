using UnityEngine;
using System.Collections;

public class PhysPotScript : MonoBehaviour {
	
	static bool isBroken;
	public GameObject Corpse;
	
	// Use this for initialization
	void Start () {
		isBroken = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(isBroken == false)
		{
		}
		
	}
	
	public void BreakPot()
	{
		Instantiate(Corpse, transform.position, transform.rotation);
		Destroy(gameObject);
	}
	
	
}
