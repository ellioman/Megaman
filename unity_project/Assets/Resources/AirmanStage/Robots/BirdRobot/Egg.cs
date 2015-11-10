using UnityEngine;
using System.Collections;

public class Egg : MonoBehaviour 
{
	// Unity Editor Variables
	public Rigidbody littleBird;
	
	// Private Instance Variables
	private Player m_player;
	private SoundManager m_soundManager;
	private bool m_falling = false;
	private float m_speed = 7.0f;
	private float m_lifeSpan = 5.0f;
	private float m_lifeTimer;
	private float m_xVel = 0.0f;
	private float m_velSlower = 7.0f;
	private float m_damage = 10.0f;	
	
	/* Constructor */
	void Awake ()
	{
		m_soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
		m_player = GameObject.Find("Player").GetComponent<Player>();
	}
	
	/* Use this for initialization */
	void Start () {
		
	}
	
	void CreateBird( Vector3 pos, bool goLeft )
	{
		Rigidbody littleBirdRobot = (Rigidbody) Instantiate(littleBird, pos, transform.rotation);
		littleBirdRobot.GetComponent<LittleBird>().Attack( goLeft, 7.0f + Random.Range(0.0f, 1.0f) );
		Physics.IgnoreCollision(littleBirdRobot.GetComponent<Collider>(), GetComponent<Collider>());
	}
	
	/**/
	void OnTriggerEnter( Collider other )
	{
		if ( other.tag == "Player" )
		{
			m_player.TakeDamage( m_damage );
		}
		
		// If we are crashing into a platform...
		else if ( other.tag == "platform" )
		{
			float dist = 1.25f;
			bool goLeft = (m_player.transform.position.x < transform.position.x);
			CreateBird( transform.position, goLeft );
			CreateBird( transform.position + Vector3.up, goLeft);
			CreateBird( transform.position + Vector3.down, goLeft);
			CreateBird( transform.position + Vector3.left, goLeft);
			CreateBird( transform.position + Vector3.right, goLeft);
			
			CreateBird( transform.position + Vector3.up * dist + Vector3.left, goLeft);
			CreateBird( transform.position + Vector3.up * dist + Vector3.right, goLeft);
			CreateBird( transform.position + Vector3.down * dist + Vector3.left, goLeft);
			CreateBird( transform.position + Vector3.down * dist + Vector3.right, goLeft);
			
			CreateBird( transform.position + Vector3.up * (dist/2.0f) + Vector3.left * (dist/2.0f), goLeft);
			CreateBird( transform.position + Vector3.up * (dist/2.0f) + Vector3.right * (dist/2.0f), goLeft);
			CreateBird( transform.position + Vector3.down * (dist/2.0f) + Vector3.left * (dist/2.0f), goLeft);
			CreateBird( transform.position + Vector3.down * (dist/2.0f) + Vector3.right * (dist/2.0f), goLeft);
			
			CreateBird( transform.position + Vector3.up * (dist/3.0f) + Vector3.left * (dist/3.0f), goLeft);
			CreateBird( transform.position + Vector3.up * (dist/3.0f) + Vector3.right * (dist/3.0f), goLeft);
			CreateBird( transform.position + Vector3.down * (dist/3.0f) + Vector3.left * (dist/3.0f), goLeft);
			CreateBird( transform.position + Vector3.down * (dist/3.0f) + Vector3.right * (dist/3.0f), goLeft);
			
			Destroy( gameObject );
		}
	}
	
	/**/
	public void TakeDamage( float dam )
	{
		m_soundManager.PlayBossHurtingSound();
		Destroy ( gameObject );
	}
	
	/**/
	public void ReleaseEgg( float xVelocity )
	{
		m_xVel = xVelocity;
		transform.parent = null;
		m_falling = true;
		m_lifeTimer = Time.time;
		tag = "shootable";
		gameObject.layer = 0;
	}
	
	/* */
	void Update () 
	{
		if ( m_falling == true )
		{
			transform.position += ((Vector3.down * m_speed) + (Vector3.left * m_xVel) )  * Time.deltaTime;
			if ( m_xVel > 0.0f ) m_xVel -= m_velSlower * Time.deltaTime;
			if ( m_xVel < 0.0f ) m_xVel = 0.0f;
			
			if ( Time.time - m_lifeTimer >= m_lifeSpan )
			{
				Destroy( gameObject );	
			}
		}
	}
}
