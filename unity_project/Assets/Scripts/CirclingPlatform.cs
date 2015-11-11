using UnityEngine;
using System.Collections;

public class CirclingPlatform : MonoBehaviour
{
	#region Variables

	// Unity Editor Variables
	public bool clockWise;		// Should the platform move clockwise?
	public float beginningAngle;	// At what angle should it start with?
	public float circleWidth; 	// What is the width of the ellipse/circle?
	public float circleHeight; 	// What is the height of the ellipse/circle?
	public float speedInSeconds; 	// How long should it take to move in a full circle?
	
	// Public Properties
	public bool ShouldAnimate { get; set; }
	
	// Private Instance Variables
	private Vector3 currentPos;
	private float speedScale;
	private Vector3 circleCenter;
	private float angle = 0.0f;
	private float fullCircle = (2.0f*Mathf.PI);
	private float fullCircleInDeg = 360.0f;
	private float convertFromDeg;

	#endregion

	#region MonoBehaviour

	// Use this for initialization
	void Start() 
	{
		currentPos = transform.position;
		convertFromDeg = (fullCircle / fullCircleInDeg);
		circleCenter = transform.position;
		ShouldAnimate = false;
	}
	
	// Called when the Collider other enters the trigger.
	protected void OnTriggerEnter( Collider other)
	{
		if (other.tag == "Player")
		{
			GameEngine.Player.transform.parent =  gameObject.transform;
		}
	}
	
	// 
	protected void OnTriggerExit( Collider other)
	{
		if (other.tag == "Player")
		{
			GameEngine.Player.transform.parent = null;
		}
	}
	
	// Update is called once per frame
	protected void Update()
	{
		if (speedInSeconds <= 0)
		{
			return;
		}
		else if (ShouldAnimate == true)
		{
			speedScale = fullCircle / speedInSeconds;
			
			if (clockWise == true)
			{
				angle = convertFromDeg * beginningAngle + (Time.time * speedScale) % fullCircle;
			}
			else if (clockWise == false)
			{
				angle = fullCircle - convertFromDeg * beginningAngle + (Time.time * speedScale) % fullCircle;
			}
			// Ellipse approach
			currentPos.x = circleCenter.x + (circleWidth/2.0f) * Mathf.Cos(angle);
			currentPos.y = circleCenter.y + (circleHeight/2.0f) * Mathf.Sin(angle);
			
			transform.position = currentPos;
		}
	}

	#endregion
}