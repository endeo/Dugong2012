using UnityEngine;
using System.Collections;

/// <summary>
/// Goblin A.I.
/// Logic for the Enemy Type "Goblin"
/// Coded by Martin and Sean Aug 2012
/// </summary>

public class GoblinAI : MonoBehaviour {
	
	//GOBLIN STATE STORAGE:
	int currentState;
	//0 - Dormant
	//1 - Alarmed
	//2 - Chasing Player
	//3 - Searching for the Player
	//4 - Attacking Mode
	
	//Arrow Variables:
	int arrowsLeft;
	int arrowDelay;
	
	float goblinHealth = 100;
	
	public GameObject goblinRagdoll;
	
	//Character Controller:
	CharacterController controller = new CharacterController();
	
	//Movement related Variables
	static float moveSpeed;
	
	//The current player targetted by the Goblin:
	static GameObject targetPlayer;
	
	//Variables for A.I. Navigation:
	float playerRange;
	float closeToLastLocation;
	Vector3 lastKnownLocation;
	
	// Initialisation of variables and objects:
	void Start () 
	{
		moveSpeed = 0.85f;
		currentState = 0;
		targetPlayer = GameObject.Find("You");
		controller = GetComponent<CharacterController>();
		arrowsLeft = 10;
	}
	
	// Update is called once per frame:
	void Update () 
	{
		//Calculate how the far the targetted player is from the Goblin:
		playerRange = Vector3.Distance(transform.position, targetPlayer.transform.position);
		
		/////////////////////////////////////
		//                                 //
		//          DORMANT MODE!          //
		//                                 //
		/////////////////////////////////////
		
		//While the Goblin isn't chasing or searching for the player it
		//goes into a dormant state where it checks if the player is close
		//enough. It then 'Hears' the player and is alarmed.
		
		if ((currentState != 2)) //I'm not chasing the player...
		{
			if((currentState != 3)) //...nor am I looking for him...
			{
				//So I'll stay where I am and chill out until...
				if(playerRange < 9.0f) //I CAN HEAR THE PLAYER!
				{
				//I'M ALARMED BY HIS NOISES!
				currentState = 1;
				}
				if(playerRange > 13.0f) //I CANNOT HEAR THE PLAYER!
				{
				//I'M NOT ALARMED BY ANY NOISES!
				currentState = 0;
				}
			}
		}
		

		/////////////////////////////////////
		//                                 //
		//           ALARMED MODE          //
		//                                 //
		/////////////////////////////////////
		
		if(currentState == 1)
		{
			//IF THE PLAYER IS SEEN BY THE GOBLIN (Using a Linecast...)
			//This could be improved by adding an angle of view, but I've not added that in yet:
			RaycastHit playerSensor;
			if(Physics.Linecast (transform.FindChild("Head").transform.position,targetPlayer.transform.position,out playerSensor))
			{
				if (playerSensor.collider.gameObject.name == "YouObject")
				{
					//Start Chasing
					currentState = 2;
				}
			}
		}
		
		/////////////////////////////////////
		//                                 //
		//          CHASING MODE           //
		//                                 //
		/////////////////////////////////////
		
		if(currentState == 2)
		{
			
			if(Vector3.Distance(transform.position,lastKnownLocation) < 1.0f)
			{
				moveSpeed = 0.3f;
			}
			else
			{
				moveSpeed = 0.85f;
			}
			
			//LOOK AT THE PLAYER!
			Quaternion targetRotation = Quaternion.LookRotation(targetPlayer.transform.position - transform.position);
			targetRotation = new Quaternion(0,targetRotation.y,0,targetRotation.w);
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.1f);
			transform.FindChild("Head").transform.rotation = Quaternion.Slerp(transform.FindChild("Head").transform.rotation, targetRotation, 0.12f);
			
			//CHECK THE PLAYER CAN STILL SEE THE GOBLIN (Using a Linecast...)
			//This could be improved by adding an angle of view, but I've not added that in yet
			RaycastHit playerSensor;
			if(Physics.Linecast (transform.FindChild("Head").transform.position,targetPlayer.transform.position,out playerSensor))
			{
				if (playerSensor.collider.gameObject.name == "YouObject")
				{
					//The goblin remembers where the player was, so he can follow the
					//player around corners. Goblins may smell, but they're not that stupid.
					lastKnownLocation = targetPlayer.transform.position;
				}
				else
				{
					//If the player has avoid the goblin, it'll go into search mode!
					currentState = 3;
				}
			}
			
			
			//If I have no arrows left or the player is very close...
			if((arrowsLeft <= 0) | (Vector3.Distance(transform.position,lastKnownLocation) < 4.0f))
			{
			//Charge the player
			var delta = lastKnownLocation - transform.position;
      		delta.Normalize();
			controller.SimpleMove(delta * moveSpeed);
			}
			else
			{
			//Shoot the player
				arrowDelay++;
				if(arrowDelay >= 100)
				{
				GameObject newArrow = Instantiate(Resources.Load("Props/Arrow1"), new Vector3 (transform.position.x,transform.position.y + 1f,transform.position.z), transform.rotation) as GameObject;
				var delta = lastKnownLocation - transform.position;
      			delta.Normalize();
				newArrow.transform.rigidbody.AddForce (delta * 600f);
				arrowsLeft --;
				arrowDelay = 0;
				}
			}
		}
		
		/////////////////////////////////////
		//                                 //
		//         SEARCHING MODE          //
		//                                 //
		/////////////////////////////////////
		
		if(currentState == 3)
		{
			
			//Move to the last location the Goblin saw the player:
			var delta = lastKnownLocation - transform.position;
      		delta.Normalize();
			controller.SimpleMove(delta * moveSpeed);
			
			//Keep an eye out for the player's whereabouts:
			RaycastHit playerSensor;
			if(Physics.Linecast (transform.FindChild("Head").transform.position,targetPlayer.transform.position,out playerSensor))
			{
				if (playerSensor.collider.gameObject.name == "YouObject")
				{
					//Start chasing him again:
					currentState = 2;
				}
			}
			
			//If the goblin gets to the location where he last saw the player and has
			//had no luck finding him, he goes back to dormant mode. (0.35f is roughly
			//the float value of the distance when the goblin stops moving.)
			closeToLastLocation = Vector3.Distance(transform.position,lastKnownLocation);
			//Debug.Log(closeToLastLocation);
			if (closeToLastLocation < 0.35f)
			{
			//Lost him... better go back to chilling out!
			currentState = 0;	
			}	
		}
		
		
		if(goblinHealth < 0.1)
		{
			killThis();
		}
		
	}
	
	void OnTriggerEnter(Collider other)
	{
		if(other.transform.name == "SheepAxe(Clone)")
		{
			goblinHealth -= 100;
		}
	}
	
	void killThis() //This kills the Goblin.
	{
		Instantiate (goblinRagdoll, transform.position, transform.rotation);
		Destroy(gameObject);
	}
	
	
	
	
}
