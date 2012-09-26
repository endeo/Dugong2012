using UnityEngine;
using System.Collections;

public class InventoryItemScript : MonoBehaviour 
{
	bool isHovered;
	Quaternion primeRotation;
	
	void Start()
	{
		primeRotation = transform.rotation;
	}
	
	void OnMouseOver() 
	{
		isHovered = true;
    	transform.Rotate(0.0f, -1.8f, 0.0f, Space.World);
    }
	
	void Update() 
	{
		if(!isHovered)
		{
			transform.rotation = Quaternion.Lerp(transform.rotation, primeRotation , 0.1f);
		}
		isHovered = false;
    }
}