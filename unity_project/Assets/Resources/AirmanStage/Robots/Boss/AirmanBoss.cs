using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AirmanBoss : MonoBehaviour 
{
	// Unity Editor Variables
	public Rigidbody m_deathParticle;
	public float m_deathParticleSpeed;
	public List<Material> m_materials;
	public float m_touchDamage;
	public Transform m_startMarker;
    public Transform m_endMarker;
	
	// Private Instance Variables
	private Player m_player;
	private Health m_health;
	private SoundManager m_soundManager;
	private AirmanWindWeapon m_weapon;
	private List<Transform> m_windShots = new List<Transform>();
	private bool m_playingBeginSequence = false;
	private bool m_fillHealthBar = false;
	private bool m_hasBeenIntroduced = false;
	private bool m_isFighting = false;	
	private int m_texIndex;
	private float m_texInterval = 0.1f;
	private float m_startFallTime;
	private float m_journeyLength;
	private float m_hurtingTimer;
	private Vector2 m_texScale = new Vector2(-1.0f, -1.0f);
	private Vector2 m_texScaleRight = new Vector2(1.0f, -1.0f);
	private Vector2 m_texScaleLeft = new Vector2(-1.0f, -1.0f);
	private Vector3 m_startingPosition;
	private Vector3 m_endPos;
	
	/* Constructor */
	void Awake ()
	{
		m_player = GameObject.Find("Player").GetComponent<Player>();
		m_health = gameObject.GetComponent<Health>();
		m_weapon = gameObject.GetComponent<AirmanWindWeapon>();
		m_soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
	}
	
	public void SetUpAirman()
	{
		animation["AirmanJumpToTheLeft"].speed = 1.65f;
		animation["AirmanJumpToTheRight"].speed = 1.65f;
		
		m_health.ShowHealthBar = true;
		m_health.HealthbarPosition = new Vector2( 30.0f, 10.0f );
		m_health.CurrentHealth = 0.0f;
		m_hasBeenIntroduced = true;
		
		// Stop the stage theme and play the boss theme
		m_soundManager.StopStageMusic();
		m_soundManager.PlayBossTheme();
		
		// Stop the player from shooting while healthbar is charging
		m_player.CanShoot = false;		
		
		// Activate the begin sequence and get the time, positions and length for it...
		m_startingPosition = m_startMarker.position;
		m_playingBeginSequence = true;
		
		m_startFallTime = Time.time;
		m_endPos = m_endMarker.position;
		m_journeyLength = Vector3.Distance(m_startingPosition, m_endPos);
	}
	
	
	/* Use this for initialization */
	void Start () 
	{
		SetUpAirman();
	}
	
	/**/
	void OnTriggerStay(Collider other) 
	{
		if ( other.tag == "Player" )
		{
			other.gameObject.SendMessage("TakeDamage", m_touchDamage );
		}
	}
	
	/**/
	void Reset()
	{
		if ( m_hasBeenIntroduced == false )
		{
			return;	
		}
		
		m_health.Reset();
		m_health.CurrentHealth = 0.0f;
		m_weapon.Reset();
		
		transform.position = m_startingPosition;
		m_playingBeginSequence = false;
		renderer.enabled = true;
		collider.enabled = true;
		m_health.IsDead = false;
		m_isFighting = false;
		m_fillHealthBar = false;
		m_texScale = m_texScaleLeft;
		
		GameObject.Find("BossBorder").gameObject.collider.enabled = true;
		GameObject.Find("BossDoor2").gameObject.SendMessage("Reset");
		GameObject.Find("BossDoorTrigger2").gameObject.collider.enabled = true;
		GameObject.Find("BossTrigger").gameObject.collider.enabled = true;
		m_player.IsExternalForceActive = false;
		
		foreach (Transform wind in m_windShots) {
			Destroy(wind.gameObject);
		}
		m_windShots.Clear();
		
		gameObject.SetActive ( false );
	}
	
	/**/
	void CreateDeathParticle( float speed, Vector3 pos, Vector3 vel )
	{
		Rigidbody particle = (Rigidbody) Instantiate(m_deathParticle, pos, transform.rotation);
		particle.transform.Rotate(90,0,0);
		particle.velocity =  vel * speed;
	}
	
	/**/
	IEnumerator CreateDeathParticles() {
		// Before the wait...
		Vector3 p1 = transform.position + Vector3.up;
		Vector3 p2 = transform.position - Vector3.up;
		Vector3 p3 = transform.position + Vector3.right;
		Vector3 p4 = transform.position - Vector3.right;
		
		Vector3 p5 = transform.position + Vector3.up + Vector3.right;
		Vector3 p6 = transform.position + Vector3.up - Vector3.right;
		Vector3 p7 = transform.position - Vector3.up - Vector3.right;
		Vector3 p8 = transform.position - Vector3.up + Vector3.right;
		
		p1.z = p2.z = -5;
		p3.z = p4.z = -7;
		p5.z = p6.z = p7.z = p8.z = -9;
		
		CreateDeathParticle( m_deathParticleSpeed, p1, ( transform.up) );
		CreateDeathParticle( m_deathParticleSpeed, p2, (-transform.up) );
		CreateDeathParticle( m_deathParticleSpeed, p3, ( transform.right) );
		CreateDeathParticle( m_deathParticleSpeed, p4, (-transform.right) );
		CreateDeathParticle( m_deathParticleSpeed, p5, ( transform.up + transform.right) );
		CreateDeathParticle( m_deathParticleSpeed, p6, ( transform.up - transform.right) );
		CreateDeathParticle( m_deathParticleSpeed, p7, (-transform.up - transform.right) );
		CreateDeathParticle( m_deathParticleSpeed, p8, (-transform.up + transform.right) );
		
		// Start the wait...
		yield return new WaitForSeconds(0.7f);
		
		// After the wait...
		CreateDeathParticle( m_deathParticleSpeed / 2.5f, p1, transform.up );
		CreateDeathParticle( m_deathParticleSpeed / 2.5f, p2,-transform.up );
		CreateDeathParticle( m_deathParticleSpeed / 2.5f, p3, transform.right );
		CreateDeathParticle( m_deathParticleSpeed / 2.5f, p4,-transform.right );
	}
	
	/**/
	IEnumerator PlayEndMusic() {
		// Before the wait...
		renderer.enabled = false;
		collider.enabled = false;
		m_soundManager.StopBossTheme();
		m_soundManager.PlayDeathSound();
		
		// Start the wait...
		yield return new WaitForSeconds(3.0f);
		
		// After the wait...
		m_soundManager.PlayStageFinishedSong();
		
		// Another wait...
		yield return new WaitForSeconds(6.5f);
		
		// Reload the level...
		m_player.IsPlayerInactive = true;
		m_player.PlayEndSequence();
		Destroy(gameObject);
	}
	
	/**/
	void KillRobot()
	{
		m_weapon.Reset();
		StartCoroutine(CreateDeathParticles());
		StartCoroutine(PlayEndMusic());
	}
	
	/**/
	void TakeDamage ( float dam )
	{
		// Make sure that shots can not kill the boss twice...
		if ( m_health.CurrentHealth > 0.0f && m_health.IsHurting == false )
		{
			m_soundManager.PlayBossHurtingSound();
			m_health.ChangeHealth ( - dam );
			m_hurtingTimer = Time.time;
			
			if ( m_health.CurrentHealth <= 0.0f )
			{
				KillRobot();	
			}
		}
	}
	
	/* Make the boss fall down, flex his muscles a little and fill his health bar */
	void PlayBeginSequence()
	{
		if ( m_fillHealthBar == true )
		{
			// Make the robot flex his muscles a little bit...
			int texIndex = (int) (Time.time / 0.1);
			renderer.material = m_materials[texIndex % 3];			
			renderer.material.SetTextureScale("_MainTex", m_texScale);
			
			// Fill up the health bar...
			if ( m_health.CurrentHealth < m_health.MaximumHealth )
			{
				m_health.CurrentHealth = ( m_health.CurrentHealth + 2.0f );
			}
			
			// If the health bar is full, make the robot start to fight!
			else
			{
				m_soundManager.StopHealthBarFillSound();
				m_playingBeginSequence = false;
				m_isFighting = true;
				m_player.CanShoot = true;
				GameObject.Find("BossBorder").gameObject.collider.enabled = false;
				m_weapon.Attack();
			}
		}
		
		// Make the boss fall down...
		else
		{
			float distCovered = (Time.time - m_startFallTime) * 10.0f;
	        float fracJourney = distCovered / m_journeyLength;
			transform.position = Vector3.Lerp(m_startingPosition, m_endPos, fracJourney);
			renderer.material.SetTextureScale("_MainTex", m_texScale);
			
			if ( fracJourney >= 1.0 )
			{
				m_fillHealthBar = true;
				m_soundManager.PlayHealthBarFillSound();
			}
		}
	}
	
	
	
	/**/	
	void AssignTexture()
	{
		if ( m_weapon.ShouldDisplayJumpingTex == true )
		{
			m_texIndex = (int) (Time.time / m_texInterval);
			renderer.material = m_materials[ (m_texIndex % 2) + 6];	
		}
		else if ( m_weapon.ShouldDisplayShootingTex == true )
		{
			renderer.material = m_materials[4];
		}
		else if ( m_weapon.ShouldDisplayBlowingTex == true )
		{
			m_texIndex = (int) (Time.time / m_texInterval);
			renderer.material = m_materials[ (m_texIndex % 2) + 2];	
		}
		else
		{
			renderer.material = m_materials[0];
		}
		
		if ( m_health.IsHurting == true )
		{
			renderer.material.color *= 0.75f + Random.value;
		}
		
		m_texScale = (m_weapon.IsTurningLeft == true) ? m_texScaleLeft : m_texScaleRight;
		renderer.material.SetTextureScale("_MainTex", m_texScale);
	}
	
	/* Update is called once per frame */
	void Update () 
	{
		if ( m_health.IsDead == true )
		{
			return;
		}
		else if ( m_playingBeginSequence == true )
		{
			PlayBeginSequence();
		}
		else if ( m_isFighting )
		{
			// Assign the appropriate texture...
			AssignTexture();
			
			if ( m_health.IsHurting == true )
			{
				if ( Time.time - m_hurtingTimer >= m_health.HurtingDelay )
				{
					m_health.IsHurting = false;	
				}
			}
		}
	}
}
