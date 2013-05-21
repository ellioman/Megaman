using UnityEngine;
using System.Collections;

[RequireComponent (typeof (CharacterController))]
public class Movement : MonoBehaviour
{
	// Properties
	public bool IsTurningLeft			{ get; private set; }
	public bool IsJumping 				{ get; private set; }
	public bool IsWalking				{ get; private set; }
	public bool IsHurting				{ get; set; }
	public bool IsFrozen 				{ get; set; }
	public bool IsExternalForceActive 	{ get; set; }
	public Vector3 ExternalForce 		{ get; set; }
	public Vector3 CheckPointPosition 	{ get; set; }
	
	// Private Instance Variables
	private CharacterController m_charController;
	private bool m_cheating = false;
	private float m_gravity = 40f;	 			// Downward force
	private float m_terminalVelocity = 20f;	// Max downward speed
	private float m_jumpSpeed = 20f;			// Upward speed
	private float m_moveSpeed = 10f;			// Left/Right speed
	private float m_verticalVelocity;
	private float m_hurtingForce = 2.0f;
	private Vector3 m_moveVector;
	private Vector3 m_startPosition = new Vector3( 13.34303f, 11.51588f, 0f);
	
	/* Use this for initialization */
	private void Awake () {
		m_charController = (CharacterController) gameObject.GetComponent("CharacterController");
	}
	
	/* Use this for initialization */
	void Start ()
	{
		IsHurting = false;
		transform.position = CheckPointPosition = m_startPosition;
	}
	
	/**/
	public void Reset()
	{
		IsFrozen = false;
		IsHurting = false;
		transform.position = CheckPointPosition;
	}
	
	/**/
	private void ApplyGravity()
	{
		if(m_moveVector.y > -m_terminalVelocity)
		{
			m_moveVector = new Vector3(m_moveVector.x, (m_moveVector.y - m_gravity * Time.deltaTime), m_moveVector.z);
		}
		
		if( m_charController.isGrounded && m_moveVector.y < -1)
		{
			IsJumping = false;
			m_moveVector = new Vector3(m_moveVector.x, (-1), m_moveVector.z);
		}
	}
	
	/**/
	private void ProcessExternalForces()
	{
		// 
		if ( IsExternalForceActive == true )
		{
			m_moveVector += ExternalForce * Time.deltaTime;
		}
	}
	
	/**/
	private void ProcessMovement()
	{
		//transform moveVector into world-space relative to character rotation
		m_moveVector = transform.TransformDirection( m_moveVector );
		
		//normalize moveVector if magnitude > 1
		if( m_moveVector.magnitude > 1 )
		{
			m_moveVector = Vector3.Normalize(m_moveVector);
		}
		
		//multiply moveVector by moveSpeed
		m_moveVector *= m_moveSpeed;
		
		//
		ProcessExternalForces();
		
		// 
		if ( IsHurting == true )
		{
			m_moveVector += ( ((IsTurningLeft == true ) ? Vector3.right : -Vector3.right) * m_hurtingForce);
		}
		
		//reapply vertical velocity to moveVector.y
		m_moveVector = new Vector3(m_moveVector.x, m_verticalVelocity, 0.0f);
		
		//apply gravity
		ApplyGravity();
		
		//move character in world-space
		m_charController.Move(m_moveVector * Time.deltaTime);
	}
	
	
	/**/
	private void CheckMovement()
	{
		// Horizontal movement...
		float deadZone = 0.01f;
		m_verticalVelocity = m_moveVector.y;
		m_moveVector = Vector3.zero;
		
		if( Input.GetAxis("Horizontal") > deadZone )
		{
			IsWalking = true;
			IsTurningLeft = false;
			m_moveVector += new Vector3(Input.GetAxis("Horizontal"),0,0);
		}
		else if ( Input.GetAxis("Horizontal") < -deadZone )
		{
			IsWalking = true;
			IsTurningLeft = true;
			m_moveVector += new Vector3(Input.GetAxis("Horizontal"),0,0);
		}
		else 
		{
			IsWalking = false;
		}
		
		// Vertical movement...
		if( Input.GetAxis("Vertical") > 0.0f )
		{
			if( m_charController.isGrounded )
			{
				IsJumping = true;
				m_verticalVelocity = m_jumpSpeed;
			}
		}
		// If there is a collision above...
		if ( (m_charController.collisionFlags & CollisionFlags.Above) != 0 )
		{
			m_verticalVelocity = -5.0f;
		}
	}
	
	/**/
	public void HandleMovement()
	{
		if ( m_cheating == true )
		{
			CalculateCheatMovements();
		}
		else if ( IsFrozen == true )
		{
			m_moveVector = Vector3.zero;
			ProcessExternalForces();
			m_charController.Move(m_moveVector * Time.deltaTime);
		}
		else
		{
			CheckMovement();
			ProcessMovement();
		}
	}
	
	/* Move freely inside the stage ...*/
	void CalculateCheatMovements() 
	{
		// TODO: Fix this code
//		rigidbody.isKinematic = true;
//		Physics.gravity = new Vector3(0.0f, 0.0f, 0.0f);
//		Vector3 pos = transform.position;
//		float h_value = Input.GetAxis ("Horizontal");
//		float d = 20.0f;
//		if (h_value > 0.0) 
//		{
//			pos = pos + Vector3.right * Time.deltaTime * walkSpeed / d;
//			renderer.material.SetTextureScale("_MainTex", new Vector2(texScaleRight.x, texScaleRight.y));
//		}
//		else if (h_value < 0.0) 
//		{
//			pos = pos + Vector3.left * Time.deltaTime * walkSpeed / d;
//			renderer.material.SetTextureScale("_MainTex", new Vector2(texScaleLeft.x, texScaleLeft.y));
//		}
//		
//		float v_value = Input.GetAxis ("Vertical");
//		if (v_value > 0.0) 
//		{
//			pos = pos + Vector3.up * Time.deltaTime * walkSpeed / d;
//		}
//		else if (v_value < 0.0) 
//		{
//			pos = pos + Vector3.down * Time.deltaTime * walkSpeed / d;
//		}
//		
//		transform.position = pos;
	}
	
//	// Update is called once per frame
//	void Update ()
//	{
////		 CharacterController controller = GetComponent<CharacterController>();
////        if (controller.collisionFlags == CollisionFlags.None)
////            print("Free floating!");
////        
////		if ((controller.collisionFlags & CollisionFlags.Sides) != 0)
////            print("Touching sides!");
////        
////        if (controller.collisionFlags == CollisionFlags.Sides)
////            print("Only touching sides, nothing else!");
////        
////		if ((controller.collisionFlags & CollisionFlags.Above) != 0)
////            print("touched the ceiling");
////        
////        if (controller.collisionFlags == CollisionFlags.Above)
////            print("Only touching Ceiling, nothing else!");
////        
////		if ((controller.collisionFlags & CollisionFlags.Below) != 0)
////            print("Touching ground");
////        
////        if (controller.collisionFlags == CollisionFlags.Below)
////            print("Only touching ground, nothing else!");
//		
//		checkMovement();
//		HandleActionInput();
//		processMovement();
//	}
}

