using UnityEngine;
using System.Collections;

public class DeathTrigger : MonoBehaviour 
{	
	// Kill/Respawn the player when he enters the trigger.
	protected void OnTriggerEnter(Collider other) 
	{
		if ( other.tag == "Player" )
		{
			other.gameObject.GetComponent<Player>().KillPlayer();
			gameObject.GetComponent<Collider>().enabled = false;
		}
    }
}
