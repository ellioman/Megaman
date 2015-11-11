using UnityEngine;
using System.Collections;

public class Shot : MonoBehaviour 
{
	#region Variables

	// Properties
	public Vector3 VelocityDirection 	{ get; set; }
	public float ShotSpeed 				{ get; set; }
	
	// Protected Instance Variables
	protected float lifeSpan = 1.2f;
	protected int damage = 10;
	protected float timeStart;

	#endregion
	
	
	#region MonoBehaviour

	// Use this for initialization
	protected void Start () 
	{
		timeStart = Time.time;
	}

	// Update is called once per frame
	protected void Update () 
	{
		if (Time.time - timeStart >= lifeSpan)
		{
			Destroy(gameObject);
		}
		
		GetComponent<Rigidbody>().velocity = VelocityDirection * ShotSpeed;
	}

	// Called when the Collider other enters the trigger.
	protected void OnTriggerEnter(Collider other) 
	{
		if (other.tag == "shootable")
		{
			InflictDamage(other.gameObject);
		}
		
		else if (other.gameObject.layer == 10 && other.tag == "unshootable")
		{
			Destroy(gameObject);	
		}
		
		else if (other.gameObject.layer == 0 && other.tag == "unshootable")
		{
			Destroy(gameObject);
		}
		
		else if (other.gameObject.layer == 0 && other.tag == "platform")
		{
			Destroy(gameObject);
		}		
	}
	
	// 
	protected void OnCollisionEnter(Collision collision) 
	{
		if (collision.gameObject.tag == "shootable")
		{
			InflictDamage(collision.gameObject);
		}
		
		else if (collision.gameObject.layer == 10 && collision.gameObject.tag == "unshootable")
		{
			Destroy(gameObject);	
		}
		
		else if (collision.gameObject.layer == 0 && collision.gameObject.tag == "unshootable")
		{
			Destroy(gameObject);
		}
		
		else if (collision.gameObject.layer == 0 && collision.gameObject.tag == "platform")
		{
			Destroy(gameObject);
		}
	}

	#endregion


	#region Protected Functions

	// 
	protected void IncreaseLifeSpan(float increase)
	{
		lifeSpan += increase;	
	}
	
	// 
	protected void InflictDamage(GameObject enemy)
	{
		if (enemy.tag == "shootable")
		{
			enemy.SendMessage("TakeDamage", damage);
		}
		
		if (enemy.tag != "Player" && enemy.tag != "shot" && enemy.tag != "unshootable")
		{
			Destroy(gameObject);
		}
	}
	
	#endregion
}
