using UnityEngine;
using System.Collections;

public class RedHornBeast : MonoBehaviour
{
	#region Variables
	
	// Unity Editor Variables
	public Rigidbody flyingRobot;
	
	// Protected Instance Variables
	protected int robotCount = 0;							//
	protected bool shouldAppear = false;					//
	protected bool startFighting = false;					//
	protected bool spikeRising = true;						//
	protected bool spikeLowering = false;					//
	protected bool createRobotOnRightSide = true;			//
	protected float lightStartTime;							//
	protected float distanceToDisappear = 32.0f;			//
	protected float spikeStartHeight;						//
	protected float spikeRisingSpeed = 0.075f;				//
	protected float spikeLoweringSpeed = 1.00f;				//
	protected float spikeWaitTimer = 0.0f;					// Timer used for timing the spike while they wait
	protected float spikeDelayTime = 2.0f; 					// How long should the spike wait at the top?
	protected float robotCreateDelay = 2.0f; 				// Used so that a small delay is between creating robots
	protected float robotCreateDelayTimer; 					// Used so that a small delay is between creating robots
	protected Vector3 spikeTransforms;						// Used for transforming the spike when fighting
	protected Vector3 spikeLeftPos;							//
	protected Vector4 color = new Vector4(0f, 0f, 0f, 0f);	//
	protected Transform lightTransform;						//
	protected GameObject spikeLeft;							//
	protected GameObject spikeRight;						//

	#endregion


	#region MonoBehaviour
	
	// The Constructor function in Unity...
	protected void Awake () 
	{
		lightTransform = gameObject.transform.FindChild("Light").transform;
		spikeLeft = transform.FindChild("SpikeLeft").gameObject;
		spikeRight = transform.FindChild("SpikeRight").gameObject;
	}
	
	// Use this for initialization
	protected void Start ()
	{
		lightTransform.GetComponent<Renderer>().enabled = false;
		spikeLeftPos = spikeLeft.transform.localPosition;
		spikeStartHeight = spikeLeft.transform.position.y;
		
		// Make the robot and its children invisible...
		GetComponent<Renderer>().material.color = color;
		spikeLeft.GetComponent<Renderer>().material.color = color;
		spikeRight.GetComponent<Renderer>().material.color = color;
	}
	
	// Update is called once per frame
	protected void Update ()
	{
		if ( startFighting == true ) 
		{
			MoveSpikes();
			MakeLightBlink();
			CreateSmallFlyingRobots();
			
			// Stop fighting if the player is too far away
			if ( (GameEngine.Player.transform.position - transform.position).magnitude >= distanceToDisappear )
			{
				ResetRedHornBeast();
			}
		}
		
		else if ( shouldAppear == true )
		{
			if ( color.x >= 1.0 )
			{
				shouldAppear = false;
				startFighting = true;
			}
			// Make the robot and its children visible...
			else 
			{
				color.x = color.y = color.z = color.w += Time.deltaTime * 3.5f;
			} 
			
			GetComponent<Renderer>().material.color = color;
			spikeLeft.GetComponent<Renderer>().material.color = color;
			spikeRight.GetComponent<Renderer>().material.color = color;
		}
	}

	#endregion


	#region Protected Functions

	// 
	protected void CreateRobot( float speed, Vector3 pos, Vector3 vel )
	{
		Rigidbody robot = (Rigidbody) Instantiate(flyingRobot, pos, transform.rotation);
		Physics.IgnoreCollision(robot.GetComponent<Collider>(), GetComponent<Collider>());
		robot.transform.parent = gameObject.transform;
		robot.velocity =  vel;
	}
	
	// 
	protected void CreateSmallFlyingRobots()
	{
		if ( robotCount < 1 && Time.time - robotCreateDelayTimer >= robotCreateDelay)
		{
			if ( createRobotOnRightSide )
			{
				Vector3 pos = transform.position;
				pos.x += transform.localScale.x / 2.0f;
				pos.z = -0.2f;
				CreateRobot( 0.0f, pos, Vector3.right );
			}
			else 
			{
				Vector3 pos = transform.position;
				pos.x -= transform.localScale.x / 2.0f;
				pos.z = -0.2f;
				CreateRobot( 0.0f, pos, -Vector3.right );
			}
			
			createRobotOnRightSide = !createRobotOnRightSide;
			robotCount++;
		}
	}
	
	// 
	protected void MoveSpikes()
	{
		if ( spikeRising )
		{
			spikeTransforms = new Vector3(0f, spikeLeftPos.y * Time.deltaTime * spikeRisingSpeed, 0f);
			spikeLeft.transform.localPosition += spikeTransforms;
			spikeRight.transform.localPosition += spikeTransforms;
			
			if ( spikeLeft.transform.localPosition.y - spikeLeftPos.y >= 0.18f )
			{
				spikeRising = false;
				spikeLowering = false;
			}
		}
		// 
		else if ( spikeLowering )
		{
			spikeTransforms = new Vector3(0f, spikeLeftPos.y * Time.deltaTime * spikeLoweringSpeed, 0f);
			spikeLeft.transform.localPosition -= spikeTransforms;
			spikeRight.transform.localPosition -= spikeTransforms;
			
			if ( spikeLeft.transform.localPosition.y - spikeLeftPos.y <= 0.00f )
			{
				spikeRising = true;
			}
		}
		else 
		{
			spikeWaitTimer += Time.deltaTime;
			if ( spikeWaitTimer > spikeDelayTime )
			{
				spikeLowering = true;
				spikeWaitTimer = 0.0f;
			}
		}
	}
	
	// 
	protected void MakeLightBlink()
	{
		if ( Time.time - lightStartTime >= 0.1f )
		{
			lightStartTime = Time.time;
			lightTransform.GetComponent<Renderer>().enabled = !lightTransform.GetComponent<Renderer>().enabled;
		}
	}
	
	// 
	protected void ResetRedHornBeast()
	{
		shouldAppear = false;
		startFighting = false;
		color = new Vector4(0f, 0f, 0f, 0f);
		GetComponent<Renderer>().material.color = color;
		
		Vector3 spikePos;
		spikePos = spikeLeft.transform.position;
		spikePos.y = spikeStartHeight;
		spikeLeft.transform.position = spikePos;
		spikePos.x = spikeRight.transform.position.x;
		spikeRight.transform.position = spikePos;
		
		spikeLeft.GetComponent<Renderer>().material.color = color;
		spikeRight.GetComponent<Renderer>().material.color = color;
		lightTransform.GetComponent<Renderer>().enabled = false;
		createRobotOnRightSide = true;
	}
	
	// 
	protected void KillRobotChildren()
	{
		// Reset all the enemy bots...
		Transform robot = transform.FindChild("Prb_SmallFlyingRobot(Clone)");
		if ( robot != null)
		{
			Destroy(robot.gameObject);
		}
		robotCount = 0;	
	}

	#endregion


	#region Public Functions
	
	// 
	public void Reset()
	{
		ResetRedHornBeast();
		KillRobotChildren();
	}

	// 
	public void Appear()
	{
		shouldAppear = true;
		lightStartTime = Time.time;
	}
	
	// 
	public void MinusRobotCount() 
	{
		robotCount--; robotCreateDelayTimer = Time.time;
	}

	#endregion
}

