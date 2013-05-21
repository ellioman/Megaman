using UnityEngine;
using System.Collections;

public class BossDoor : MonoBehaviour 
{
	// Private Instance Variables
	private SoundManager m_soundManager;
	private Player m_player;
	private float m_playerSpeed;
	private float m_doorSpeed;
	private bool m_opening = false;
	private bool m_closing = false;
	private bool m_playerGoneThrough = false;
	private Vector3 m_initialScale;
	
	/**/
	void Awake()
	{
		m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		m_soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
	}
	
	/* Use this for initialization */
	void Start () {
		m_initialScale = transform.localScale;
		m_playerSpeed = 250.0f;
		m_doorSpeed = 10.0f;
	}
	
	/**/
	void Reset()
	{
		m_opening = false;
		m_closing = false;
		m_playerGoneThrough = false;
	}
	
	/**/
	void scaleCube(float scaleAmount)
	{
		Vector3 sc = transform.localScale;
		Vector3 pos = transform.localPosition;
		
		//Scale the object on X axis in local units
		sc.y += scaleAmount;
		transform.localScale = sc;
 
		//Move the object on X axis in local units
		pos.y -= scaleAmount/1.3f; 
		transform.localPosition = pos;
	}
	
	/**/
	void openDoor()
	{
		m_soundManager.PlayBossDoorSound();
		gameObject.collider.isTrigger = true;
		m_opening = true;
	}
	
	/**/
	void closeDoor()
	{
		m_soundManager.PlayBossDoorSound();
		gameObject.collider.isTrigger = false;
		m_closing = true;
	}	
	
	/* Update is called once per frame */
	void Update () 
	{
		if ( m_playerGoneThrough == true )
		{
			return;
		}
		
		if ( m_opening )
		{
			scaleCube( -m_doorSpeed * Time.deltaTime );
			
			if ( transform.localScale.y <= 0.5 )
			{
				m_opening = false;
				m_player.IsExternalForceActive = true;
				m_player.ExternalForce = new Vector3 (m_playerSpeed, 0.0f, 0.0f);
				m_soundManager.StopBossDoorSound();
			}
		}
		
		else if ( m_closing )
		{
			scaleCube( m_doorSpeed * Time.deltaTime );
			
			if ( transform.localScale.y >= m_initialScale.y )
			{
				m_closing = false;
				m_playerGoneThrough = true;
				m_player.IsExternalForceActive = false;
				m_soundManager.StopBossDoorSound();
			}
		}
	
	}
}
