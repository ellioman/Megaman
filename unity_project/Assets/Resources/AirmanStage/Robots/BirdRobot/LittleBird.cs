using UnityEngine;
using System.Collections;

public class LittleBird : MonoBehaviour 
{
	// Private Instance Variables
	private Player m_player;
	private SoundManager m_soundManager;
	private bool m_attacking = false;
	private Vector3 m_direction;
	private float m_speed = 7.5f;
	private float m_lifeSpan = 10.0f;
	private float m_lifeTimer;
	private float m_damage = 10.0f;
	
	/* Constructor */
	void Awake ()
	{
		m_soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
		m_player = GameObject.Find("Player").GetComponent<Player>();
	}
	
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
		m_soundManager.PlayBossHurtingSound();
		Destroy ( gameObject );
	}
	
	/**/
	void OnTriggerEnter( Collider other )
	{
		if ( other.tag == "Player" )
		{
			m_player.TakeDamage( m_damage );
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
