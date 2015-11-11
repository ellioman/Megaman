using UnityEngine;
using System.Collections;

public class LevelCamera : MonoBehaviour 
{
	#region Variables

	// Public Properties
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
	private Vector3 playerPos;
	private Vector3 deltaPos;

	#endregion


	#region MonoBehaviour
	
	// Use this for initialization
	protected void Start () 
	{
		Vector3 startPosition = new Vector3(13.34303f, 11.51588f, -10f);
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
	protected void Update () 
	{
		// If the camera is transitioning between parts of the scene...
		if (IsTransitioning == true || ShouldStayStill == true)
		{
			return;
		}
		
		// Make the camera follow the player...
		playerPos = GameEngine.Player.transform.position;
		deltaPos = playerPos - transform.position;
		
		// Check the x pos 
		if ((deltaPos.x < 0.0f && CanMoveLeft) || (deltaPos.x > 0.0f && CanMoveRight)) 		
		{
			transform.position = new Vector3(playerPos.x, transform.position.y, transform.position.z);
		}
		
		// Check the y pos 
		if ((deltaPos.y < 0.0f && CanMoveDown) || (deltaPos.y > 0.0f && CanMoveUp)) 		
		{
			transform.position = new Vector3(transform.position.x, playerPos.y, transform.position.z);
		}
		
		// Make the level restart if the user presses escape...
		if (Input.GetKeyDown (KeyCode.Escape)) 
		{
    		Application.LoadLevel (0);
  		} 
	}

	#endregion


	#region Public Functions

	// 
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

	#endregion
}
