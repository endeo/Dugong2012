using UnityEngine;
using System.Collections;

public class PlayerFacer : MonoBehaviour {

	public GameObject TargetPlayer;
	
	// Update is called once per frame
	
	void Start()
	{
		TargetPlayer = GameObject.Find("YouObject");
		Debug.Log ("I just spawned!");
	}
	
	void Update () 
	{
		transform.LookAt(TargetPlayer.transform.FindChild("Main Camera2"));
		transform.Rotate(90.0f, 0.0f, 0.0f);
		
		
	}
}
