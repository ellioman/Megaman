using UnityEngine;
using System.Collections;

[RequireComponent (typeof (CharacterController))]
public class Movement : MonoBehaviour
{
	#region Variables

	// Properties
	public bool IsTurningLeft			{ get; protected set; }
	public bool IsJumping 				{ get; protected set; }
	public bool IsWalking				{ get; protected set; }
	public bool IsHurting				{ get; set; }
	public bool IsFrozen 				{ get; set; }
	public bool IsExternalForceActive 	{ get; set; }
	public Vector3 ExternalForce 		{ get; set; }
	public Vector3 CheckPointPosition 	{ get; set; }
	
	// Protected Instance Variables
	protected CharacterController charController;
	protected bool cheating = false;
	protected float gravity = 40f;	 			// Downward force
	protected float terminalVelocity = 20f;	// Max downward speed
	protected float jumpSpeed = 20f;			// Upward speed
	protected float moveSpeed = 10f;			// Left/Right speed
	protected float verticalVelocity;
	protected float hurtingForce = 2.0f;
	protected Vector3 moveVector = Vector3.zero;
	protected Vector3 startPosition = new Vector3(13.34303f, 11.51588f, 0f);

	#endregion
	
	
	#region MonoBehaviour

	// Use this for initialization
	protected void Awake()
	{
		charController = (CharacterController) gameObject.GetComponent("CharacterController");
	}
	
	// Use this for initialization
	protected void Start ()
	{
		IsHurting = false;
		transform.position = CheckPointPosition = startPosition;
	}

	#endregion
	
	
	#region Protected Functions

	//
	protected void ApplyGravity()
	{
		if (moveVector.y > -terminalVelocity)
		{
			moveVector = new Vector3(moveVector.x, (moveVector.y - gravity * Time.deltaTime), moveVector.z);
		}
		
		if (charController.isGrounded && moveVector.y < -1)
		{
			IsJumping = false;
			moveVector = new Vector3(moveVector.x, (-1), moveVector.z);
		}
	}
	
	//
	protected void ProcessExternalForces()
	{
		// 
		if (IsExternalForceActive == true)
		{
			moveVector += ExternalForce * Time.deltaTime;
		}
	}
	
	//
	protected void ProcessMovement()
	{
		//transform moveVector into world-space relative to character rotation
		moveVector = transform.TransformDirection(moveVector);
		
		//normalize moveVector if magnitude > 1
		if (moveVector.magnitude > 1)
		{
			moveVector = Vector3.Normalize(moveVector);
		}
		
		//multiply moveVector by moveSpeed
		moveVector *= moveSpeed;
		
		//
		ProcessExternalForces();
		
		// 
		if (IsHurting == true)
		{
			moveVector += (((IsTurningLeft == true) ? Vector3.right : -Vector3.right) * hurtingForce);
		}
		
		//reapply vertical velocity to moveVector.y
		moveVector = new Vector3(moveVector.x, verticalVelocity, 0.0f);
		
		//apply gravity
		ApplyGravity();
		
		//move character in world-space
		charController.Move(moveVector * Time.deltaTime);
	}

	//
	protected void CheckMovement()
	{
		// Horizontal movement...
		float deadZone = 0.01f;
		verticalVelocity = moveVector.y;
		moveVector = Vector3.zero;
		
		if (Input.GetAxis("Horizontal") > deadZone)
		{
			IsWalking = true;
			IsTurningLeft = false;
			moveVector += new Vector3(Input.GetAxis("Horizontal"),0,0);
		}
		else if (Input.GetAxis("Horizontal") < -deadZone)
		{
			IsWalking = true;
			IsTurningLeft = true;
			moveVector += new Vector3(Input.GetAxis("Horizontal"),0,0);
		}
		else 
		{
			IsWalking = false;
		}
		
		// Vertical movement...
		if (Input.GetAxis("Vertical") > 0.0f)
		{
			if (charController.isGrounded)
			{
				IsJumping = true;
				verticalVelocity = jumpSpeed;
			}
		}

		// If there is a collision above...
		if ((charController.collisionFlags & CollisionFlags.Above) != 0)
		{
			verticalVelocity = -5.0f;
		}
	}

	#endregion
	
	
	#region Public Functions
	
	//
	public void Reset()
	{
		IsFrozen = false;
		IsHurting = false;
		transform.position = CheckPointPosition;
	}

	//
	public void HandleMovement()
	{
		if (IsFrozen == true)
		{
			moveVector = Vector3.zero;
			ProcessExternalForces();
			charController.Move(moveVector * Time.deltaTime);
		}
		else
		{
			CheckMovement();
			ProcessMovement();
		}
	}

	#endregion
}

