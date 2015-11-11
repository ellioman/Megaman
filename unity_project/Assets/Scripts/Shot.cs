using UnityEngine;
using System.Collections;

public class Shot : MonoBehaviour 
{
	// Properties
	public Vector3 VelocityDirection 	{ get; set; }
	public float ShotSpeed 				{ get; set; }
	
	// Private Instance Variables
	private float m_lifeSpan = 1.2f;
	private int m_damage = 10;
	private float m_timeStart;
	
	// Called when the Collider other enters the trigger.
	protected void OnTriggerEnter(Collider other) 
	{
		if ( other.tag == "shootable" )
		{
			InflictDamage( other.gameObject );
		}
		
		else if ( other.gameObject.layer == 10 && other.tag == "unshootable" )
		{
			Destroy( gameObject );	
		}
		
		else if ( other.gameObject.layer == 0 && other.tag == "unshootable" )
		{
			Destroy( gameObject );
		}
		
		else if ( other.gameObject.layer == 0 && other.tag == "platform" )
		{
			Destroy( gameObject );
		}		
	}
	
	/**/
	void OnCollisionEnter( Collision collision ) 
	{
		if ( collision.gameObject.tag == "shootable" )
		{
			InflictDamage( collision.gameObject );
		}
		
		else if ( collision.gameObject.layer == 10 && collision.gameObject.tag == "unshootable" )
		{
			Destroy( gameObject );	
		}
		
		else if ( collision.gameObject.layer == 0 && collision.gameObject.tag == "unshootable" )
		{
			Destroy( gameObject );
		}
		
		else if ( collision.gameObject.layer == 0 && collision.gameObject.tag == "platform" )
		{
			Destroy( gameObject );
		}
	}
	
	/* Use this for initialization */
	void Start () 
	{
		m_timeStart = Time.time;
	}
	
	/**/
	void IncreaseLifeSpan( float increase )
	{
		m_lifeSpan += increase;	
	}
	
	/**/
	void InflictDamage( GameObject enemy )
	{
		if (enemy.tag == "shootable")
		{
			enemy.SendMessage("TakeDamage", m_damage );
		}
		
		if ( enemy.tag != "Player" && enemy.tag != "shot" && enemy.tag != "unshootable" )
		{
			Destroy(gameObject);
		}
	}
	
	/* Update is called once per frame */
	void Update () 
	{
		if ( Time.time - m_timeStart >= m_lifeSpan )
		{
			Destroy(gameObject);
		}
		
		GetComponent<Rigidbody>().velocity = VelocityDirection * ShotSpeed;
	}
}
