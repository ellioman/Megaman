using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;
using System.Collections.Generic;

public class AirmanWindWeapon : MonoBehaviour
{
	#region Variables

	// Unity Editor Variables
	public Transform weaponPrefab;
	
	// Public Properties
	public bool IsTurningLeft { get; set; }
	public bool ShouldDisplayShootingTex { get; set; }
	public bool ShouldDisplayJumpingTex  { get; set; }
	public bool ShouldDisplayBlowingTex  { get; set; }

	// Protected Const Variables
	protected const int NUM_OF_SHOTS_BEFORE_JUMPING = 3;

	// Protected Instance Variables
	protected int shootingCounter = 0;
	protected bool isBlowing;
	protected bool isJumping;
	protected bool isShooting;
	protected bool isFighting;
	protected bool isPlayingAnimation;
	protected bool shouldDestroyWind = false;
	protected float fightingTimer;
	protected float blowDelay = 2.0f;	
	protected float windDestroyDelay = 1.0f;
	protected float windPower = 400.0f;
	protected Player player = null;
	protected Animation anim = null;
	protected List<Vector3> nextAttack;
	protected List<Vector3> attack1Right = new List<Vector3>();
	protected List<Vector3> attack2Right = new List<Vector3>();
	protected List<Vector3> attack3Right = new List<Vector3>();
	protected List<Vector3> attack1Left = new List<Vector3>();
	protected List<Vector3> attack2Left = new List<Vector3>();
	protected List<Vector3> attack3Left = new List<Vector3>();
	protected List<AirmanWind> windShots = new List<AirmanWind>();

	#endregion
	
	
	#region MonoBehaviour

	// Constructor
	protected void Awake ()
	{
		player = FindObjectOfType<Player>();
		Assert.IsNotNull(player);

		anim = GetComponent<Animation>();
		Assert.IsNotNull(anim);
	}
	
	// Use this for initialization
	protected void Start ()
	{
		InitAttackLists();
		nextAttack = attack1Left;
		
		anim.Stop();
		shootingCounter = 0;
		
		IsTurningLeft = true;
		isBlowing = false;
		isJumping = false;
		isShooting = false;
		isFighting = false;
		isPlayingAnimation = false;
		shouldDestroyWind = false;
		ShouldDisplayShootingTex = false;
		ShouldDisplayJumpingTex = false;
		ShouldDisplayBlowingTex = false;
	}

	// Update is called once per frame
	protected void Update ()
	{
		if (isFighting == true)
		{
			if (isShooting == true)
			{
				Shoot();
			}
			else if (isBlowing)
			{
				Blow();	
			}
			else if (isJumping == true)
			{
				Jump();	
			}
		}
	}

	#endregion


	#region Protected Functions

	// 
	protected void InitAttackLists()
	{
		// Create the attack 1 list...
		attack1Left.Add(new Vector3(-20.0f, 7.0f, 0.0f));
		attack1Left.Add(new Vector3(-16.0f, 2.0f, 0.0f));
		attack1Left.Add(new Vector3(-14.0f, 5.0f, 0.0f));
		attack1Left.Add(new Vector3(-9.0f, 0.0f, 0.0f));
		attack1Left.Add(new Vector3(-6.0f, 8.0f, 0.0f));
		attack1Left.Add(new Vector3(-3.0f, 4.0f, 0.0f));
		
		attack1Right.Add(new Vector3(20.0f, 7.0f, 0.0f));
		attack1Right.Add(new Vector3(16.0f, 2.0f, 0.0f));
		attack1Right.Add(new Vector3(14.0f, 5.0f, 0.0f));
		attack1Right.Add(new Vector3(9.0f, 0.0f, 0.0f));
		attack1Right.Add(new Vector3(6.0f, 8.0f, 0.0f));
		attack1Right.Add(new Vector3(3.0f, 4.0f, 0.0f));
		
		// Create the attack 2 list...
		attack2Left.Add(new Vector3(-18.0f, 6.0f, 0.0f));
		attack2Left.Add(new Vector3(-16.0f, 0.0f, 0.0f));
		attack2Left.Add(new Vector3(-12.0f, 12.0f, 0.0f));
		attack2Left.Add(new Vector3(-10.0f, 6.5f, 0.0f));
		attack2Left.Add(new Vector3(-9.5f, 2.5f, 0.0f));
		attack2Left.Add(new Vector3(-3.0f, 0.4f, 0.0f));
		
		attack2Right.Add(new Vector3(18.0f, 6.0f, 0.0f));
		attack2Right.Add(new Vector3(16.0f, 0.0f, 0.0f));
		attack2Right.Add(new Vector3(12.0f, 12.0f, 0.0f));
		attack2Right.Add(new Vector3(10.0f, 6.5f, 0.0f));
		attack2Right.Add(new Vector3(9.5f, 2.5f, 0.0f));
		attack2Right.Add(new Vector3(3.0f, 0.4f, 0.0f));
		
		// Create the attack 3 list...
		attack3Left.Add(new Vector3(-17.0f, 2.0f, 0.0f));
		attack3Left.Add(new Vector3(-15.5f, 5.5f, 0.0f));
		attack3Left.Add(new Vector3(-12.0f, 0.0f, 0.0f));
		attack3Left.Add(new Vector3(-10.0f, 2.5f, 0.0f));
		attack3Left.Add(new Vector3(-4.5f, 2.4f, 0.0f));
		attack3Left.Add(new Vector3(-3.5f, 4.5f, 0.0f));
		
		attack3Right.Add(new Vector3(17.0f, 2.0f, 0.0f));
		attack3Right.Add(new Vector3(15.5f, 5.5f, 0.0f));
		attack3Right.Add(new Vector3(12.0f, 0.0f, 0.0f));
		attack3Right.Add(new Vector3(10.0f, 2.5f, 0.0f));
		attack3Right.Add(new Vector3(4.5f, 2.4f, 0.0f));
		attack3Right.Add(new Vector3(3.5f, 4.5f, 0.0f));
	}

