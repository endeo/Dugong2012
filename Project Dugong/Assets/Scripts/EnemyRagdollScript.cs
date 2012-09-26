using UnityEngine;
using System.Collections;

public class EnemyRagdollScript : MonoBehaviour {
	
	int ragdollLifespan = 0;
	Vector3 hitLocation;
	public GameObject playerTarget;
	
	// Use this for initialization
	void Start () 
	{
		moveRagdoll();
	}
	
	// Update is called once per frame
	void Update () 
	{
		ragdollLifespan++;
		if(ragdollLifespan == 240)
		{
			Destroy(gameObject);
		}
	}
	
	void moveRagdoll()
	{
		
		hitLocation = playerTarget.transform.TransformDirection(playerTarget.transform.position);
		transform.Find("Body").rigidbody.AddForce(hitLocation * -200);
		transform.Find("Body/Head").rigidbody.AddRelativeForce(hitLocation * -100);
		int randomCoinDrop = Random.Range(1, 5);
		Debug.Log ("Spawning random coins: " + randomCoinDrop.ToString());
		for(int i = 0; i < randomCoinDrop; i++)
		{
			GameObject newCoin = Instantiate(Resources.Load("Props/Coin1"), transform.position, transform.rotation) as GameObject;
			newCoin.rigidbody.AddRelativeForce(hitLocation * -20); 
		}
	}
}
