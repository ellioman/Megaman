using UnityEngine;
using System.Collections;

public class DeathTrigger : MonoBehaviour 
{	
	/*
	 * Kill/Respawn the player when he enters the trigger.
	 * 
	*/
	void OnTriggerEnter(Collider other) 
	{
		if ( other.tag == "Player" )
		{
			other.gameObject.GetComponent<Player>().KillPlayer();
			gameObject.collider.enabled = false;
		}
    }
}