	// 
	protected void CreateWindShot(Vector3 pos)
	{
		Transform windTransform = (Transform) Instantiate(weaponPrefab, transform.position, transform.rotation);
		windTransform.SendMessage("SetPosition", transform.position + pos);
		windTransform.transform.parent = gameObject.transform;

		AirmanWind wind = windTransform.GetComponent<AirmanWind>();
		if (wind)
		{
			windShots.Add(wind);
		}
	}
	
	// 
	protected void Blow()
	{
		if (shouldDestroyWind == true)
		{
			if (Time.time - fightingTimer >= windDestroyDelay)
			{
				// Destroy the wind gameobjects and clear the list
				for(int i = 0; i < windShots.Count; i++)
				{
					Destroy(windShots[i].gameObject);
				}
				windShots.Clear();
				
				player.IsExternalForceActive = false;
				isBlowing = false;
				ShouldDisplayBlowingTex = false;
				isShooting = true;
				shouldDestroyWind = false;
				fightingTimer = Time.time;	
			}
			else
			{
				if (IsTurningLeft == true)
				{
					player.ExternalForce = -Vector3.right * windPower;
				}
				else if (IsTurningLeft == false)
				{
					player.ExternalForce = Vector3.right * windPower;
				}
			}
		}
		else
		{
			if (Time.time - fightingTimer >= blowDelay)
			{
				// Make the wind go away!
				for(int i = 0; i < windShots.Count; i++)
				{
					windShots[i].GoAway();
				}
				
				player.IsExternalForceActive = true;
				ShouldDisplayShootingTex = false;
				ShouldDisplayBlowingTex = true;
				shouldDestroyWind = true;
				fightingTimer = Time.time;
			}
		}
	}
	
	// 
	protected void Shoot()
	{
		// If the boss has shot three times...
		if (shootingCounter == NUM_OF_SHOTS_BEFORE_JUMPING)
		{
			// Make him jump to the other side and attack again...
			isShooting = false;
			isJumping = true;
			shootingCounter = 0;
		}
		else 
		{
			// Create the wind...
			foreach (Vector3 wind in nextAttack) 
			{
				CreateWindShot(wind);
			}
			
			// Set up the next attack
			if (IsTurningLeft == true)
			{
				if (shootingCounter == 0) nextAttack = attack2Left;
				else if (shootingCounter == 1) nextAttack = attack3Left;
				else if (shootingCounter == 2) nextAttack = attack1Right;
			}
			else if (IsTurningLeft == false)
			{
				if (shootingCounter == 0) nextAttack = attack2Right;
				else if (shootingCounter == 1) nextAttack = attack3Right;
				else if (shootingCounter == 2) nextAttack = attack1Left;
			}
			
			// Set the appropriate variables
			ShouldDisplayShootingTex = true;
			isBlowing = true;
			isShooting = false;
			fightingTimer = Time.time;
			shootingCounter++;
		}
	}
	
	// 
	protected void Jump()
	{
		if (isPlayingAnimation == false)
		{
			if (IsTurningLeft == true)
			{
				anim.Play("AirmanJumpToTheLeft");
			}
			else
			{
				anim.Play("AirmanJumpToTheRight");
			}
			
			ShouldDisplayJumpingTex = true;
			isPlayingAnimation = true;			
		}
		
		// If we're done playing the animation...
		if (anim.IsPlaying("AirmanJumpToTheLeft") == false && anim.IsPlaying("AirmanJumpToTheRight") == false)
		{
			isPlayingAnimation = false;
			ShouldDisplayJumpingTex = false;
			IsTurningLeft = !IsTurningLeft;
			isShooting = true;
			isJumping = false;
		}
	}

	#endregion


	#region Public Functions

	// 
	public void Attack()
	{
		isFighting = true;
		isShooting = true;
	}

	public void Reset()
	{
		anim.Stop();
		shootingCounter = 0;
		nextAttack = attack1Left;
		IsTurningLeft = true;
		isFighting = false;
		isShooting = false;
		isBlowing = false;
		isJumping = false;
		isPlayingAnimation = false;
		shouldDestroyWind = false;
		ShouldDisplayShootingTex = false;
		ShouldDisplayJumpingTex = false;
		ShouldDisplayBlowingTex = false;
		
		player.IsExternalForceActive = false;
		for(int i = 0; i < windShots.Count; i++)
		{
			Destroy(windShots[i].gameObject);
		}
		windShots.Clear();
	}

	#endregion
}

