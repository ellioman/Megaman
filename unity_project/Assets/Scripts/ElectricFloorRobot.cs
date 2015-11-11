using UnityEngine;
using System.Collections;

public class ElectricFloorRobot : MonoBehaviour 
{
	#region Variables

	// Unity Editor Variables
	[SerializeField] protected Rigidbody shotPrefab;
	
	// Protected Instance Variables
	protected float distanceToStop = 14.0f;
	protected float attackDelay = 2.0f;
	protected float attackTimer;

	#endregion


	#region MonoBehaviour

	// Use this for initialization
	protected void Start () 
	{
		attackTimer = Time.time;
	}

	// Update is called once per frame
	protected void Update () 
	{
		Vector3 direction = GameEngine.Player.transform.position - transform.position;
		
		// Kill this object if the player is too far away
		if ( direction.magnitude <=  distanceToStop)
		{
			Attack();
		}
	}

	#endregion

	#region Protected Functions

	// 
	protected void KillChildren()
	{
		// Destroy all the shots...
		foreach(Transform child in transform)
		{
			Destroy( child.gameObject );
		}
	}
	
	// 
	protected void Attack()
	{
		if ( Time.time - attackTimer >= attackDelay )
		{
			attackTimer = Time.time;
			
			Vector3 pos = transform.position + Vector3.up * 0.8f + Vector3.right * 0.1f;
			Rigidbody electricShot = (Rigidbody) Instantiate(shotPrefab, pos, transform.rotation);
			Physics.IgnoreCollision(electricShot.GetComponent<Collider>(), GetComponent<Collider>());
			electricShot.GetComponent<ElectricFloorRobotShot>().Attack( GameEngine.Player.transform.position );
			electricShot.transform.parent = gameObject.transform;
		}
	}

	#endregion

	#region Public Functions
	
	// 
	public void Reset()
	{
		KillChildren();
	}
	
	#endregion
}
