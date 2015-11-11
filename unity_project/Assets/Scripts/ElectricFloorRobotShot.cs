using UnityEngine;
using System.Collections;

public class ElectricFloorRobotShot : MonoBehaviour 
{
	#region Variables

	// Protected Instance Variables
	protected int texDir = 1;
	protected bool keepAttacking = false;
	protected float texChangeDelay = 0.1f;
	protected float texTimer;
	protected float gravity = 11.8f;
	protected float jumpAmount = 10.0f;
	protected float verticalVelocity;
	protected float lifeSpan = 5.0f;
	protected float lifeTimer;
	protected Vector3 attackPos;
	protected Vector3 moveVector;
	
	#endregion
	
	
	#region MonoBehaviour

	// Use this for initialization
	protected void Start ()
	{
		lifeTimer = texTimer = Time.time;
	}
	
	// Update is called once per frame
	protected void Update () 
	{
		if (keepAttacking == true)
		{
			verticalVelocity = moveVector.y;
			moveVector = (attackPos - transform.position);
			moveVector.y = verticalVelocity;
			
			ApplyGravity();
			
			transform.position += moveVector * Time.deltaTime;
		}
		
		if (Time.time - texTimer >= texChangeDelay)
		{
			texTimer = Time.time;
			texDir *= -1;
			GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(texDir, 1));
		}
		
		// Time to kill the shot?
		if (Time.time - lifeTimer >= lifeSpan)
		{
			Destroy(gameObject);
		}
	}

	// Called when the Collider other enters the trigger.
	protected void OnTriggerEnter(Collider collider) 
	{
		if (collider.tag == "Player")
		{
			collider.gameObject.GetComponent<Player>().TakeDamage(10f);
			Destroy(gameObject);
		}
	}

	// 
	protected void OnCollisionEnter(Collision collision) 
	{
		if (collision.gameObject.tag == "Player")
		{
			collision.gameObject.GetComponent<Player>().TakeDamage(10f);
			Destroy(gameObject);
		}
		
		if (collision.transform.position.y < transform.position.y)
		{
			keepAttacking = false;
		}
	}
	
	#endregion
	
	
	#region Protected Functions
	
	// 
	protected void ApplyGravity()
	{
		moveVector = new Vector3(moveVector.x, (moveVector.y - gravity * Time.deltaTime), moveVector.z);
	}

	#endregion
	
	
	#region Public Functions

	// 
	public void Attack(Vector3 playerPos)
	{
		attackPos = playerPos;
		moveVector = (attackPos - transform.position);
		moveVector.y = jumpAmount;
		verticalVelocity = jumpAmount;
		keepAttacking = true;
	}

	#endregion
}
