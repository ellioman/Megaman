using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AirmanWindWeapon : MonoBehaviour
{
	// Unity Editor Variables
	public Transform m_windWeapon;
	
	// Properties
	public bool IsTurningLeft {get; set;}
	public bool ShouldDisplayShootingTex {get; set;}
	public bool ShouldDisplayJumpingTex  {get; set;}
	public bool ShouldDisplayBlowingTex  {get; set;}
	
	// Private Instance Variables
	private Player m_player;
	private List<Transform> m_windShots = new List<Transform>();
	private bool m_isBlowing;
	private bool m_isJumping;
	private bool m_isShooting;
	private bool m_isFighting;
	private bool m_isPlayingAnimation;
	private bool m_shouldDestroyWind = false;
	private int m_howManyShotsBeforeJumping = 3;
	private int m_shootingCount = 0;
	private float m_fightingTimer;
	private float m_blowDelay = 2.0f;	
	private float m_windDestroyDelay = 1.0f;
	private float m_windPower = 400.0f;
	private List<Vector3> m_nextAttack;
	private List<Vector3> m_attack1Right = new List<Vector3>();
	private List<Vector3> m_attack2Right = new List<Vector3>();
	private List<Vector3> m_attack3Right = new List<Vector3>();
	private List<Vector3> m_attack1Left = new List<Vector3>();
	private List<Vector3> m_attack2Left = new List<Vector3>();
	private List<Vector3> m_attack3Left = new List<Vector3>();
	
	/**/
	private void InitAttackLists()
	{
		// Create the attack 1 list...
		m_attack1Left.Add( new Vector3( -20.0f, 7.0f, 0.0f ) );
		m_attack1Left.Add( new Vector3( -16.0f, 2.0f, 0.0f ) );
		m_attack1Left.Add( new Vector3( -14.0f, 5.0f, 0.0f ) );
		m_attack1Left.Add( new Vector3( -9.0f, 0.0f, 0.0f ) );
		m_attack1Left.Add( new Vector3( -6.0f, 8.0f, 0.0f ) );
		m_attack1Left.Add( new Vector3( -3.0f, 4.0f, 0.0f ) );
		
		m_attack1Right.Add( new Vector3( 20.0f, 7.0f, 0.0f ) );
		m_attack1Right.Add( new Vector3( 16.0f, 2.0f, 0.0f ) );
		m_attack1Right.Add( new Vector3( 14.0f, 5.0f, 0.0f ) );
		m_attack1Right.Add( new Vector3( 9.0f, 0.0f, 0.0f ) );
		m_attack1Right.Add( new Vector3( 6.0f, 8.0f, 0.0f ) );
		m_attack1Right.Add( new Vector3( 3.0f, 4.0f, 0.0f ) );
		
		// Create the attack 2 list...
		m_attack2Left.Add( new Vector3( -18.0f, 6.0f, 0.0f ) );
		m_attack2Left.Add( new Vector3( -16.0f, 0.0f, 0.0f ) );
		m_attack2Left.Add( new Vector3( -12.0f, 12.0f, 0.0f ) );
		m_attack2Left.Add( new Vector3( -10.0f, 6.5f, 0.0f ) );
		m_attack2Left.Add( new Vector3( -9.5f, 2.5f, 0.0f ) );
		m_attack2Left.Add( new Vector3( -3.0f, 0.4f, 0.0f ) );
		
		m_attack2Right.Add( new Vector3( 18.0f, 6.0f, 0.0f ) );
		m_attack2Right.Add( new Vector3( 16.0f, 0.0f, 0.0f ) );
		m_attack2Right.Add( new Vector3( 12.0f, 12.0f, 0.0f ) );
		m_attack2Right.Add( new Vector3( 10.0f, 6.5f, 0.0f ) );
		m_attack2Right.Add( new Vector3( 9.5f, 2.5f, 0.0f ) );
		m_attack2Right.Add( new Vector3( 3.0f, 0.4f, 0.0f ) );
		
		// Create the attack 3 list...
		m_attack3Left.Add( new Vector3( -17.0f, 2.0f, 0.0f ) );
		m_attack3Left.Add( new Vector3( -15.5f, 5.5f, 0.0f ) );
		m_attack3Left.Add( new Vector3( -12.0f, 0.0f, 0.0f ) );
		m_attack3Left.Add( new Vector3( -10.0f, 2.5f, 0.0f ) );
		m_attack3Left.Add( new Vector3( -4.5f, 2.4f, 0.0f ) );
		m_attack3Left.Add( new Vector3( -3.5f, 4.5f, 0.0f ) );
		
		m_attack3Right.Add( new Vector3( 17.0f, 2.0f, 0.0f ) );
		m_attack3Right.Add( new Vector3( 15.5f, 5.5f, 0.0f ) );
		m_attack3Right.Add( new Vector3( 12.0f, 0.0f, 0.0f ) );
		m_attack3Right.Add( new Vector3( 10.0f, 2.5f, 0.0f ) );
		m_attack3Right.Add( new Vector3( 4.5f, 2.4f, 0.0f ) );
		m_attack3Right.Add( new Vector3( 3.5f, 4.5f, 0.0f ) );
	}
	
	/* Constructor */
	void Awake ()
	{
		m_player = GameObject.Find("Player").GetComponent<Player>();
//		soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
	}
	
	/* Use this for initialization */
	void Start ()
	{
		InitAttackLists();
		m_nextAttack = m_attack1Left;
		
		animation.Stop();
		m_shootingCount = 0;
		
		IsTurningLeft = true;
		m_isBlowing = false;
		m_isJumping = false;
		m_isShooting = false;
		m_isFighting = false;
		m_isPlayingAnimation = false;
		m_shouldDestroyWind = false;
		ShouldDisplayShootingTex = false;
		ShouldDisplayJumpingTex = false;
		ShouldDisplayBlowingTex = false;
	}
	
	public void Reset()
	{
		animation.Stop();
		m_shootingCount = 0;
		m_nextAttack = m_attack1Left;
		IsTurningLeft = true;
		m_isFighting = false;
		m_isShooting = false;
		m_isBlowing = false;
		m_isJumping = false;
		m_isPlayingAnimation = false;
		m_shouldDestroyWind = false;
		ShouldDisplayShootingTex = false;
		ShouldDisplayJumpingTex = false;
		ShouldDisplayBlowingTex = false;
		
		m_player.IsExternalForceActive = false;
		foreach (Transform wind in m_windShots) {
			Destroy(wind.gameObject);
		}
		m_windShots.Clear();
	}
	
	/**/
	private void CreateWindShot( Vector3 pos )
	{
		Transform wind = (Transform) Instantiate(m_windWeapon, transform.position, transform.rotation);
		wind.SendMessage("SetPosition", transform.position + pos);
		wind.transform.parent = gameObject.transform;
		m_windShots.Add(wind);
	}
	
	/**/
	void Blow()
	{
		if ( m_shouldDestroyWind == true )
		{
			if ( Time.time - m_fightingTimer >= m_windDestroyDelay )
			{
				// Destroy the wind gameobjects and clear the list
				foreach (Transform wind in m_windShots) {
					Destroy(wind.gameObject);
				}
				m_windShots.Clear();
				
				m_player.IsExternalForceActive = false;
				m_isBlowing = false;
				ShouldDisplayBlowingTex = false;
				m_isShooting = true;
				m_shouldDestroyWind = false;
				m_fightingTimer = Time.time;	
			}
			else
			{
				if ( IsTurningLeft == true )
				{
					m_player.ExternalForce = -Vector3.right * m_windPower;
				}
				else if ( IsTurningLeft == false )
				{
					m_player.ExternalForce = Vector3.right * m_windPower;
				}
			}
		}
		else
		{
			if ( Time.time - m_fightingTimer >= m_blowDelay )
			{
				// Make the wind go away!
				foreach (Transform wind in m_windShots) {
					wind.SendMessage("GoAway");
				}
				
				m_player.IsExternalForceActive = true;
				ShouldDisplayShootingTex = false;
				ShouldDisplayBlowingTex = true;
				m_shouldDestroyWind = true;
				m_fightingTimer = Time.time;
			}
		}
	}
	
	/**/
	private void Shoot()
	{
		// If the boss has shot three times...
		if ( m_shootingCount == m_howManyShotsBeforeJumping )
		{
			// Make him jump to the other side and attack again...
			m_isShooting = false;
			m_isJumping = true;
			m_shootingCount = 0;
		}
		else 
		{
			// Create the wind...
			foreach (Vector3 wind in m_nextAttack ) 
			{
				CreateWindShot( wind );
			}
			
			// Set up the next attack
			if ( IsTurningLeft == true )
			{
				if ( m_shootingCount == 0 ) m_nextAttack = m_attack2Left;
				else if ( m_shootingCount == 1 ) m_nextAttack = m_attack3Left;
				else if ( m_shootingCount == 2 ) m_nextAttack = m_attack1Right;
			}
			else if ( IsTurningLeft == false )
			{
				if ( m_shootingCount == 0 ) m_nextAttack = m_attack2Right;
				else if ( m_shootingCount == 1 ) m_nextAttack = m_attack3Right;
				else if ( m_shootingCount == 2 ) m_nextAttack = m_attack1Left;
			}
			
			// Set the appropriate variables
			ShouldDisplayShootingTex = true;
			m_isBlowing = true;
			m_isShooting = false;
			m_fightingTimer = Time.time;
			m_shootingCount++;
		}
	}
	
	/**/
	void Jump()
	{
		if ( m_isPlayingAnimation == false )
		{
			if ( IsTurningLeft == true )
			{
				animation.Play("AirmanJumpToTheLeft");
			}
			else
			{
				animation.Play("AirmanJumpToTheRight");
			}
			
			ShouldDisplayJumpingTex = true;
			m_isPlayingAnimation = true;			
		}
		
		// If we're done playing the animation...
		if ( animation.IsPlaying("AirmanJumpToTheLeft") == false && animation.IsPlaying("AirmanJumpToTheRight") == false )
		{
			m_isPlayingAnimation = false;
			ShouldDisplayJumpingTex = false;
			IsTurningLeft = !IsTurningLeft;
			m_isShooting = true;
			m_isJumping = false;
		}
	}
	
	/**/
	public void Attack()
	{
		m_isFighting = true;
		m_isShooting = true;
	}
	
	/* Update is called once per frame */
	void Update ()
	{
		if ( m_isFighting == true )
		{
			if ( m_isShooting == true )
			{
				Shoot();
			}
			else if ( m_isBlowing )
			{
				Blow();	
			}
			else if ( m_isJumping == true )
			{
				Jump();	
			}
		}
	}
}

