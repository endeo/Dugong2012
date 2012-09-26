using UnityEngine;
using System.Collections;

public class ChestScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		foreach(Transform child in transform) 
		{
            child.renderer.material.color = new Color(0.451f, 0.373f, 0.275f);
		}
	}
	
	public void highlightChest()
	{
		foreach(Transform child in transform) 
		{
            child.renderer.material.color = new Color(0.9f, 0.0f, 0.0f);
		}
	}
	
}
