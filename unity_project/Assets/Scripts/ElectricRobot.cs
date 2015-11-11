using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;
using System.Collections.Generic;

public class ElectricRobot : MonoBehaviour 
{
	#region Variables

	// Unity Editor Variables
	[SerializeField] protected Rigidbody electricShot;
	[SerializeField] protected CirclingPlatform platform;
	[SerializeField] protected BoxCollider robotCollider;
	[SerializeField] protected List<Material> textureMaterials;

	// Protected Instance Variables
	protected int health = 30;
	protected int currentHealth;
	protected int damage = 10;
	protected int texIndex = 0;
	protected bool isShooting = false;
	protected bool isDead = false;
	protected float texChangeInterval = 0.1f;
	protected float distanceToStop = 32.0f;
	protected float shootingRangeDiameter = 10f;
	protected float shootAgainDelay = 2f;
	protected float shootingTimer;
	protected Vector2 texScale;
	protected Vector2 texScaleRight = new Vector2(1.0f, -1.0f);
	protected Vector2 texScaleLeft = new Vector2(-1.0f, -1.0f);
	protected Vector3 turningLeftColliderPos = new Vector3(0.2f, -0.8f, 0f);
	protected Vector3 turningRightColliderPos = new Vector3(-0.2f, -0.8f, 0f);
	protected Collider col = null;
	protected Renderer rend = null;

	#endregion


	#region MonoBehaviour

	// Constructor
	protected void Awake()
	{
		col = GetComponent<Collider>();
		Assert.IsNotNull(col);

		rend = GetComponent<Renderer>();
		Assert.IsNotNull(rend);
	}

	// Use this for initialization 
	protected void Start()
	{
		texScale = texScaleLeft;
		currentHealth = health;
	}

	// Update is called once per frame 
	protected void Update() 
	{
		// Stop fighting if the player is too far away
		if ((GameEngine.Player.transform.transform.position - transform.position).magnitude >= distanceToStop)
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

	#endregion

	
	#region Protected Functions

	// 
	protected void KillRobot()
	{
		isDead = true;
		col.enabled = false;
	}
	
	// 
	protected void OnTriggerStay(Collider other) 
	{
		if (other.tag == "Player")
		{
			GameEngine.Player.TakeDamage(damage);
		}
	}
	
	//  Make the robot take damage 
	protected void TakeDamage(int damageTaken)
	{
		GameEngine.SoundManager.Play(AirmanLevelSounds.BOSS_HURTING);
		currentHealth -= damageTaken;
		if (currentHealth <= 0)
		{
			KillRobot();
		}
	}

	// 
	protected void AssignTexture()
	{
		texIndex = (int) (Time.time / texChangeInterval);
		
		// Make the robot always face the player...
		bool playerOnLeftSide = (GameEngine.Player.transform.position.x - transform.position.x < -1.0f);
		
		// If the robot is dead
		if (isDead == true)
		{
			// display the platform textures...
			rend.material = textureMaterials[(texIndex % 2) + 6 ];
			rend.material.SetTextureScale("_MainTex", texScaleLeft);
			robotCollider.center = turningLeftColliderPos;
		}
		
		// If the robot is shooting...
		else if (isShooting == true)
		{
			rend.material = textureMaterials[(texIndex % 2) + 4 ];
			texScale = (playerOnLeftSide == true) ? texScaleLeft : texScaleRight;
			rend.material.SetTextureScale("_MainTex", texScale);
			robotCollider.center = (playerOnLeftSide == true) ? turningLeftColliderPos : turningRightColliderPos;
		}
		else
		{
			// Assign the material
			rend.material = textureMaterials[texIndex % 4];
			texScale = (playerOnLeftSide == true) ? texScaleLeft : texScaleRight;
			rend.material.SetTextureScale("_MainTex", texScale);
			robotCollider.center = (playerOnLeftSide == true) ? turningLeftColliderPos : turningRightColliderPos;
		}
	}
	
	//  Shoot an electric arrow towards the player 	
	protected void Shoot()
	{
		isShooting = true;
		shootingTimer = Time.time;
		
		Rigidbody shot = (Rigidbody) Instantiate(electricShot, transform.position, transform.rotation);
		shot.transform.parent = gameObject.transform;
		Physics.IgnoreCollision(shot.GetComponent<Collider>(), col);
		
		ElectricShot shotScript = shot.GetComponent<ElectricShot>();
		if (shotScript)
		{
			shotScript.TargetDirection = GameEngine.Player.transform.position;
		}
	}
	
	// 
	protected void CheckIfRobotCanShoot()
	{
		// If the robot is alive, check if he is in range to shoot the player
		if (isDead == false)
		{
			// If the robot is in shooting range...
			float distanceToPlayer = (GameEngine.Player.transform.position - transform.position).magnitude;
			if (distanceToPlayer < shootingRangeDiameter)
			{
				// If the robot is ready to shoot...
				if (isShooting == false && Time.time - shootingTimer >= shootAgainDelay)
				{
					Shoot();
				}
			}
		}
	}

	#endregion


	#region Public Functions

	// 
	public void SetIsShooting(bool status)
	{
		isShooting = status;
	}
	
	// 
	public void Reset()
	{
		isDead = false;
		col.enabled = true;
		currentHealth = health;
	}

	#endregion
}
