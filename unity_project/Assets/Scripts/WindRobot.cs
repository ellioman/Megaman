using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;
using System.Collections.Generic;

public class WindRobot : MonoBehaviour 
{
	#region Variables

	// Unity Editor Variables
	public List<Material> animationMaterials;
	
	// Protected Instance Variables
	protected bool armsUp = false;
	protected bool weaponActivated = false;
	protected bool isTurningLeft = true;
	protected bool isDead = false;
	protected int damage = 30;
	protected int health = 30;
	protected int texIndex;
	protected int currentHealth;
	protected float windRange = 12.0f;
	protected float windPower = 250.0f;
	protected float distanceToPlayer;
	protected float texChangeTimer;
	protected float texArmsUpInterval = 0.1f;
	protected float texArmsDownInterval = 0.1f;
	protected Vector2 texScale = new Vector2(-1.0f, -1.0f);
	protected Vector2 texScaleRight = new Vector2(1.0f, -1.0f);
	protected Vector2 texScaleLeft = new Vector2(-1.0f, -1.0f);
	protected Vector3 windDirection = new Vector3(-1.0f, 0f, 0f);

	#endregion


	#region MonoBehaviour

	// Constructor
	protected void Awake() 
	{
		Assert.IsTrue(animationMaterials.Count == 4);
	}

	// Use this for initialization
	protected void Start() 
	{
		texChangeTimer = Time.time;
		currentHealth = health;
	}

	// Update is called once per frame
	protected void Update() 
	{
		if (isDead == false)
		{
			distanceToPlayer = GameEngine.Player.transform.position.x - transform.position.x;
			
			// Make sure the robot is facing the player...
			if (isTurningLeft == false && distanceToPlayer <= 0.0f)
			{
				MakeRobotTurnLeft();
			}
			else if (isTurningLeft == true && distanceToPlayer > 0.0f)
			{
				MakeRobotTurnRight();
			}
			
			// Turn on / off the weapon if the player is in range...
			if (Mathf.Abs(distanceToPlayer) <= windRange)
			{
				if (weaponActivated == false)
				{
					TurnWindWeaponOn();
				}
			}
			else if (weaponActivated == true)
			{
				TurnWindWeaponOff();
			}
			
			// Put a texture on the robot...
			AssignTexture();
		}
	}

	#endregion


	#region Protected Functions

	//
	protected void KillRobot()
	{
		isDead = true;
		GetComponent<Renderer>().enabled = false;
		GetComponent<Collider>().enabled = false;
	}
	
	//
	protected void OnTriggerStay(Collider other) 
	{
		if (other.tag == "Player")
		{
			other.gameObject.SendMessage("TakeDamage", damage);
		}
	}
	
	// Make the robot take damage
	protected void TakeDamage(int damageTaken)
	{
		GameEngine.SoundManager.Play(AirmanLevelSounds.BOSS_HURTING);
		currentHealth -= damageTaken;
		
		if (currentHealth <= 0)
		{
			TurnWindWeaponOff();
			KillRobot();
		}
	}
	
	//
	protected void SendWindInfoToPlayer()
	{
		GameEngine.Player.ExternalForce = windDirection * windPower;
	}
	
	// Turn on the wind weapon
	protected void TurnWindWeaponOn()
	{
		weaponActivated = true;
		GameEngine.Player.IsExternalForceActive = true;
		SendWindInfoToPlayer();
	}
	
	// Turn off the wind weapon
	protected void TurnWindWeaponOff()
	{
		GameEngine.Player.IsExternalForceActive = false;
		weaponActivated = false;
	}
	
	//
	protected void MakeRobotTurnLeft()
	{
		isTurningLeft = true;
		texScale = texScaleLeft;
		windDirection *= -1;
		SendWindInfoToPlayer();
	}
	
	//
	protected void MakeRobotTurnRight()
	{
		isTurningLeft = false;
		texScale = texScaleRight;
		windDirection *= -1;
		SendWindInfoToPlayer();
	}

	//  Three textures are used to simulate animation on the robot
	protected void AssignTexture()
	{
		if (armsUp == true)
		{
			texIndex = (int) (Time.time / texArmsUpInterval);
			GetComponent<Renderer>().material = animationMaterials[(texIndex % 2) + 2 ];
			
			if (Time.time - texChangeTimer >= 0.35f)
			{
				texChangeTimer = Time.time;
				armsUp = !armsUp;
			}
		}
		else
		{
			texIndex = (int) (Time.time / texArmsDownInterval);
			GetComponent<Renderer>().material = animationMaterials[(texIndex % 2) ];
			
			if (Time.time - texChangeTimer >= 1.99f)
			{
				texChangeTimer = Time.time;
				armsUp = !armsUp;
			}
		}
		
		GetComponent<Renderer>().material.SetTextureScale("_MainTex", texScale);
	}

	#endregion


	#region Public Functions

	// 
	public void Reset()
	{
		isDead = false;
		GetComponent<Renderer>().enabled = true;
		GetComponent<Collider>().enabled = true;
		currentHealth = health;
	}

	#endregion
}
