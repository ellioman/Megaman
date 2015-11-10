using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerOLD : MonoBehaviour 
{
	public bool cheat = false;
	public List<Material> mats;
	public SoundManager soundManager;
//	public HealthBar healthbar;
	
	// Shooting...
	public Rigidbody shot;
	public float shotSpeed = 10f;
	private bool isTurningRight = true;
	public float hurtingTime;
	private float hurtingTimer;
	private bool canShoot = true;
	
	// Health...
	public Rigidbody deathParticle;
	public float deathParticleSpeed;
//	private float startHealth = 100.0f;
	private float health = 100.0f;
	private bool isHurting = false;
	private bool isDead = false;
	
	// Platform variables...
	private bool touchingAMovingPlatform = false;
	private Vector3 movingPlatformVelocity;
	
	// Movement variables...
	private bool canMoveLeft = true;
	private bool canMoveRight = true;
	private bool canMoveUp = false;
	private bool canMoveDown = true;
	private bool isFrozen = false;
	private bool shouldOverrideNormalMovement = false;
	private bool isBeingAffectedByExternalVelocity = false;
	private Vector3 externalVelocity;
	
	// Jumping / falling variables...
	private bool isJumping = false;
	private bool isWalking = false;
	private bool reachedJumpingTop = false;
	private float jumpYPos;
    public float jumpHeight = 15.0f;
	public float jumpSpeed = 29.0f;
	public float walkSpeed = 30.0f;
	public float fallingDrag;
	public float jumpingDrag;
	public Vector3 gravity;
	
	// Calculation variables
	Vector3 tempVelocity;
	
	// Checkpoint variables
	private Vector3 checkpointPos;
	
	// Texture variables...
	private Vector2 texScale = new Vector2(-1.0f, -1.0f);
	private Vector2 texScaleLeft = new Vector2(1.0f, -1.0f);
	private Vector2 texScaleRight = new Vector2(-1.0f, -1.0f);
	private float standingTexInterval = 0.3f;
	private float walkingTexInterval = 0.2f;
	private int standingTexIndex;
	private int walkingTexIndex;
	
	// Public functions...
	public bool getCanMoveLeft () { return canMoveLeft; }
	public bool getCanMoveRight () { return canMoveRight; }
	public bool getCanMoveUp () { return canMoveUp; }
	public bool getCanMoveDown () { return canMoveDown; }
	
	// Set functions...
	void SetTouchingAMovingPlatform( bool status ){ this.touchingAMovingPlatform = status; }
	void SetMovingPlatformDeltaVelocity( Vector3 vel ) { this.movingPlatformVelocity = vel; }
	void SetIsBeingAffectedByExternalVelocity( bool status ) { this.isBeingAffectedByExternalVelocity = status; }
	void SetExternalVelocity( Vector3 vel ) { this.externalVelocity = vel; }
	void SetOverrideNormalMovement( bool status ) 	{ this.shouldOverrideNormalMovement = status; }
	void SetCanMoveRight( bool status ) 	{ this.canMoveRight = status; }
	void SetCanMoveLeft( bool status ) 		{ this.canMoveLeft = status; }
	void SetCanMoveUp( bool status ) 		{ this.canMoveUp = status; }
	void SetCanMoveDown( bool status ) 		{ this.canMoveDown = status; }
	void SetIsJumping( bool status ) 		{ this.isJumping = status; }
	void SetReachedJumpingTop( bool status ){ this.reachedJumpingTop = status; }
	void SetCanShoot( bool status ){ this.canShoot = status; }
	void SetUseGravity( bool status ){ this.GetComponent<Rigidbody>().useGravity = status; }
	
	/* */
	void SetIsFrozen( bool status )
	{ 
		this.isFrozen = status; 
		GetComponent<Rigidbody>().useGravity = !status;
	}
	
	/**/
	void SetCheckPointPosition( Vector3 pos )
	{
		this.checkpointPos = pos;
	}
	
	
	/**/
	void CreateDeathParticle( float speed, Vector3 pos, Vector3 vel )
	{
		Rigidbody particle = (Rigidbody) Instantiate(deathParticle, pos, transform.rotation);
		Physics.IgnoreCollision(particle.GetComponent<Collider>(), GetComponent<Collider>());
		particle.transform.Rotate(90,0,0);
		particle.velocity =  vel * speed;
	}
	
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
	
	/*
	 * 
	 * 
	 */
	IEnumerator WaitAndReset() {
		// Before the wait...
		this.isFrozen = true;
		this.isDead = true;
		this.GetComponent<Renderer>().enabled = false;
		
		soundManager.SendMessage("StopStageMusic");
		soundManager.SendMessage("StopBossTheme");
		soundManager.SendMessage("PlayDeathSound");
		
		// Start the wait...
		yield return new WaitForSeconds(3.6f);
		
		// After the wait...
		
		// Reset the player
		ResetPlayer();
		this.GetComponent<Renderer>().enabled = true;
		
		// Reset all the enemy bots...
		GameObject[] enemyRobots = GameObject.FindGameObjectsWithTag("enemyRobot");
		foreach (GameObject robot in enemyRobots)
		{
			robot.SendMessage("reset");
		}
		
		// Reset the camera
		GameObject.FindGameObjectWithTag("MainCamera").SendMessage("Reset");
		GameObject.Find("Airman").SendMessage("Reset");
		
		// Play the music again...
		soundManager.SendMessage("PlayStageMusic");
	}
	
	/*
	 * 
	 * 
	 */
	void KillPlayer() {
		if ( isDead != true )
		{
			StartCoroutine(WaitAndReset());
			StartCoroutine(CreateDeathParticles());
		}
	}
	
	/**/
	void TakeDamage( float dam )
	{
		// If the player isn't already hurting...
		if ( this.isHurting == false && this.isDead == false )
		{
			this.isHurting = true;
			this.hurtingTimer = Time.time;
			this.soundManager.SendMessage("PlayHurtingSound");	
			this.health -= dam;
//			this.healthbar.SendMessage("UpdateHealth", this.health / this.startHealth );
			
			if ( this.health <= 0.0f )
			{
				this.KillPlayer();	
			}
		}		
	}
	
	/*
	  * Use this for initialization
	  */ 
	void Start () 
	{
		this.checkpointPos = transform.position;
		Physics.gravity = gravity;
		Physics.IgnoreLayerCollision(8,9, true);
//		healthbar.SendMessage("UpdateHealth", health);
		boxCollider = (BoxCollider)GetComponent(typeof(BoxCollider));
	}
	
	/*
	 * 
	 */
	void ResetPlayer() 
	{
		this.canMoveLeft = true;
		this.canMoveRight = true;
		this.canMoveUp = true;
		this.canMoveDown = true;
		this.isFrozen = false;
		this.health = 100.0f;
		this.isDead = false;
//		healthbar.SendMessage("UpdateHealth", health);
		transform.position = checkpointPos;
	}
	
	/* Update is called once per frame */
	void Update() 
	{
		// If we are cheating...
		if ( cheat ) 
		{
			CalculateCheatMovements(); 
		}
		
		// If the player is dead or frozen, do nothing.
		else if ( this.isDead == true || this.isFrozen == true )
		{
			return;
		}
		
		// If something else is controlling the player movement...
		else if ( this.shouldOverrideNormalMovement == true )
		{
			GetComponent<Rigidbody>().velocity = movingPlatformVelocity;
		}
		
		// Otherwise, calculate the movement of the player...
		else
		{
			CalculateMovement();
		}
		
		/* Texture... */
		this.AssignTexture();
		
		/* Shooting */
		
		// Check if the player is shooting...
		if( Input.GetKeyDown(KeyCode.LeftShift) && this.canShoot )
    	{
        	this.Shoot();
    	}
		
		if( Input.GetKeyDown(KeyCode.Space) )
    	{
//        	StartCoroutine(CreateDeathParticles());
    	}
		
		/* Health */
		// Check if the player is hurting
		if ( this.isHurting == true )
		{
			if ( Time.time - this.hurtingTimer >= this.hurtingTime )
			{
				this.isHurting = false;	
			}
		}
	}
	
	private float boxColliderWidth = 1.2f;
	private float boxColliderHeight = 1.875f;
	private float boxColliderCenter = -0.073f;
	private BoxCollider boxCollider;
	
	/**/	
	void AssignTexture()
	{
		
		if ( this.isHurting )
		{
			this.transform.localScale = new Vector3 (2.3f, 2.3f, 1f);
			boxCollider.size = new Vector3 (boxColliderWidth / 2.3f, boxColliderHeight / 2.3f, 1f);
			boxCollider.center = new Vector3 (0f, boxColliderCenter, 0f);
			
			this.GetComponent<Renderer>().material = mats[7];
			this.GetComponent<Renderer>().material.color *= 0.75f + Random.value;
		}
		else if ( this.isJumping == true )
		{
			this.transform.localScale = new Vector3 (2.4f, 2.4f, 1f);
			boxCollider.size = new Vector3 (0.5f, 1.875f / 2.4f, 1f);
			boxCollider.center = new Vector3 (0f, -0.073f, 0f);
			
			this.GetComponent<Renderer>().material = mats[2];
		}
		else if ( this.isWalking )
		{
			this.transform.localScale = new Vector3 (1.875f, 1.875f, 1f);
			boxCollider.size = new Vector3 (0.64f, 1f, 1f);
			boxCollider.center = new Vector3 (0f, boxColliderCenter, 0f);
			
			this.walkingTexIndex = (int) (Time.time / this.walkingTexInterval);
			this.GetComponent<Renderer>().material = mats[ (walkingTexIndex % 4) + 3];
		}
		else 
		{
			this.transform.localScale = new Vector3 (1.875f, 1.875f, 1f);
			boxCollider.size = new Vector3 (0.64f, 1f, 1f);
			boxCollider.center = new Vector3 (0f, boxColliderCenter, 0f);
			
			this.standingTexIndex = (int) (Time.time / this.standingTexInterval);
			this.GetComponent<Renderer>().material = ( standingTexIndex % 10 == 0 ) ? mats[1] : mats[0];
		}
		this.GetComponent<Renderer>().material.SetTextureScale("_MainTex", texScale);
	}
	
	/* Create a shot in the direction the player is facing... */
	void Shoot() 
	{		
		if (this.isTurningRight == true)
		{
			Rigidbody rocketClone = (Rigidbody) Instantiate(shot, transform.position, transform.rotation);
			Physics.IgnoreCollision(rocketClone.GetComponent<Collider>(), GetComponent<Collider>());
			Physics.IgnoreLayerCollision(8,9, true);
			Physics.IgnoreLayerCollision(11,9, true);
			rocketClone.transform.Rotate(90,0,0);
			rocketClone.SendMessage("SetVelocityDirection", transform.right);
			rocketClone.SendMessage("SetShotSpeed", shotSpeed);
		}
		else
		{
			Rigidbody rocketClone = (Rigidbody) Instantiate(shot, transform.position, transform.rotation);
			Physics.IgnoreCollision(rocketClone.GetComponent<Collider>(), GetComponent<Collider>());
			Physics.IgnoreLayerCollision(8,9, true);
			Physics.IgnoreLayerCollision(11,9, true);
			rocketClone.transform.Rotate(90,0,0);
			rocketClone.SendMessage("SetVelocityDirection", -transform.right);
			rocketClone.SendMessage("SetShotSpeed", shotSpeed);
		}
		
		soundManager.SendMessage("PlayShootingSound");
//	
//		// You can also acccess other components / scripts of the clone
//		rocketClone.GetComponent<MyRocketScript>().DoSomething();
		
		
	}
	
	/*
	 * Calculate the movement of the player.
	 * Both horizontal and vertical including jumping and gravity.
	*/
	void CalculateMovement( ) 
	{
		// Clear the velocity from the last frame
		this.tempVelocity = Vector3.zero;
			
		// Horizontal
		this.tempVelocity += this.CalculateHorizontalMovement();
		
		// Vertical
		this.tempVelocity += this.CalculateVerticalMovement();
		
		// External
		this.tempVelocity += this.CalculateExternalVelocity();
		
		// If the player is hurting...
		if ( this.isHurting )
		{
			// Make him go backwards a little bit
			if ( this.isTurningRight == true )	
			{
				this.tempVelocity -= new Vector3(30.0f * Time.deltaTime, 0f ,0f);
			}
			else
			{
				this.tempVelocity += new Vector3(30.0f * Time.deltaTime, 0f ,0f);
			}	
		}
		
		
		// If the player is being affected by external velocity...
		if ( this.touchingAMovingPlatform == true )
		{
			// Move its position with the object.
			this.transform.position += this.movingPlatformVelocity;
		}
		
		this.GetComponent<Rigidbody>().velocity = this.tempVelocity;
	}
	
	/**/
	Vector3 CalculateExternalVelocity()
	{
		// If the player is being affected by external velocity...
		if ( this.isBeingAffectedByExternalVelocity == true )
		{
			// Move its position with the object.
			return this.externalVelocity;
		}
		else
		{
			return Vector3.zero;
		}
		
	}
	
	/*
	 * 
	*/
	Vector3 CalculateHorizontalMovement( ) 
	{
		Vector3 hVelocity = Vector3.zero;
		float h_value = Input.GetAxis ("Horizontal");
		if (h_value > 0.0) 
		{
			this.isTurningRight = true;
			this.isWalking = true;
			this.texScale = this.texScaleRight;
			GetComponent<Renderer>().material.SetTextureScale("_MainTex", texScaleRight);
			hVelocity = Vector3.right * Time.deltaTime * walkSpeed;
		}
		else if (h_value < 0.0 && canMoveLeft == true ) 
		{
			this.isTurningRight = false;
			this.isWalking = true;
			this.texScale = this.texScaleLeft;
			GetComponent<Renderer>().material.SetTextureScale("_MainTex", texScaleLeft);
			hVelocity = Vector3.left * Time.deltaTime * walkSpeed;
		}
		else if ( h_value == 0.0f)
		{
			this.isWalking = false;
		}
		
		return hVelocity;
	}
	
	/*
	 * 
	*/
	Vector3 CalculateVerticalMovement( ) 
	{
		Vector3 vVelocity = Vector3.zero;
		float v_value = Input.GetAxis ("Vertical");
		
		// If the player isn't already jumping, is on
		// the ground and wants to jump...
		if (this.isJumping == false && this.canMoveUp == true && v_value > 0) 
		{
			this.isJumping = true;
			this.reachedJumpingTop = false;
			this.canMoveUp = false;
			this.jumpYPos = transform.position.y;
			GetComponent<Rigidbody>().useGravity = false;
//			rigidbody.drag = this.jumpingDrag;
		}
		
		// If the player is jumping...
		if ( this.isJumping == true ) 
		{
			// ...and hasn't reached the top
			if ( this.reachedJumpingTop == false ) 
			{
				float distCovered = transform.position.y - jumpYPos;
	        	float fracJourney = distCovered / this.jumpHeight;
				
				// If the player has reached the top
				if ( fracJourney >= 1.0 ) 
				{
					this.reachedJumpingTop = true;
					Physics.gravity = this.gravity;
					GetComponent<Rigidbody>().useGravity = true;
//					rigidbody.drag = this.fallingDrag;
				}
				
				// Otherwise keep going up...
				else 
				{
					vVelocity += Vector3.up * Time.deltaTime * jumpSpeed;
				}
			}
			// ...and has reached the top (maybe hit a platform?)
			else
			{
				this.reachedJumpingTop = true;
				GetComponent<Rigidbody>().useGravity = true;
				GetComponent<Rigidbody>().drag = this.fallingDrag;
			}
				
		}
		
		return vVelocity;
	}
	
	/* Move freely inside the stage ...*/
	void CalculateCheatMovements() 
	{
		GetComponent<Rigidbody>().isKinematic = true;
		Physics.gravity = new Vector3(0.0f, 0.0f, 0.0f);
		Vector3 pos = transform.position;
		float h_value = Input.GetAxis ("Horizontal");
		float d = 20.0f;
		if (h_value > 0.0) 
		{
			pos = pos + Vector3.right * Time.deltaTime * walkSpeed / d;
			GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(texScaleRight.x, texScaleRight.y));
		}
		else if (h_value < 0.0) 
		{
			pos = pos + Vector3.left * Time.deltaTime * walkSpeed / d;
			GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(texScaleLeft.x, texScaleLeft.y));
		}
		
		float v_value = Input.GetAxis ("Vertical");
		if (v_value > 0.0) 
		{
			pos = pos + Vector3.up * Time.deltaTime * walkSpeed / d;
		}
		else if (v_value < 0.0) 
		{
			pos = pos + Vector3.down * Time.deltaTime * walkSpeed / d;
		}
		
		transform.position = pos;
	}
}
