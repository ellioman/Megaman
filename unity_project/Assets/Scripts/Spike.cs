using UnityEngine;
using System.Collections;

public class Spike : MonoBehaviour 
{
	// Private Instance Variables
	private float m_damage = 10.0f;
	
	/**/
	void InflictDamage( GameObject objectHit )
	{
		if ( objectHit.tag == "Player" )
		{
			Player.Instance.TakeDamage ( m_damage );
		}
	}
	
	/* */
	void OnCollisionStay( Collision collision ) 
	{
		InflictDamage( collision.gameObject );
	}
	
	/* */
	void OnTriggerStay(Collider other) 
	{
		InflictDamage( other.gameObject );
	}
}
