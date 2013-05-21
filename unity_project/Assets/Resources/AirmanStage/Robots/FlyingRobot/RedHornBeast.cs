using UnityEngine;
using System.Collections;

public class RedHornBeast : MonoBehaviour
{
	// Unity Editor Variables
	public Rigidbody flyingRobot;
	
	// Private Instance Variables
	private Transform m_light;								//
	private GameObject m_player;							//
	private GameObject m_spikeLeft;							//
	private GameObject m_spikeRight;						//
	private bool m_shouldAppear = false;					//
	private bool m_startFighting = false;					//
	private bool m_spikeRising = true;						//
	private bool m_spikeLowering = false;					//
	private bool m_createRobotOnRightSide = true;			//
	private int m_robotCount = 0;							//
	private float m_lightStartTime;							//
	private float m_distanceToDisappear = 32.0f;			//
	private float m_spikeStartHeight;						//
	private float m_spikeRisingSpeed = 0.075f;				//
	private float m_spikeLoweringSpeed = 1.00f;				//
	private float m_spikeWaitTimer = 0.0f;					// Timer used for timing the spike while they wait
	private float m_spikeDelayTime = 2.0f; 					// How long should the spike wait at the top?
	private float m_robotCreateDelay = 2.0f; 				// Used so that a small delay is between creating robots
	private float m_robotCreateDelayTimer; 					// Used so that a small delay is between creating robots
	private Vector3 m_spikeTransforms;						// Used for transforming the spike when fighting
	private Vector3 m_spikeLeftPos;							//
	private Vector4 m_color = new Vector4(0f, 0f, 0f, 0f);	//
	
	/* */
	public void Appear()
	{
		m_shouldAppear = true;
		m_lightStartTime = Time.time;
	}
	
	/**/
	public void MinusRobotCount() 
	{
		m_robotCount--; m_robotCreateDelayTimer = Time.time;
	}
	
	/**/
	void ResetRedHornBeast()
	{
		m_shouldAppear = false;
		m_startFighting = false;
		m_color = new Vector4(0f, 0f, 0f, 0f);
		renderer.material.color = m_color;
		
		Vector3 spikePos;
		spikePos = m_spikeLeft.transform.position;
		spikePos.y = m_spikeStartHeight;
		m_spikeLeft.transform.position = spikePos;
		spikePos.x = m_spikeRight.transform.position.x;
		m_spikeRight.transform.position = spikePos;
		
		m_spikeLeft.renderer.material.color = m_color;
		m_spikeRight.renderer.material.color = m_color;
		m_light.renderer.enabled = false;
		m_createRobotOnRightSide = true;
	}
	
	/**/
	private void KillRobotChildren()
	{
		// Reset all the enemy bots...
		Transform robot = transform.FindChild("Prb_SmallFlyingRobot(Clone)");
		if ( robot != null)
		{
			Destroy(robot.gameObject);
		}
		m_robotCount = 0;	
	}
	
	/*
	 * 
	 */
	public void Reset()
	{
		ResetRedHornBeast();
		KillRobotChildren();
	}
	
	/* The Constructor function in Unity... */
	void Awake () 
	{
		m_player = GameObject.FindGameObjectWithTag("Player");
		m_light = gameObject.transform.FindChild("Light").transform;
		m_spikeLeft = transform.FindChild("SpikeLeft").gameObject;
		m_spikeRight = transform.FindChild("SpikeRight").gameObject;
	}
	
	/* Use this for initialization */
	void Start ()
	{
		m_light.renderer.enabled = false;
		m_spikeLeftPos = m_spikeLeft.transform.localPosition;
		m_spikeStartHeight = m_spikeLeft.transform.position.y;
		
		// Make the robot and its children invisible...
		renderer.material.color = m_color;
		m_spikeLeft.renderer.material.color = m_color;
		m_spikeRight.renderer.material.color = m_color;
	}
	
	/* Update is called once per frame */
	void Update ()
	{
		if ( m_startFighting == true ) 
		{
			MoveSpikes();
			MakeLightBlink();
			CreateSmallFlyingRobots();
			
			// Stop fighting if the player is too far away
			if ( (m_player.transform.position - transform.position).magnitude >= m_distanceToDisappear )
			{
				ResetRedHornBeast();
			}
		}
		
		else if ( m_shouldAppear == true )
		{
			if ( m_color.x >= 1.0 )
			{
				m_shouldAppear = false;
				m_startFighting = true;
			}
			// Make the robot and its children visible...
			else 
			{
				m_color.x = m_color.y = m_color.z = m_color.w += Time.deltaTime * 3.5f;
			} 
			
			renderer.material.color = m_color;
			m_spikeLeft.renderer.material.color = m_color;
			m_spikeRight.renderer.material.color = m_color;
		}
	}
	
	/* */
	void CreateRobot( float speed, Vector3 pos, Vector3 vel )
	{
		Rigidbody robot = (Rigidbody) Instantiate(flyingRobot, pos, transform.rotation);
		Physics.IgnoreCollision(robot.collider, collider);
		robot.transform.parent = gameObject.transform;
		robot.velocity =  vel;
	}
	
	/* */
	void CreateSmallFlyingRobots()
	{
		if ( m_robotCount < 1 && Time.time - m_robotCreateDelayTimer >= m_robotCreateDelay)
		{
			if ( m_createRobotOnRightSide )
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
			
			m_createRobotOnRightSide = !m_createRobotOnRightSide;
			m_robotCount++;
		}
	}
	
	/* */
	void MoveSpikes()
	{
		if ( m_spikeRising )
		{
			m_spikeTransforms = new Vector3(0f, m_spikeLeftPos.y * Time.deltaTime * m_spikeRisingSpeed, 0f);
			m_spikeLeft.transform.localPosition += m_spikeTransforms;
			m_spikeRight.transform.localPosition += m_spikeTransforms;
			
			if ( m_spikeLeft.transform.localPosition.y - m_spikeLeftPos.y >= 0.18f )
			{
				m_spikeRising = false;
				m_spikeLowering = false;
			}
		}
		// 
		else if ( m_spikeLowering )
		{
			m_spikeTransforms = new Vector3(0f, m_spikeLeftPos.y * Time.deltaTime * m_spikeLoweringSpeed, 0f);
			m_spikeLeft.transform.localPosition -= m_spikeTransforms;
			m_spikeRight.transform.localPosition -= m_spikeTransforms;
			
			if ( m_spikeLeft.transform.localPosition.y - m_spikeLeftPos.y <= 0.00f )
			{
				m_spikeRising = true;
			}
		}
		else 
		{
			m_spikeWaitTimer += Time.deltaTime;
			if ( m_spikeWaitTimer > m_spikeDelayTime )
			{
				m_spikeLowering = true;
				m_spikeWaitTimer = 0.0f;
			}
		}
	}
	
	/**/
	void MakeLightBlink()
	{
		if ( Time.time - m_lightStartTime >= 0.1f )
		{
			m_lightStartTime = Time.time;
			m_light.renderer.enabled = !m_light.renderer.enabled;
		}
	}
}

