using UnityEngine;
using System.Collections;

public class Spike : MonoBehaviour 
{
	// Private Instance Variables
	private Player m_player;
	private float m_damage = 10.0f;
	
	/**/
	void InflictDamage( GameObject objectHit )
	{
		if ( objectHit.tag == "Player" )
		{
			m_player.TakeDamage ( m_damage );
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
	
	/* Use this for initialization */
	void Start () 
	{
		m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
	}
	
	/* Update is called once per frame */
	void Update () {
	
	}
}
