using UnityEngine;
using System.Collections;

public class ElectricFloorRobot : MonoBehaviour 
{
	// Unity Editor Variables
	public Rigidbody m_shot;
	
	// Private Instance Variables
	private float m_distanceToStop = 14.0f;
	private float m_attackDelay = 2.0f;
	private float m_attackTimer;

	/* Use this for initialization */
	void Start () 
	{
		m_attackTimer = Time.time;
	}
	
	/**/
	public void Reset()
	{
		KillChildren();
	}
	
	/**/
	void KillChildren()
	{
		// Destroy all the shots...
		foreach(Transform child in transform)
		{
		    Destroy( child.gameObject );
		}
	}
	
	/* */
	void Attack()
	{
		if ( Time.time - m_attackTimer >= m_attackDelay )
		{
			m_attackTimer = Time.time;
			
			Vector3 pos = transform.position + Vector3.up * 0.8f + Vector3.right * 0.1f;
			Rigidbody electricShot = (Rigidbody) Instantiate(m_shot, pos, transform.rotation);
			Physics.IgnoreCollision(electricShot.GetComponent<Collider>(), GetComponent<Collider>());
			electricShot.GetComponent<ElectricFloorRobotShot>().Attack( Player.Instance.transform.position );
			electricShot.transform.parent = gameObject.transform;
		}
	}
	
	/* Update is called once per frame */
	void Update () 
	{
		Vector3 direction = Player.Instance.transform.position - transform.position;
		
		// Kill this object if the player is too far away
		if ( direction.magnitude <=  m_distanceToStop)
		{
			Attack();
		}
	}
}
