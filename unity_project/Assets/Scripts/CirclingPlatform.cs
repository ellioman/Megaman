using UnityEngine;
using System.Collections;

public class CirclingPlatform : MonoBehaviour {
	// Unity Editor Variables
	public bool m_clockWise;		// Should the platform move clockwise?
	public float m_beginningAngle;	// At what angle should it start with?
	public float m_circleWidth; 	// What is the width of the ellipse/circle?
	public float m_circleHeight; 	// What is the height of the ellipse/circle?
  	public float m_speedInSeconds; 	// How long should it take to move in a full circle?
	
	// Properties
	public bool ShouldAnimate { get; set; }
	
	// Private Instance Variables
	private Vector3 m_currentPos;
	private float m_speedScale;
	private Vector3 m_circleCenter;
	private float m_angle = 0.0f;
	private float m_fullCircle = (2.0f*Mathf.PI);
	private float m_fullCircleInDeg = 360.0f;
	private float m_convertFromDeg;
	
	/* Use this for initialization */
	void Start () 
	{
		m_currentPos = transform.position;
		m_convertFromDeg = (m_fullCircle / m_fullCircleInDeg);
		m_circleCenter = transform.position;
		ShouldAnimate = false;
	}
	
	// Called when the Collider other enters the trigger.
	protected void OnTriggerEnter( Collider other )
	{
		if ( other.tag == "Player" )
		{
			Player.Instance.transform.parent =  gameObject.transform;
		}
	}
	
	/**/
	void OnTriggerExit( Collider other )
	{
		if ( other.tag == "Player" )
		{
			Player.Instance.transform.parent = null;
		}
	}
	
	/* Update is called once per frame */
	void Update () {
		if ( m_speedInSeconds <= 0 )
		{
			return;
		}
		else if ( ShouldAnimate == true )
		{
			m_speedScale = m_fullCircle / m_speedInSeconds;
			
			if ( m_clockWise == true ) {
				m_angle = m_convertFromDeg * m_beginningAngle + (Time.time * m_speedScale) % m_fullCircle;
			}
			else if ( m_clockWise == false ) {
				m_angle = m_fullCircle - m_convertFromDeg * m_beginningAngle + (Time.time * m_speedScale) % m_fullCircle;
			}
			
			// Circle approach
//			currentPos.x = CircleCenter.x + Mathf.Sin(angle) * radius;
//			currentPos.y = CircleCenter.y + Mathf.Cos(angle) * radius;
			
			// Ellipse approach
			m_currentPos.x = m_circleCenter.x + (m_circleWidth/2.0f) * Mathf.Cos(m_angle);
			m_currentPos.y = m_circleCenter.y + (m_circleHeight/2.0f) * Mathf.Sin(m_angle);
			
			transform.position = m_currentPos;
		}
	}
}