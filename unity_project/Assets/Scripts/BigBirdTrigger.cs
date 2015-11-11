using UnityEngine;
using System.Collections;

public class BigBirdTrigger : MonoBehaviour 
{
	#region MonoBehaviour

	// Called when the Collider other enters the trigger.
	protected void OnTriggerEnter( Collider other )
	{
		if (other.tag == "Player")
		{
			transform.parent.gameObject.GetComponent<BigBird>().Attack();
			gameObject.GetComponent<Collider>().enabled = false;
		}
	}

	#endregion
}
