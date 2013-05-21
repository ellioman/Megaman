using UnityEngine;
using System.Collections;

public class BigBirdTrigger : MonoBehaviour 
{	
	/**/
	void OnTriggerEnter( Collider other )
	{
		if ( other.tag == "Player" )
		{
			transform.parent.gameObject.GetComponent<BigBird>().Attack();
			gameObject.collider.enabled = false;
		}
	}
}
