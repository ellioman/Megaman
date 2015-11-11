using UnityEngine;
using System.Collections;

public class DeathTrigger : MonoBehaviour 
{	
	#region MonoBehaviour

	// Kill/Respawn the player when he enters the trigger.
	protected void OnTriggerEnter(Collider other) 
	{
		if (other.tag == "Player")
		{
			GameEngine.Player.KillPlayer();
			gameObject.GetComponent<Collider>().enabled = false;
		}
    }

	#endregion
}
