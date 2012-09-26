using UnityEngine;
using System.Collections;

public class PlayerFacer : MonoBehaviour {

	public GameObject TargetPlayer;
	
	// Update is called once per frame
	void Update () 
	{
		transform.LookAt(TargetPlayer.transform.FindChild("You/Head"));
		transform.Rotate(90.0f, 0.0f, 0.0f);
	}
}
