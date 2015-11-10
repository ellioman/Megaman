using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ElectricRobot : MonoBehaviour 
{
	// Unity Editor Variables
	public Rigidbody m_electricShot;
	public List<Material> m_materials;
	
	// Private Instance Variables
	private Transform player;
	private GameObject soundManager;
	private CirclingPlatform platform;
	private BoxCollider m_robotCollider;
	private bool m_isShooting = false;
	private bool m_isDead = false;
	private int m_health = 30;
	private int m_currentHealth;
	private int m_damage = 10;
	private int m_texIndex = 0;
	private float m_texChangeInterval = 0.1f;
	private float m_distanceToStop = 32.0f;
	private float m_shootingRangeDiameter = 10f;
	private float m_shootAgainDelay = 2f;
	private float m_shootingTimer;
	private Vector2 m_texScale;
	private Vector2 m_texScaleRight = new Vector2(1.0f, -1.0f);
	private Vector2 m_texScaleLeft = new Vector2(-1.0f, -1.0f);
	private Vector3 m_turningLeftColliderPos = new Vector3(0.2f, -0.8f, 0f );
	private Vector3 m_turningRightColliderPos = new Vector3(-0.2f, -0.8f, 0f );
	
	/**/
	public void SetIsShooting( bool status )
	{
		m_isShooting = status;
	}
	
	/**/
	public void Reset()
	{
		m_isDead = false;
		GetComponent<Collider>().enabled = true;
		m_currentHealth = m_health;
	}
	
	/**/
	void KillRobot()
	{
		m_isDead = true;
		GetComponent<Collider>().enabled = false;
	}
	
	/**/
	void OnTriggerStay(Collider other) 
	{
		if ( other.tag == "Player" )
		{
			other.gameObject.SendMessage("TakeDamage", m_damage );
		}
	}
	
	/* Make the robot take damage */
	void TakeDamage( int damageTaken )
	{
		soundManager.SendMessage("PlayBossHurtingSound");
		m_currentHealth -= damageTaken;
		if ( m_currentHealth <= 0 )
		{
			KillRobot();
		}
	}
	
	/* Use this for initialization */
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player").transform;
		soundManager = GameObject.Find("SoundManager").gameObject;
		platform = gameObject.transform.parent.GetComponent<CirclingPlatform>();
		m_robotCollider = (BoxCollider) transform.parent.FindChild("PlatformBoxCollider").gameObject.GetComponent<Collider>();
		m_texScale = m_texScaleLeft;
		m_currentHealth = m_health;
	}
		
	/**/
	void AssignTexture()
	{
		m_texIndex = (int) (Time.time / m_texChangeInterval);
		
		// Make the robot always face the player...
		bool playerOnLeftSide = (player.position.x - transform.position.x < -1.0f);
		
		// If the robot is dead
		if ( m_isDead == true )
		{
			// display the platform textures...
			GetComponent<Renderer>().material = m_materials[(m_texIndex % 2) + 6 ];
			GetComponent<Renderer>().material.SetTextureScale("_MainTex", m_texScaleLeft);
			m_robotCollider.center = m_turningLeftColliderPos;
		}
		
		// If the robot is shooting...
		else if ( m_isShooting == true )
		{
			GetComponent<Renderer>().material = m_materials[(m_texIndex % 2) + 4 ];
			m_texScale = ( playerOnLeftSide == true) ? m_texScaleLeft : m_texScaleRight;
			GetComponent<Renderer>().material.SetTextureScale("_MainTex", m_texScale);
			m_robotCollider.center = (playerOnLeftSide == true) ? m_turningLeftColliderPos : m_turningRightColliderPos;
		}
		else
		{
			// Assign the material
			GetComponent<Renderer>().material = m_materials[m_texIndex % 4];
			m_texScale = ( playerOnLeftSide == true) ? m_texScaleLeft : m_texScaleRight;
			GetComponent<Renderer>().material.SetTextureScale("_MainTex", m_texScale);
			m_robotCollider.center = (playerOnLeftSide == true) ? m_turningLeftColliderPos : m_turningRightColliderPos;
		}
	}
	
	/* Shoot an electric arrow towards the player */	
	void Shoot()
	{
		m_isShooting = true;
		m_shootingTimer = Time.time;
		
		Rigidbody shot = (Rigidbody) Instantiate(m_electricShot, transform.position, transform.rotation);
		shot.transform.parent = gameObject.transform;
		Physics.IgnoreCollision(shot.GetComponent<Collider>(), GetComponent<Collider>());
		
		shot.GetComponent<ElectricShot>().TargetDirection = player.position;
	}
	
	/**/
	void CheckIfRobotCanShoot()
	{
		// If the robot is alive, check if he is in range to shoot the player
		if ( m_isDead == false )
		{
			// If the robot is in shooting range...
			float distanceToPlayer = (player.position - transform.position).magnitude;
			if ( distanceToPlayer < m_shootingRangeDiameter )
			{
				// If the robot is ready to shoot...
				if ( m_isShooting == false && Time.time - m_shootingTimer >= m_shootAgainDelay )
				{
					Shoot ();
				}
			}
		}
	}
	
	/* Update is called once per frame */
	void Update () 
	{
		// Stop fighting if the player is too far away
		if ( (player.transform.position - transform.position).magnitude >= m_distanceToStop )
		{
			platform.ShouldAnimate = false;
		}
		else
		{
			platform.ShouldAnimate = true;
			
			// Check if the robot can shoot the player
			CheckIfRobotCanShoot();
			
			// Put the correct texture on the robot
			AssignTexture();
		}
	}	
}
