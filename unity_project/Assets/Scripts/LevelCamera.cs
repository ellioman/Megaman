using UnityEngine;
using System.Collections;

public class LevelCamera : MonoBehaviour 
{
	// Properties
	public Vector3 CameraPosition		{ get{return transform.position;} set{transform.position = value;} }
	public Vector3 CheckpointPosition 	{ get; set; }
	public bool CheckpointCanMoveLeft 	{ get; set; }
	public bool CheckpointCanMoveRight 	{ get; set; }
	public bool CheckpointCanMoveUp 	{ get; set; }
	public bool CheckpointCanMoveDown 	{ get; set; }
	public bool CanMoveLeft				{ get; set; }
	public bool CanMoveRight			{ get; set; }
	public bool CanMoveUp				{ get; set; }
	public bool CanMoveDown				{ get; set; }
	public bool IsTransitioning			{ get; set; }
	public bool ShouldStayStill			{ get; set; }
	
	// Private Instance Variables
	private Vector3 m_playerPos;
	private Vector3 m_deltaPos;
	
	/**/
	public void Reset()
	{
		ShouldStayStill = false;
		IsTransitioning = false;
		transform.position = CheckpointPosition;
		CanMoveLeft = CheckpointCanMoveLeft;
		CanMoveRight = CheckpointCanMoveRight;
		CanMoveUp = CheckpointCanMoveUp;
		CanMoveDown = CheckpointCanMoveDown;		
		transform.position = CameraPosition;
	}
	
	/* Use this for initialization */
	void Start () 
	{
		Vector3 startPosition = new Vector3( 13.34303f, 11.51588f, -10f);
//		startPosition.y = -24.2f;
		transform.position = startPosition; 
		CheckpointPosition = startPosition;
		
		ShouldStayStill = false;
		IsTransitioning = false;
		CanMoveRight = true;
		CanMoveLeft = true;
		CanMoveUp = false;
		CanMoveDown = false;
		CheckpointCanMoveLeft = false;
		CheckpointCanMoveRight = true;
		CheckpointCanMoveUp = false;
		CheckpointCanMoveDown = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		// If the camera is transitioning between parts of the scene...
		if ( IsTransitioning == true || ShouldStayStill == true )
		{
			return;
		}
		
		// Make the camera follow the player...
		m_playerPos = Player.Instance.transform.position;
		m_deltaPos = m_playerPos - transform.position;
		
		// Check the x pos 
		if ( (m_deltaPos.x < 0.0f && CanMoveLeft) || (m_deltaPos.x > 0.0f && CanMoveRight) ) 		
		{
			transform.position = new Vector3( m_playerPos.x, transform.position.y, transform.position.z );
		}
		
		// Check the y pos 
		if ( (m_deltaPos.y < 0.0f && CanMoveDown) || (m_deltaPos.y > 0.0f && CanMoveUp)) 		
		{
			transform.position = new Vector3( transform.position.x, m_playerPos.y, transform.position.z );
		}
		
		// Make the level restart if the user presses escape...
		if (Input.GetKeyDown (KeyCode.Escape)) 
		{
    		Application.LoadLevel (0);
  		} 
	}
}
