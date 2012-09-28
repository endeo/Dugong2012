using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
	//Local Public and Public Variables
	//The character controller/cam attached to player
	static CharacterController controller = new CharacterController();
	public Transform cam;
	public Transform inventoryPrefab;
	static Transform theGameCharacter;
	public AudioClip footSteps;
	
	
	//Public Variables determining different stats
	static int PlayerState;
	// 0 - 3D active
	// 1 - Paused
	// 2 - Reading Sign
	// 3 - 2D Active
	// 4 - Picking stuff up
	// 5 - EXPIRIMENTAL 3RD PERSON
	int previousState;
	
	static int attackCycle = 0;
	float PlayerHealth;
	float attackInterval;
	float lastAttack;
	int isAttacking = 0;
	
	bool isCarrying;
	
	int pickupCycle = 0;
	
	
	public float rotationspeed = 0.1f;
	public float gravity = 5.0f;
	public float movespeed = 3.0F;
    public bool CameraFollow;
	
	//Camera Position related Variables
	static float CameraOffsetX;
	static float CameraOffsetY;
	static float CameraOffsetZ;
	static Quaternion CameraRotationY;
	
	//Movement related Variables
	static Vector3 movedir;
	static bool ismoving;
	static int LastDirection = 1;
	static Vector3 targetPoint;
	
	//How strong the player is in terms of moving other RigidBodies
	public float pushPower = 2.0F;
	
	//Equipment
	//Weapons
	public GameObject Hatchet;
	public GameObject SheepAxe;
	
	//Helmets
	public GameObject AscendedHelm;
	
	
	//Initialize Variables
	void Start () 
	{
		//Initialize Stats
		ChangePlayerState(5);
		attackCycle = 0;
		PlayerHealth = PlayerPrefs.GetFloat("PlayerHealth", 100.0f);
		previousState = PlayerState;
		isCarrying = false;
		
		//Change equipment on load
		switch(PlayerPrefs.GetInt ("HelmetID", 0))
		{
		case 0:
			ChangeHelmet("None");
			break;
		case 1:
			ChangeHelmet("Ascended");
			break;
		default:
			break;
		}
		
		switch(PlayerPrefs.GetInt ("WeaponID", 0))
		{
		case 0:
			ChangeWeapon("None");
			break;
		case 1:
			ChangeWeapon("Hatchet");
			break;
		case 2:
			ChangeWeapon("SheepAxe");
			break;
		default:
			break;
		}
		
		//Setup Character controller and Initiate Camera
		theGameCharacter = transform.FindChild("You");
		controller = GetComponent<CharacterController>();
		cam = Camera.main.transform;
		CameraFollow = false;
		CameraOffsetX = cam.transform.position.x;
		CameraOffsetY = cam.transform.position.y;
		CameraOffsetZ = cam.transform.position.z;
		CameraRotationY = cam.transform.rotation;
		
		Screen.showCursor = false;
		
	}
	
	
	// Update is called once per frame, regardless of Framerate
	void FixedUpdate () 
	{
		//
		//		Player Rotation
		//
		
		// 3D Rotation
		if (PlayerState == 0)
		{
			// Generate a plane that intersects the transform's position with an upwards normal.
		    Plane playerPlane = new Plane(Vector3.up, transform.position);
		
		    // Generate a ray from the cursor position
		    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		
		    // Determine the point where the cursor ray intersects the plane.
		    // This will be the point that the object must look towards to be looking at the mouse.
		    // Raycasting to a Plane object only gives us a distance, so we'll have to take the distance,
		    //   then find the point along that ray that meets that distance.  This will be the point
		    //   to look at.
		    float hitdist = 0.0f;
		    // If the ray is parallel to the plane, Raycast will return false.
		    if (playerPlane.Raycast (ray, out hitdist)) 
		    {
		        // Get the point along the ray that hits the calculated distance.
		        targetPoint = ray.GetPoint(hitdist);
		    
		        // Determine the target rotation.  This is the rotation if the transform looks at the target point.
		        Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
		    
		        // Smoothly rotate towards the target point.
		        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationspeed);
				theGameCharacter.transform.FindChild("Head").transform.rotation = Quaternion.Slerp(theGameCharacter.transform.FindChild("Head").transform.rotation, targetRotation, 0.15f);
		    }
		}
		
		//2D Rotation
		if (PlayerState == 3)
		{
			if(LastDirection == 0)
			{
				transform.rotation = Quaternion.Slerp(transform.rotation, new Quaternion(0.0f, 0.7f, 0.0f, 0.7f), 0.05f);
				theGameCharacter.transform.FindChild("Head").transform.rotation = Quaternion.Slerp(theGameCharacter.transform.FindChild("Head").transform.rotation, new Quaternion(0.0f, 0.7f, 0.0f, 0.7f), 0.06f);
			}
			if(LastDirection == 1)
			{
				transform.rotation = Quaternion.Slerp(transform.rotation, new Quaternion(0.0f, 0.7f, 0.0f, -0.7f), 0.05f);
				theGameCharacter.transform.FindChild("Head").transform.rotation = Quaternion.Slerp(theGameCharacter.transform.FindChild("Head").transform.rotation, new Quaternion(0.0f, 0.7f, 0.0f, -0.7f), 0.06f);
			}
		}
		
		if(isAttacking > 0)
		{
			if(isAttacking == 17)
			{
				transform.Find("You/YouAscendedArms/RShoulder/RArm/SheepAxe(Clone)").collider.enabled = true;
			}
			if(isAttacking == 25)
			{
				transform.Find("You/YouAscendedArms/RShoulder/RArm/SheepAxe(Clone)").collider.enabled = false;
			}
			isAttacking++;
			if(isAttacking == 30)
			{
				isAttacking = 0;
			}
		}
	}
	
   	
	//Called every frame that has actually loaded
	void Update() 
	{
		//Movement
		if (PlayerState == 5)
		{
			
			int moveCount = 0;
			if (Input.GetKey(KeyCode.W))
			{
				ismoving = true;
				movedir += transform.TransformDirection(Vector3.forward);
        		moveCount++;
				theGameCharacter.animation.CrossFade("WalkingFront", 0.3f);
			}
			if (Input.GetKey(KeyCode.A))
			{
				ismoving = true;
				movedir += transform.TransformDirection(Vector3.left);
	        	moveCount++;
			}
			if (Input.GetKey(KeyCode.S))
			{
				ismoving = true;
				movedir += transform.TransformDirection(Vector3.back);
	        	moveCount++;
			}
			if (Input.GetKey(KeyCode.D))
			{
				ismoving = true;
				movedir += transform.TransformDirection(Vector3.right);
	        	moveCount++;
			}
			
			if(ismoving)
			{
				controller.SimpleMove(movedir * (movespeed * (1.0f - (moveCount * 0.2f))));
				if(!audio.isPlaying)
				{
					audio.Play();
				}
			}
			
			if(!ismoving && controller.isGrounded == true)
			{
				audio.Stop();
				theGameCharacter.animation.CrossFade("idle", 0.2f);
			}
			
			if(controller.isGrounded == false && ismoving == false)
			{
				controller.SimpleMove(Vector3.down * (Time.deltaTime + gravity));
			}
			
			if(ismoving && controller.isGrounded)
			{
				if(attackCycle == 0 && !isCarrying)
				{
					theGameCharacter.FindChild("YouAscendedArms").animation.CrossFade("WalkingFront", 0.8f);
				}
			}
			else if(controller.isGrounded == false)
			{
			}
			else
			{
				if(attackCycle == 0)
				{
					theGameCharacter.FindChild("YouAscendedArms").animation.CrossFade("idle", 0.2f);
				}
			}
			
			
			float h = 2.0f * Input.GetAxis("Mouse X");
        	transform.Rotate(0, h, 0);
			
			movedir = new Vector3(0,0,0);
			ismoving = false;
			moveCount = 0;
			
		}
		
		
		//Let the camera slerp follow the player and rotate towards target
		if(CameraFollow)
		{
			Vector3 camupdater = Vector3.Slerp(cam.transform.position, (new Vector3((transform.position.x + CameraOffsetX), (transform.position.y + CameraOffsetY), (transform.position.z + CameraOffsetZ))), 0.05f);
			cam.transform.position = camupdater;
			cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, CameraRotationY, 0.05f);
		}
		
		//Pause Menu
		if (Input.GetKeyDown(KeyCode.E))
		{
			if(inventoryPrefab.camera.enabled)
			{
				inventoryPrefab.camera.enabled = false;
				ChangePlayerState(5);
			}
			else
			{
				inventoryPrefab.camera.enabled = true;
				ChangePlayerState(1);
				inventoryPrefab.GetComponent<InventoryController>().UpdateInventoryList();
			}
		}
		
		
		
		//Zoom Camera (BETA)
		if(Input.GetAxis("Mouse ScrollWheel") < 0)
		{
			//Camera.main.transform.localPosition += new Vector3(0.0f, -1.0f, -1.0f);
		}
		if(Input.GetAxis("Mouse ScrollWheel") > 0)
		{
			//Camera.main.fieldOfView -= 1;
		}
		
		//Mouse Control
		//Sign reading state
		if(PlayerState == 2)
		{
			if(Input.GetMouseButtonDown(0))
			{
				transform.GetComponent<GUIScript>().NextPage();
				print ("Moving on");
			}
		}
		//2D or 3D Active
		if(PlayerState == 5 || PlayerState == 3)
		{
			if(Application.loadedLevelName == "TestingArena")
			{
				Ray CheckingRay = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit CheckingInfo;
				if (Physics.Raycast(CheckingRay,out CheckingInfo, 100.0f))
				{
            		if(CheckingInfo.transform.name == "Chest001")
					{
						CheckingInfo.transform.GetComponent<ChestScript>().highlightChest();
					}
					if(CheckingInfo.transform.name == "Sign001")
					{
						CheckingInfo.transform.GetComponent<SignScript>().highlightSign();
					}
					if(CheckingInfo.transform.name == "Sign002")
					{
						CheckingInfo.transform.GetComponent<SignScript>().highlightSign();
					}
				}	
			}
			if(Input.GetMouseButtonDown(0))
			{
				bool foundInteraction = false;
				Ray CheckingRay = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit CheckingInfo;
				if (Physics.Raycast(CheckingRay,out CheckingInfo, 100.0f))
				{
            		if(CheckingInfo.transform.name == "Chest001")
					{
						print ("Found Chest");
						foundInteraction = true;
					}
					if(CheckingInfo.transform.name == "Sign001")
					{
						CheckingInfo.transform.GetComponent<SignScript>().activateSign();
						foundInteraction = true;
					}
					if(CheckingInfo.transform.name == "Sign002")
					{
						CheckingInfo.transform.GetComponent<SignScript>().activateSign();
						foundInteraction = true;
					}
					if(CheckingInfo.transform.name == "Switch001")
					{
						CheckingInfo.transform.animation.Play("Activate");
						foundInteraction = true;
					}
					if(CheckingInfo.transform.name == "LargeKey")
					{
						previousState = GetPlayerState();
						ChangePlayerState(4);
						
						transform.Find("You/YouAscendedArms").animation.CrossFade("PickUp", 0.1f);
						theGameCharacter.animation.Play("PickUp");
						//CheckingInfo.transform.GetComponent<LiftableObject>().toggleObject();
						foundInteraction = true;
					}
					
				}
				if(!foundInteraction && attackCycle == 0)
				{
					isAttacking = 1;
					attackCycle += 1;
					lastAttack = Time.time;
					transform.Find("You/YouAscendedArms").animation.CrossFade("Attack1", 0.1f);
				}
			}
		}
		//Pause Menu
		if(PlayerState == 1)
		{
			if(Input.GetMouseButtonDown(0))
			{
				Ray CheckingRay = inventoryPrefab.camera.ScreenPointToRay(Input.mousePosition);
				RaycastHit CheckingInfo;
				if (Physics.Raycast(CheckingRay,out CheckingInfo, 100.0f))
				{
					switch(CheckingInfo.transform.name)
					{
					case "Helmet1":
						PlayerPrefs.SetInt("HelmetID", 0);
						ChangeHelmet("None");
						print ("Found Helmet1");
						break;
					case "Helmet2(Clone)":
						PlayerPrefs.SetInt("HelmetID", 1);
						ChangeHelmet("Ascended");
						print ("Found Helmet2");
						break;
					case "Weapon1(Clone)":
						PlayerPrefs.SetInt("WeaponID", 1);
						ChangeWeapon("Hatchet");
						print ("Found Weapon1");
						break;
					case "Weapon2(Clone)":
						PlayerPrefs.SetInt("WeaponID", 2);
						ChangeWeapon("SheepAxe");
						print ("Found Weapon2");
						break;
					default:
						break;
					}
				}
			}
		}
		if(PlayerPrefs.GetInt("PlayerMoney", 0) > 999)
		{
			PlayerPrefs.SetInt("PlayerMoney", 999);
		}
		if(PlayerPrefs.GetInt("PlayerMoney", 0) < 0)
		{
			PlayerPrefs.SetInt("PlayerMoney", 0);
		}
		if(attackCycle > 0)
		{
			if(Time.time > (lastAttack + attackInterval))
			{
				attackCycle --;
			}
		}
		if(GetPlayerState() == 4)
		{
			if(pickupCycle == 30)
			{
				theGameCharacter.animation.Play("PickUp");
				theGameCharacter.animation["PickUp"].speed = -1.0F;
			}
			if(pickupCycle == 60)
			{
				resetPlayerState();
				pickupCycle = 0;
				theGameCharacter.animation["PickUp"].speed = 1.0F;
				if(isCarrying)
				{
					isCarrying = false;
				}
				else
				{
					isCarrying = true;
				}
			}
			pickupCycle++;
		}
    }
	
	//Interact with objects
	void OnControllerColliderHit(ControllerColliderHit hit) 
	{
		if(hit.transform.name == "Coin1" || hit.transform.name == "Coin1(Clone)")
		{
			Destroy(hit.gameObject);
			PlayerPrefs.SetInt("PlayerMoney", (PlayerPrefs.GetInt("PlayerMoney") + 1));
		}
        Rigidbody body = hit.collider.attachedRigidbody;
        if (body == null || body.isKinematic)
            return;
        
        if (hit.moveDirection.y < -0.3F)
            return;
        
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
        body.velocity = pushDir * pushPower;
    }
	
	void OnTriggerEnter(Collider other) 
	{
		//This is the structure that determines what happens when the player enters certain events.
        if(other.name == "Event1111")
		{
			CameraOffsetZ = 2.8f;
			CameraOffsetX = 0.0f;
			ChangePlayerState(5);
			ToggleCameraFollow(true);
			Destroy(other);
		}
		if(other.name == "Event1112")
		{
			//transform.position = new Vector3(-19.6f, -1.27f, 0.22f);
		}
		if(other.name == "Event1121")
		{
			//transform.position = new Vector3(-19.6f, -1.27f, 0.22f);
		}
		if(other.name == "Event1122")
		{
			CameraOffsetZ = 2.2f;
			CameraOffsetX = -3.0f;
			CameraRotationY = new Quaternion(0.1f, 0.9f, -0.3f, 0.4f);
			PlayerState = 0;
			Destroy(other);
		}
		
    }
	
	
	public void ToggleCameraFollow(bool SetState)
	{
		CameraFollow = SetState;
	}
	
	public void ToggleCameraFollow()
	{
		if(CameraFollow)
		{
			CameraFollow = false;
		}
		else
		{
			CameraFollow = true;
		}
	}
	
	public void ChangeHelmet(string TargetHelmet)
	{
		//Before we change helmet, we destroy the current one
		foreach(Transform child in transform.Find("You/Head"))
		{
			Destroy(child.gameObject);
		}
		//Apply nothing
		if(TargetHelmet == "None")
		{
		}
		//Apply the helm of the ascended
		if(TargetHelmet == "Ascended")
		{
			GameObject NewHelmet = new GameObject();
			NewHelmet = Instantiate(AscendedHelm, transform.FindChild("You/Head").position, transform.FindChild("You/Head").rotation) as GameObject;
			NewHelmet.transform.parent = transform.FindChild("You/Head"); //Make helm a child of the head, so it moves when the head does.
		}
			
	}
	
	public void ChangeWeapon(string TargetWeapon)
	{
		//Before we change helmet, we destroy the current one
		foreach(Transform child in transform.Find("You/YouAscendedArms/RShoulder/RArm"))
		{
			Destroy(child.gameObject);
		}
		//Apply nothing
		if(TargetWeapon == "None")
		{
		}
		//Apply the helm of the ascended
		if(TargetWeapon == "Hatchet")
		{
			GameObject NewWeapon = new GameObject();
			NewWeapon = Instantiate(Hatchet, transform.FindChild("You/YouAscendedArms/RShoulder/RArm").position + new Vector3(0.0f, -0.14f, 0.0f), transform.FindChild("You/YouAscendedArms/RShoulder/RArm").rotation) as GameObject;
			NewWeapon.transform.Rotate(new Vector3(0.0f, 180.0f, 180.0f), Space.Self);
			NewWeapon.transform.parent = transform.FindChild("You/YouAscendedArms/RShoulder/RArm");
			attackInterval = 0.5f;
		}
		if(TargetWeapon == "SheepAxe")
		{
			GameObject NewWeapon = new GameObject();
			NewWeapon = Instantiate(SheepAxe, transform.FindChild("You/YouAscendedArms/RShoulder/RArm").position + new Vector3(0.0f, -0.14f, 0.0f), transform.FindChild("You/YouAscendedArms/RShoulder/RArm").rotation) as GameObject;
			NewWeapon.transform.Rotate(new Vector3(90.0f, 0.0f, 0.0f), Space.Self);
			NewWeapon.transform.parent = transform.FindChild("You/YouAscendedArms/RShoulder/RArm");
			attackInterval = 1.5f;
		}
			
	}
	
	
	public int GetPlayerState()
	{
		return PlayerState;
	}
	
	public void ChangePlayerState(int StateModifier)
	{
		PlayerState = StateModifier;
		if(StateModifier == 5)
		{
			Screen.lockCursor = true;
		}
		else
		{
			Screen.lockCursor = false;
		}
	}
	
	public void resetPlayerState()
	{
		PlayerState = previousState;
	}
	
	
}
