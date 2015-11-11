using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;
using System.Collections.Generic;
	
public class SmallFlyingRobot : MonoBehaviour
{
	#region Variables

	// Unity Editor Variables
	public List<Material> materials;
	
	// Protected Instance Variables
	protected bool shouldAttack = false;
	protected int damage = 10;
	protected int health = 10;
	protected int texIndex;
	protected float robotSpeed = 35;
	protected float attackDelay = 0.7f;
	protected float attackDelayTimer;
	protected float distanceToDisappear = 32.0f;
	protected float texChangeInterval = 0.2f;
	protected Renderer rend = null;
	protected Rigidbody rBody = null;

	#endregion


	#region MonoBehaviour

	// Constructor
	protected void Awake()
	{
		rBody = GetComponent<Rigidbody>();
		Assert.IsNotNull(rBody);

		rend = GetComponent<Renderer>();
		Assert.IsNotNull(rend);
	}

	// Use this for initialization 
	protected void Start ()
	{
		attackDelayTimer = Time.time;
	}

	// Update is called once per frame 
	protected void Update()
	{
		if (shouldAttack == false)
		{
			if (Time.time - attackDelayTimer >= attackDelay)
			{
				shouldAttack = true;
			}
		}
		else
		{
			Vector3 direction = GameEngine.Player.transform.position - transform.position;
			
			// Kill this object if the player is too far away
			if (direction.magnitude >= distanceToDisappear)
			{
				KillRobot();
			}
			else
			{
				rBody.velocity = direction.normalized * (Time.deltaTime * robotSpeed);
			}
		}
		
		// Update the textures...
		texIndex = (int) (Time.time / texChangeInterval);
		rend.material = materials[texIndex % materials.Count];
	}

	//
	protected void OnTriggerStay(Collider other) 
	{
		if (other.tag == "Player")
		{
			GameEngine.Player.TakeDamage(damage);
		}
	}
	
	//
	protected void OnCollisionStay(Collision collision) 
	{
		if (collision.gameObject.tag == "Player")
		{
			GameEngine.Player.TakeDamage(damage);
		}
	}

	#endregion


	#region Protected Functions
	
	//
	protected void KillRobot()
	{
		transform.parent.gameObject.GetComponent<RedHornBeast>().MinusRobotCount();
		Destroy(gameObject);
	}

	//
	protected void TakeDamage(int damageTaken)
	{
		GameEngine.SoundManager.Play(AirmanLevelSounds.BOSS_HURTING);
		health -= damageTaken;
		if (health <= 0)
		{
			KillRobot();
		}
	}

	#endregion


	#region Public Functions

	// 
	public void Reset()
	{
		KillRobot();	
	}

	#endregion
}

