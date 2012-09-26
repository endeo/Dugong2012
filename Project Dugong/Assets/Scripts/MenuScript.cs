using UnityEngine;
using System.Collections;

public class MenuScript : MonoBehaviour {
	
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnMouseOver() 
	{
        guiTexture.color = new Color(255.0F, 255.0F, 0.0f);
    }
	void OnMouseExit() 
	{
        guiTexture.color = new Color(255.0F, 255.0F, 255.0f);
    }
	
	void OnMouseDown () 
	{
		if(gameObject.name == "ButExit")
		{
			Application.Quit();
		}
		if(gameObject.name == "ButStartDemo")
		{
			Application.LoadLevel("DemoDungeon");
		}
	}
	
}
