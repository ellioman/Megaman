using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour
{
	// Unity Editor Variables
	public Vector3 m_playerPosition;
	public Vector3 m_cameraPosition;
	public bool m_cameraCanMoveLeft;
	public bool m_cameraCanMoveRight;
	public bool m_cameraCanMoveUp;
	public bool m_cameraCanMoveDown;
	
	// Called when the Collider other enters the trigger.
	protected void OnTriggerEnter(Collider other) 
	{
		if ( other.tag == "Player" )
		{
			Player.Instance.CheckpointPosition = m_playerPosition;
			LevelCamera cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<LevelCamera>();
			cam.CheckpointPosition = m_cameraPosition;
			cam.CheckpointCanMoveLeft = m_cameraCanMoveLeft;
			cam.CheckpointCanMoveRight = m_cameraCanMoveRight;
			cam.CheckpointCanMoveUp = m_cameraCanMoveUp;
			cam.CheckpointCanMoveDown = m_cameraCanMoveDown;
			Destroy(this.gameObject);
		}
	}
}

