using UnityEngine;
using System.Collections;

public class ElectricFloorRobotShot : MonoBehaviour 
{
	// Private Instance Variables
	private int m_texDir = 1;
	private bool m_keepAttacking = false;
	private float m_texChangeDelay = 0.1f;
	private float m_texTimer;
	private float m_gravity = 11.8f;
	private float m_jumpAmount = 10.0f;
	private float m_verticalVelocity;
	private float m_lifeSpan = 5.0f;
	private float m_lifeTimer;
	private Vector3 m_attackPos;
	private Vector3 m_moveVector;
	
	/* Use this for initialization */
	void Start () {
		m_lifeTimer = m_texTimer = Time.time;
	}
	
	/**/
	public void Attack( Vector3 playerPos )
	{
		m_attackPos = playerPos;
		m_moveVector = (m_attackPos - transform.position);
		m_moveVector.y = m_jumpAmount;
		m_verticalVelocity = m_jumpAmount;
		m_keepAttacking = true;
	}
	
	/**/
	void OnCollisionEnter( Collision collision ) 
	{
		if ( collision.gameObject.tag == "Player" )
		{
			collision.gameObject.GetComponent<Player>().TakeDamage( 10f );
			Destroy(gameObject);
		}
		
		if ( collision.transform.position.y < transform.position.y )
		{
			m_keepAttacking = false;
		}
	}
	
	/**/
	void OnTriggerEnter( Collider collider ) 
	{
		if ( collider.tag == "Player" )
		{
			collider.gameObject.GetComponent<Player>().TakeDamage( 10f );
			Destroy(gameObject);
		}
		
		if ( collider.transform.position.y < transform.position.y )
		{
//			keepAttacking = false;
		}
	}
	
	/**/
	private void ApplyGravity()
	{
//		if(moveVector.y > -terminalVelocity)
		{
			m_moveVector = new Vector3(m_moveVector.x, (m_moveVector.y - m_gravity * Time.deltaTime), m_moveVector.z);
		}
	}
	
	/* Update is called once per frame */
	void Update () 
	{
		if ( m_keepAttacking == true )
		{
			m_verticalVelocity = m_moveVector.y;
			m_moveVector = (m_attackPos - transform.position);
			m_moveVector.y = m_verticalVelocity;
			
			ApplyGravity();
			
			transform.position += m_moveVector * Time.deltaTime;
		}
		
		if ( Time.time - m_texTimer >= m_texChangeDelay )
		{
			m_texTimer = Time.time;
			m_texDir *= -1;
			renderer.material.SetTextureScale("_MainTex", new Vector2( m_texDir, 1) );
		}
		
		// Time to kill the shot?
		if ( Time.time - m_lifeTimer >= m_lifeSpan )
		{
			Destroy(gameObject);
		}
	}
}
