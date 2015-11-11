using UnityEngine;
using System.Collections;

public class BigBird : MonoBehaviour 
{
	#region Variables

	// Protected Instance Variables
	protected Egg m_egg;
	protected bool m_moving = false;
	protected bool m_attacking = false;
	protected float m_speed = 10.0f;
	protected float m_lifeSpan = 5.0f;
	protected float m_lifeTimer;
	protected float m_damage = 20.0f;

	#endregion


	#region MonoBehaviour

	// Constructor 
	protected void Awake ()
	{
		m_egg = gameObject.GetComponentInChildren<Egg>();
	}

	// Update is called once per frame
	protected void Update () 
	{
		if ( m_moving == true )
		{
			transform.position += (-Vector3.right * m_speed * Time.deltaTime);
			
			if ( Time.time - m_lifeTimer >= m_lifeSpan )
			{
				Destroy ( gameObject );	
			}
		}
		
		if ( m_attacking == true )
		{
			if ( Mathf.Abs(Player.Instance.transform.position.x - transform.position.x) <= 10.0f )
			{
				m_egg.ReleaseEgg( m_speed );
				m_attacking = false;
			}
		}
	}
	
	//
	protected void OnTriggerEnter( Collider other )
	{
		if ( other.tag == "Player" )
		{
			Player.Instance.TakeDamage( m_damage );
		}
	}

	#endregion


	#region Public Functions
	
	//
	public void TakeDamage( float dam )
	{
		SoundManager.Instance.Play(AirmanLevelSounds.BOSS_HURTING);
		Destroy ( gameObject );
	}
	
	// 
	public void Attack()
	{
		m_lifeTimer = Time.time;
		m_moving = true;
		m_attacking = true;
	}
	
	#endregion
}
