using UnityEngine;
using System.Collections;

public class BigBird : MonoBehaviour 
{
	// Private Instance Variables
	private Player m_player;
	private SoundManager m_soundManager;
	private Egg m_egg;
	private bool m_moving = false;
	private bool m_attacking = false;
	private float m_speed = 10.0f;
	private float m_lifeSpan = 5.0f;
	private float m_lifeTimer;
	private float m_damage = 20.0f;
	
	/* Constructor */
	void Awake ()
	{
		m_soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
		m_player = GameObject.Find("Player").GetComponent<Player>();
		m_egg = gameObject.GetComponentInChildren<Egg>();
	}
	
	/* Use this for initialization */
	void Start () 
	{
		
	}
	
	/**/
	void OnTriggerEnter( Collider other )
	{
		if ( other.tag == "Player" )
		{
			m_player.TakeDamage( m_damage );
		}
	}
	
	/**/
	public void TakeDamage( float dam )
	{
		m_soundManager.PlayBossHurtingSound();
		Destroy ( gameObject );
	}
	
	/**/
	public void Reset()
	{
		
	}
	
	/* */
	public void Attack()
	{
		m_lifeTimer = Time.time;
		m_moving = true;
		m_attacking = true;
	}
	
	/* Update is called once per frame */
	void Update () 
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
			if ( Mathf.Abs(m_player.transform.position.x - transform.position.x) <= 10.0f )
			{
				m_egg.ReleaseEgg( m_speed );
				m_attacking = false;
			}
		}
	}
}
