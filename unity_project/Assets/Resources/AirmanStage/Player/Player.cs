using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
	// Unity Editor Variables
	public Rigidbody m_deathParticle;
	public List<Material> m_playerMaterials;
	
	// Properties
	public bool IsPlayerInactive 		{ get; set; }
	public bool IsFrozen 				{ get { return m_movement.IsFrozen; } 				set { m_movement.IsFrozen = value; }				}
	public bool IsExternalForceActive 	{ get { return m_movement.IsExternalForceActive; }	set { m_movement.IsExternalForceActive = value; } 	}
	public bool IsDead 					{ get { return m_health.IsDead; } 					set { m_health.IsDead = value; }					}
	public bool CanShoot 				{ get { return m_shooting.CanShoot; } 				set { m_shooting.CanShoot = value; }				}
	public float CurrentHealth 			{ get { return m_health.CurrentHealth; } 			set { m_health.CurrentHealth = value; } 			}
	public Vector3 ExternalForce 		{ get { return m_movement.ExternalForce; } 			set { m_movement.ExternalForce = value; } 			}
	public Vector3 CheckpointPosition 	{ get { return m_movement.CheckPointPosition; } 	set { m_movement.CheckPointPosition = value; } 		}
	
	// Private Instance Variables
	private LevelCamera m_camera;
	private SoundManager m_soundManager;
	private Movement m_movement;
	private Shooting m_shooting;
	private Health m_health;
	private GameObject m_playerTexture;
	private int m_walkingTexIndex;
	private int m_standingTexIndex;
	private float m_walkingTexInterval = 0.2f;
	private float m_standingTexInterval = 0.3f;
	private Vector2 m_texScaleLeft = new Vector2(1.0f, -1.0f);
	private Vector2 m_texScaleRight = new Vector2(-1.0f, -1.0f);
	
	/* Constructor */
	void Awake ()
	{
		m_soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
		m_camera = GameObject.Find("Camera").GetComponent<LevelCamera>();
		m_movement = gameObject.GetComponent<Movement>();
		m_shooting = gameObject.GetComponent<Shooting>();
		m_health = gameObject.GetComponent<Health>();
		m_playerTexture = transform.FindChild ("PlayerTexture").gameObject;
	}
	
	/* Use this for initialization */
	void Start ()
	{
		IsPlayerInactive = false;
		m_health.HealthbarPosition = new Vector2(10,10);
		m_health.ShowHealthBar = true;
	}
	
	/**/
	void Reset() 
	{
		m_health.Reset();
		m_movement.Reset();
		m_shooting.Reset();
		m_playerTexture.renderer.enabled = true;
		IsPlayerInactive = false;
	}
	
	/**/
	void CreateDeathParticle( float speed, Vector3 pos, Vector3 vel )
	{
		Rigidbody particle = (Rigidbody) Instantiate(m_deathParticle, pos, transform.rotation);
		Physics.IgnoreCollision(particle.collider, collider);
		particle.transform.Rotate(90,0,0);
		particle.velocity =  vel * speed;
	}
	
	/**/
	IEnumerator CreateDeathParticles( Vector3 pos ) 
	{
		float deathParticleSpeed = 6.0f;
		
		// Before the wait...
		Vector3 p1 = pos + Vector3.up;
		Vector3 p2 = pos - Vector3.up;
		Vector3 p3 = pos + Vector3.right;
		Vector3 p4 = pos - Vector3.right;
		
		Vector3 p5 = pos + Vector3.up + Vector3.right;
		Vector3 p6 = pos + Vector3.up - Vector3.right;
		Vector3 p7 = pos - Vector3.up - Vector3.right;
		Vector3 p8 = pos - Vector3.up + Vector3.right;
		
		p1.z = p2.z = -5;
		p3.z = p4.z = -7;
		p5.z = p6.z = p7.z = p8.z = -9;
		
		this.CreateDeathParticle( deathParticleSpeed, p1, ( transform.up) );
		this.CreateDeathParticle( deathParticleSpeed, p2, (-transform.up) );
		this.CreateDeathParticle( deathParticleSpeed, p3, ( transform.right) );
		this.CreateDeathParticle( deathParticleSpeed, p4, (-transform.right) );
		this.CreateDeathParticle( deathParticleSpeed, p5, ( transform.up + transform.right) );
		this.CreateDeathParticle( deathParticleSpeed, p6, ( transform.up - transform.right) );
		this.CreateDeathParticle( deathParticleSpeed, p7, (-transform.up - transform.right) );
		this.CreateDeathParticle( deathParticleSpeed, p8, (-transform.up + transform.right) );
		
		// Start the wait...
		yield return new WaitForSeconds(0.7f);
		
		// After the wait...
		this.CreateDeathParticle( deathParticleSpeed / 2.5f, p1, transform.up );
		this.CreateDeathParticle( deathParticleSpeed / 2.5f, p2,-transform.up );
		this.CreateDeathParticle( deathParticleSpeed / 2.5f, p3, transform.right );
		this.CreateDeathParticle( deathParticleSpeed / 2.5f, p4,-transform.right );
	}
	
	/**/
	private IEnumerator MovePlayerUp()
	{
		while (true)
		{
			transform.position += Vector3.up * 35.0f * Time.deltaTime;
			yield return null;
		}
	}
	
	/**/
	private IEnumerator MakeThePlayerLeaveStage()
	{
		m_playerTexture.renderer.material = m_playerMaterials[14];
		yield return new WaitForSeconds(0.05f);
		
		m_playerTexture.renderer.material = m_playerMaterials[15];
		yield return new WaitForSeconds(0.05f);
		
		m_soundManager.PlayMegamanLeavesStageSound();
		m_playerTexture.renderer.material = m_playerMaterials[16];
		m_playerTexture.transform.localScale = new Vector3( 0.04f, 1.0f, 0.2f );
		StartCoroutine( "MovePlayerUp" );
		
		yield return new WaitForSeconds(3.0f);
		
		StopCoroutine ( "MovePlayerUp" );
		IsPlayerInactive = false;
		Application.LoadLevel (0);
	}
	
	/* */
	public void PlayEndSequence()
	{
		StartCoroutine( MakeThePlayerLeaveStage() );
	}
	
	/**/
	GameObject[] FindGameObjectsWithLayer (int layer) 
	{
		GameObject[] goArray = FindObjectsOfType(typeof(GameObject)) as GameObject[];
		List<GameObject> goList = new List<GameObject>();
		for (int i = 0; i < goArray.Length; i++) 
		{
			if (goArray[i].layer == layer) 
			{
				goList.Add(goArray[i]);
			}
		}
		if (goList.Count == 0) {
		   return null;
		}
		return goList.ToArray();
	}
	
	
	/**/
	IEnumerator WaitAndReset() {
		/* Before the wait... */
		m_health.IsDead = true;
		m_movement.IsFrozen = true;
		m_playerTexture.renderer.enabled = false;
		m_camera.ShouldStayStill = true;
		m_shooting.CanShoot = false;
		
		
		CharacterController cc = (CharacterController) GetComponent(typeof(CharacterController));
		cc.detectCollisions = false;
		
		m_soundManager.StopStageMusic();
		m_soundManager.StopBossTheme();
		m_soundManager.PlayDeathSound();
		
		/* Start the wait... */
		yield return new WaitForSeconds(3.6f);
		
		/* After the wait... */
		
		// Reset the camera
		m_camera.Reset();
		
		// Reset the player
		Reset();
		cc.detectCollisions = true;		
		
		if ( GameObject.Find("Airman") != null ) 
		{
			GameObject.Find("Airman").SendMessage("Reset");
		}
		
		// Play the music again...
		m_soundManager.SendMessage("PlayStageMusic");
		m_camera.ShouldStayStill = false;
		
		// Reset all the enemy bots...
		int enemyRobotsLayer = 10;
		GameObject[] enemyRobots = FindGameObjectsWithLayer( enemyRobotsLayer );
		foreach (GameObject robot in enemyRobots)
		{
			robot.SendMessage("Reset");
		}
		
		// Reset the birdtriggers...
		GameObject[] birdTriggers = GameObject.FindGameObjectsWithTag( "birdTrigger" );
		foreach( GameObject trigger in birdTriggers )
		{
			trigger.collider.enabled = true;	
		}
		
		/* Start another wait to avoid double deaths by the hand of deathtriggers... */
		yield return new WaitForSeconds(0.3f);
		
		// Reset the deathtriggers...
		GameObject[] triggers = GameObject.FindGameObjectsWithTag( "deathTrigger" );
		foreach( GameObject trigger in triggers )
		{
			trigger.collider.enabled = true;	
		}
	}
	
	
	/*
	 */
	public void KillPlayer() 
	{
		StartCoroutine(CreateDeathParticles( transform.position ));
		StartCoroutine(WaitAndReset());
	}
	
	/* */
	public void TakeDamage( float damage )
	{
		// If the player isn't already hurting or dead...
		if ( m_health.IsHurting == false && m_health.IsDead == false )
		{
			m_soundManager.PlayHurtingSound();
			m_health.ChangeHealth( -damage );
			m_movement.IsHurting = true;
			
			if ( m_health.IsDead == true )
			{
				KillPlayer();
			}
		}
	}
	
	/**/	
	void AssignTexture()
	{
		if ( m_health.IsHurting == true && m_health.IsDead == false )
		{
			m_playerTexture.transform.localScale = new Vector3( 0.1175f, 1.0f, 0.1175f );
			m_playerTexture.renderer.material = m_playerMaterials[7];
			m_playerTexture.renderer.material.color *= 0.75f + Random.value;
		}
		else if ( m_movement.IsJumping == true )
		{
			if ( m_shooting.IsShooting == true )
			{
				m_playerTexture.transform.localScale = new Vector3( 0.1175f, 1.0f, 0.1175f );
				m_playerTexture.renderer.material = m_playerMaterials[9];
			}
			else
			{
				m_playerTexture.transform.localScale = new Vector3( 0.1175f, 1.0f, 0.1175f );
				m_playerTexture.renderer.material = m_playerMaterials[2];
			}
		}
		else if ( m_movement.IsWalking == true )
		{
			m_walkingTexIndex = (int) (Time.time / m_walkingTexInterval);
			
			if ( m_shooting.IsShooting == true )
			{
				m_playerTexture.transform.localScale = new Vector3( 0.13f, 1.0f, 0.1f );
				m_playerTexture.renderer.material = m_playerMaterials[ (m_walkingTexIndex % 4) + 10];
			}
			else
			{
				m_playerTexture.transform.localScale = new Vector3( 0.1f, 1.0f, 0.1f );
				m_playerTexture.renderer.material = m_playerMaterials[ (m_walkingTexIndex % 4) + 3];
			}
		}
		// Standing...
		else 
		{	
			if ( m_shooting.IsShooting == true )
			{
				m_playerTexture.transform.localScale = new Vector3( 0.128f, 1.0f, 0.1f );
				m_playerTexture.renderer.material = m_playerMaterials[8];
			}
			else
			{
				m_playerTexture.transform.localScale = new Vector3( 0.1f, 1.0f, 0.1f );
				m_standingTexIndex = (int) (Time.time / m_standingTexInterval);
				m_playerTexture.renderer.material = ( m_standingTexIndex % 10 == 0 ) ? m_playerMaterials[1] : m_playerMaterials[0];
			}
		}
		
		m_playerTexture.renderer.material.SetTextureScale("_MainTex", (m_movement.IsTurningLeft) ? m_texScaleLeft : m_texScaleRight  );
	}
	
	
	/* Update is called once per frame */
	void Update ()
	{
		if ( IsPlayerInactive == false )
		{
			// Handle the horizontal and Vertical movements
			m_movement.HandleMovement();
			
			// Handle shooting
			if( (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.LeftShift)) && m_shooting.CanShoot == true )
	    	{
	        	m_shooting.Shoot( m_movement.IsTurningLeft );
				m_soundManager.PlayShootingSound();
	    	}
			
			// Handle health...
			if ( m_health.IsHurting )
			{
				if ( Time.time - m_health.HurtingTimer >= m_health.HurtingDelay )
				{
					m_movement.IsHurting = false;
					m_health.IsHurting = false;	
				}
			}
		
			// Assign the appropriate texture to the player...
			AssignTexture();
			
			if ( transform.position.z != 0 )
			{
				Vector3 pos = transform.position;
				pos.z = 0;
				transform.position = pos;
			}
		}
	}
}