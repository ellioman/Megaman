using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WindRobot : MonoBehaviour 
{
	// Unity Editor Variables
	public List<Material> mats;
	
	// Private Instance Variables
	private bool m_armsUp = false;
	private bool m_weaponActivated = false;
	private bool m_isTurningLeft = true;
	private bool m_isDead = false;
	private int m_damage = 30;
	private int m_health = 30;
	private int m_texIndex;
	private int m_currentHealth;
	private float m_windRange = 12.0f;
	private float m_windPower = 250.0f;
	private float m_distanceToPlayer;
	private float m_texChangeTimer;
	private float m_texArmsUpInterval = 0.1f;
	private float m_texArmsDownInterval = 0.1f;
	private Vector2 m_texScale = new Vector2(-1.0f, -1.0f);
	private Vector2 m_texScaleRight = new Vector2(1.0f, -1.0f);
	private Vector2 m_texScaleLeft = new Vector2(-1.0f, -1.0f);
	private Vector3 m_windDirection = new Vector3(-1.0f, 0f, 0f);
	
	
	/**/
	public void Reset()
	{
		m_isDead = false;
		GetComponent<Renderer>().enabled = true;
		GetComponent<Collider>().enabled = true;
		m_currentHealth = m_health;
	}
	
	/**/
	void KillRobot()
	{
		m_isDead = true;
		GetComponent<Renderer>().enabled = false;
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
		SoundManager.Instance.Play(AirmanLevelSounds.BOSS_HURTING);
		m_currentHealth -= damageTaken;
		
		if ( m_currentHealth <= 0 )
		{
			TurnWindWeaponOff();
			KillRobot();
		}
	}
	
	/**/
	void SendWindInfoToPlayer()
	{
		Player.Instance.ExternalForce = m_windDirection * m_windPower;
	}
	
	/* Turn on the wind weapon */
	void TurnWindWeaponOn()
	{
		m_weaponActivated = true;
		Player.Instance.IsExternalForceActive = true;
		SendWindInfoToPlayer();
	}
	
	/* Turn off the wind weapon */
	void TurnWindWeaponOff()
	{
		Player.Instance.IsExternalForceActive = false;
		m_weaponActivated = false;
	}
	
	/**/
	void MakeRobotTurnLeft()
	{
		m_isTurningLeft = true;
		m_texScale = m_texScaleLeft;
		m_windDirection *= -1;
		SendWindInfoToPlayer();
	}
	
	/**/
	void MakeRobotTurnRight()
	{
		m_isTurningLeft = false;
		m_texScale = m_texScaleRight;
		m_windDirection *= -1;
		SendWindInfoToPlayer();
	}
	
	/* Use this for initialization */
	void Start () 
	{
		m_texChangeTimer = Time.time;
		m_currentHealth = m_health;
	}
	
	/*  Three textures are used to simulate animation on the robot */
	void AssignTexture()
	{
		if ( m_armsUp == true )
		{
			m_texIndex = (int) (Time.time / m_texArmsUpInterval);
			GetComponent<Renderer>().material = mats[(m_texIndex % 2) + 2 ];
			
			if ( Time.time - m_texChangeTimer >= 0.35f )
			{
				m_texChangeTimer = Time.time;
				m_armsUp = !m_armsUp;
			}
		}
		else
		{
			m_texIndex = (int) (Time.time / m_texArmsDownInterval);
			GetComponent<Renderer>().material = mats[(m_texIndex % 2) ];
			
			if ( Time.time - m_texChangeTimer >= 1.99f )
			{
				m_texChangeTimer = Time.time;
				m_armsUp = !m_armsUp;
			}
		}
		
		GetComponent<Renderer>().material.SetTextureScale("_MainTex", m_texScale);
	}
	
	/* Update is called once per frame */
	void Update () 
	{
		if ( m_isDead == false )
		{
			m_distanceToPlayer = Player.Instance.transform.position.x - transform.position.x;
			
			// Make sure the robot is facing the player...
			if ( m_isTurningLeft == false && m_distanceToPlayer <= 0.0f )
			{
				MakeRobotTurnLeft();
			}
			else if ( m_isTurningLeft == true && m_distanceToPlayer > 0.0f )
			{
				MakeRobotTurnRight();
			}
			
			// Turn on / off the weapon if the player is in range...
			if ( Mathf.Abs(m_distanceToPlayer) <= m_windRange )
			{
				if ( m_weaponActivated == false )
				{
					TurnWindWeaponOn();
				}
			}
			else if ( m_weaponActivated == true )
			{
				TurnWindWeaponOff();
			}
			
			// Put a texture on the robot...
			AssignTexture();
		}
	}
}
