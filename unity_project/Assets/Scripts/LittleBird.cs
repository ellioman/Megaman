using UnityEngine;
using System.Collections;

public class LittleBird : MonoBehaviour 
{
	// Private Instance Variables
	private bool m_attacking = false;
	private Vector3 m_direction;
	private float m_speed = 7.5f;
	private float m_lifeSpan = 10.0f;
	private float m_lifeTimer;
	private float m_damage = 10.0f;
	
	/**/
	public void Attack( bool goLeft, float birdSpeed )
	{
		m_attacking = true;
		m_direction = (goLeft == true) ? Vector3.left + Vector3.up * 0.15f : Vector3.right + Vector3.up * 0.15f;
		m_speed = birdSpeed;
		m_lifeTimer = Time.time;
	}
	
	/**/
	public void TakeDamage( float dam )
	{
		SoundManager.Instance.Play(AirmanLevelSounds.BOSS_HURTING);
		Destroy ( gameObject );
	}
	
	// Called when the Collider other enters the trigger.
	protected void OnTriggerEnter( Collider other )
	{
		if ( other.tag == "Player" )
		{
			Player.Instance.TakeDamage( m_damage );
		}
	}
	
	/* Use this for initialization */
	void Start () {
		
	}
	
	/* */
	void Update () 
	{
		if ( m_attacking == true )
		{
			transform.position += (m_direction * m_speed * Time.deltaTime);
			
			if ( Time.time - m_lifeTimer >= m_lifeSpan )
			{
				Destroy ( gameObject );	
			}
		}
	}
}
