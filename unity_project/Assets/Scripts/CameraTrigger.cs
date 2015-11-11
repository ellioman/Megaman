using UnityEngine;
using System.Collections;

public class CameraTrigger : MonoBehaviour
{
	// Unity Editor Variables
	public bool m_bossDoorTrigger;
	public BossDoor m_door;
	public bool m_onEnterCanMoveLeft;
	public bool m_onExitCanMoveLeft;
	public bool m_onEnterCanMoveRight;
	public bool m_onExitCanMoveRight;
	public bool m_onEnterCanMoveUp;
	public bool m_onExitCanMoveUp;
	public bool m_onEnterCanMoveDown;
	public bool m_onExitCanMoveDown;
	public bool m_shouldMoveCamera;
	public float m_transitionDuration;
	public Vector3 m_freezeEndPosition;
	
	// Private Instance Variables
	private LevelCamera m_camera;
	private float m_transitionStatus = 0.0f;
	private bool m_isTransitioning = false;
	private Vector3 m_startPosition;
	private float m_startTime;
	
	// Called when the Collider other enters the trigger.
	protected void OnTriggerEnter(Collider other) 
	{
		if ( other.tag == "Player" )
		{
			m_camera.CanMoveLeft = m_onEnterCanMoveLeft;
			m_camera.CanMoveRight = m_onEnterCanMoveRight;
			m_camera.CanMoveUp = m_onEnterCanMoveUp;
			m_camera.CanMoveDown = m_onEnterCanMoveDown;
			
			if ( m_bossDoorTrigger )
			{
				m_door.SendMessage("openDoor");
				Player.Instance.IsFrozen = true;
				m_camera.IsTransitioning = true;
			}
			
			if ( m_shouldMoveCamera == true )
			{
				m_startPosition = m_camera.CameraPosition;
				m_isTransitioning = true;
				m_startTime = Time.time;
				m_camera.IsTransitioning = true;
			}
		}
    }
	
	/**/
	void OnTriggerExit(Collider other) 
	{
		if ( other.tag == "Player" )
		{
			m_camera.CanMoveLeft = m_onExitCanMoveLeft;
			m_camera.CanMoveRight = m_onExitCanMoveRight;
			m_camera.CanMoveUp = m_onExitCanMoveUp;
			m_camera.CanMoveDown = m_onExitCanMoveDown;
			
			if ( m_bossDoorTrigger )
			{
				Player.Instance.IsFrozen = false;
				m_door.SendMessage("closeDoor");
				GetComponent<Collider>().enabled = false;
			}
		}
    }
	
	/**/
	void Awake()
	{
		m_camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<LevelCamera>();
	}
	
	/* Update is called once per frame */
	void Update ()
	{
		if ( m_isTransitioning == true )
		{
			m_transitionStatus = (Time.time - m_startTime) / m_transitionDuration;
			m_camera.CameraPosition = Vector3.Lerp(m_startPosition, m_freezeEndPosition, m_transitionStatus );
			
			if ( m_transitionStatus  >= 1.0 )
			{
				m_isTransitioning = false;
				m_camera.IsTransitioning = false;
				m_camera.CameraPosition = m_freezeEndPosition;
				m_camera.CanMoveLeft = m_onExitCanMoveLeft;
				m_camera.CanMoveRight = m_onExitCanMoveRight;
				m_camera.CanMoveUp = m_onExitCanMoveUp;
				m_camera.CanMoveDown = m_onExitCanMoveDown;
			}
		}
	}
}

