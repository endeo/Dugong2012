using UnityEngine;
using System.Collections;

public class InventoryController : MonoBehaviour {
	
	public GameObject Helmet2;
	public GameObject Weapon1;
	public GameObject Weapon2;
	public GameObject InventoryMarker;
	
	GameObject WeaponMarker;
	GameObject HelmetMarker;
	GameObject TrinketMarker;
	
	// Use this for initialization
	void Start () 
	{
		WeaponMarker = Instantiate(InventoryMarker, Vector3.zero, new Quaternion(0.5f, 0.0f, 0.0f, -0.5f)) as GameObject;
		WeaponMarker.transform.parent = transform.FindChild("InventoryPlane");
		HelmetMarker = Instantiate(InventoryMarker, Vector3.zero, new Quaternion(0.5f, 0.0f, 0.0f, -0.5f)) as GameObject;
		HelmetMarker.transform.parent = transform.FindChild("InventoryPlane");
	}
	
	// Update is called once per frame
	void Update () 
	{
		switch(PlayerPrefs.GetInt("HelmetID", 0))
		{
		case 0:
			HelmetMarker.transform.position = Vector3.Lerp(HelmetMarker.transform.position, (transform.Find("InventoryPlane").transform.position + new Vector3(-1.32f, -0.48f, -0.05f)), 0.5f);
			break;
		case 1:
			HelmetMarker.transform.position = Vector3.Lerp(HelmetMarker.transform.position, (transform.Find("InventoryPlane").transform.position + new Vector3(-1.02f, -0.48f, -0.05f)), 0.5f);
			break;
		default:
			break;
		}
		switch(PlayerPrefs.GetInt("WeaponID", 0))
		{
		case 0:
			WeaponMarker.transform.position = Vector3.Lerp(WeaponMarker.transform.position, (transform.Find("InventoryPlane").transform.position + new Vector3(-1.32f, -0.48f, 1000.05f)), 0.5f);
			break;
		case 1:
			WeaponMarker.transform.position = Vector3.Lerp(WeaponMarker.transform.position, (transform.Find("InventoryPlane").transform.position + new Vector3(-1.32f, 0.075f, -0.05f)), 0.5f);
			break;
		case 2:
			WeaponMarker.transform.position = Vector3.Lerp(WeaponMarker.transform.position, (transform.Find("InventoryPlane").transform.position + new Vector3(-1.03f, 0.075f, -0.05f)), 0.5f);
			break;
		default:
			break;
		}
	}
	
	public void UpdateInventoryList ()
	{
		if(PlayerPrefs.GetInt("HasHelmet2", 0) == 1)
		{
			if(transform.Find("InventoryPlane/Helmet2(Clone)") == null)
			{
				GameObject helmetTwo = Instantiate(Helmet2, (transform.FindChild("InventoryPlane").transform.position + new Vector3(-0.55f, -1.03f, 0.1f)), new Quaternion(0.0f, 1.0f, 0.0f, 0.0f)) as GameObject;
				helmetTwo.transform.parent = transform.FindChild("InventoryPlane");
				helmetTwo.transform.localPosition = new Vector3(-0.55f, -1.03f, 0.1f);
			}
		}
		if(PlayerPrefs.GetInt("HasWeapon1", 0) == 1)
		{
			if(transform.Find("InventoryPlane/Weapon1(Clone)") == null)
			{
				GameObject weaponOne = Instantiate(Weapon1, (transform.FindChild("InventoryPlane").transform.position + new Vector3(-1.55f, -1.03f, 0.1f)), new Quaternion(0.0f, 0.5f, 0.0f, 5.0f)) as GameObject;
				weaponOne.transform.parent = transform.FindChild("InventoryPlane");
				weaponOne.transform.Rotate(new Vector3(-10.0f, -90.0f, 0.0f), Space.Self);
				weaponOne.transform.localPosition = new Vector3(-0.039f, -1.31f, 0.1f);
			}
		}
		if(PlayerPrefs.GetInt("HasWeapon2", 0) == 1)
		{
			if(transform.Find("InventoryPlane/Weapon2(Clone)") == null)
			{
				GameObject weaponTwo = Instantiate(Weapon2, (transform.FindChild("InventoryPlane").transform.position + new Vector3(-1.55f, -1.03f, 0.1f)), new Quaternion(0.0f, 0.0f, 0.0f, 1.0f)) as GameObject;
				weaponTwo.transform.parent = transform.FindChild("InventoryPlane");
				weaponTwo.transform.Rotate(new Vector3(-80.0f, 90.0f, 0.0f), Space.Self);
				weaponTwo.transform.localPosition = new Vector3(-0.039f, -1.04f, 0.1f);
			}
		}
	}
}
