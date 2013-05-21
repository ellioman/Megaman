using UnityEngine;
using System.Collections;
using System.Collections.Generic;
	
public class SmallFlyingRobot : MonoBehaviour
{
	// Unity Editor Variables
	public List<Material> m_materials;
	
	// Private Instance Variables
	private Player m_player;
	private SoundManager m_soundManager;
	private bool m_attack = false;
	private int m_damage = 10;
	private int m_health = 10;
	private int m_texIndex;
	private float m_robotSpeed = 35;
	private float m_attackDelay = 0.7f;
	private float m_attackDelayTimer;
	private float m_distanceToDisappear = 32.0f;
	private float m_texChangeInterval = 0.2f;
	
	/* The Constructor */
	void Awake () 
	{
		m_soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
		m_player = GameObject.Find("Player").GetComponent<Player>();
	}
	
	/* Use this for initialization */
	void Start ()
	{
		m_attackDelayTimer = Time.time;
	}
	
	/**/
	void KillRobot()
	{
		transform.parent.gameObject.GetComponent<RedHornBeast>().MinusRobotCount();
		Destroy(gameObject);
	}
	
	/**/
	public void Reset()
	{
		KillRobot();	
	}
	
	/**/
	void OnTriggerStay(Collider other) 
	{
		if ( other.tag == "Player" )
		{
			m_player.TakeDamage( m_damage );
		}
	}
	
	/**/
	void OnCollisionStay( Collision collision ) 
	{
		if ( collision.gameObject.tag == "Player" )
		{
			m_player.TakeDamage( m_damage );
		}
	}
	
	/**/
	void TakeDamage( int damageTaken )
	{
		m_soundManager.PlayBossHurtingSound();
		m_health -= damageTaken;
		if ( m_health <= 0 )
		{
			KillRobot();
		}
	}
	
	/* Update is called once per frame */
	void Update ()
	{
		if ( m_attack == false )
		{
			if ( Time.time - m_attackDelayTimer >= m_attackDelay)
			{
				m_attack = true;
			}
		}
		else
		{
			Vector3 direction = m_player.transform.position - transform.position;
			
			// Kill this object if the player is too far away
			if ( direction.magnitude >= m_distanceToDisappear )
			{
				KillRobot();
			}
			else
			{
				direction.Normalize();
				rigidbody.velocity = direction * (Time.deltaTime * m_robotSpeed);
			}
		}
		
		// Update the textures...
		m_texIndex = (int) (Time.time / m_texChangeInterval);
		renderer.material = m_materials[m_texIndex % m_materials.Count];
	}
}

