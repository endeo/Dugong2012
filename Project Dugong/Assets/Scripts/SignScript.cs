using UnityEngine;
using System.Collections;

public class SignScript : MonoBehaviour {
	
	static bool highlighted;
	static Color TargetColor;
	public GameObject TargetPlayer;
	
	// Use this for initialization
	void Start () {
		highlighted = false;
		foreach(Transform child in transform) 
		{
            child.renderer.material.color = new Color(1.0f, 1.0f, 1.0f);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(highlighted)
		{
			TargetColor = new Color(0.5f, 1.0f, 0.5f);
			foreach(Transform child in transform) 
			{
				if(child.renderer.material.color.r > TargetColor.r)
            	child.renderer.material.color -= new Color(0.05f, 0.0f, 0.05f);
			}
		}
		else
		{
			TargetColor = new Color(1.0f, 1.0f, 1.0f);
			foreach(Transform child in transform) 
			{
				if(child.renderer.material.color != TargetColor)
            	child.renderer.material.color += new Color(0.05f, 0.0f, 0.05f);
			}
		}
		highlighted = false;
	}
	
	public void highlightSign()
	{	
		highlighted = true;	
	}
	
	public void activateSign()
	{
		if(transform.name == "Sign001")
		{
				print("Activating Sign001");
				
				TargetPlayer.GetComponent<GUIScript>().ChangePage(0, "Well done!\nNotice how you may only move left or right in 2D rooms like these. Head downstairs to learn more about weapons.", "Sign", 3);
				TargetPlayer.GetComponent<PlayerController>().ChangePlayerState(2);
		}
		if(transform.name == "Sign002")
		{
				print("Activating Sign002");
				
				TargetPlayer.GetComponent<GUIScript>().ChangePage(0, "TESTICLES\nSEAN'S TEST SIGN - PAY NO ATTENTION TO HOW TESTY THIS SIGN IS!", "Sign", 0);
				TargetPlayer.GetComponent<PlayerController>().ChangePlayerState(2);
		}
	}
}
