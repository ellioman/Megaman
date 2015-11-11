using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour
{
	#region Variables

	// Unity Editor Variables
	[SerializeField] protected Vector3 playerPosition;
	[SerializeField] protected Vector3 cameraPosition;
	[SerializeField] protected bool cameraCanMoveLeft;
	[SerializeField] protected bool cameraCanMoveRight;
	[SerializeField] protected bool cameraCanMoveUp;
	[SerializeField] protected bool cameraCanMoveDown;

	#endregion
	
	
	#region MonoBehaviour

	// Called when the Collider other enters the trigger.
	protected void OnTriggerEnter(Collider other) 
	{
		if (other.tag == "Player")
		{
			GameEngine.Player.CheckpointPosition = playerPosition;
			LevelCamera cam = FindObjectOfType<LevelCamera>();
			cam.CheckpointPosition = cameraPosition;
			cam.CheckpointCanMoveLeft = cameraCanMoveLeft;
			cam.CheckpointCanMoveRight = cameraCanMoveRight;
			cam.CheckpointCanMoveUp = cameraCanMoveUp;
			cam.CheckpointCanMoveDown = cameraCanMoveDown;
			Destroy(this.gameObject);
		}
	}

	#endregion
}

