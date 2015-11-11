using UnityEngine;
using System.Collections;

public class BossDoor : MonoBehaviour 
{
	#region Variables

	// Protected Instance Variables
	protected float playerSpeed = 250f;
	protected float doorSpeed = 10f;
	protected bool isOpening = false;
	protected bool isClosing = false;
	protected bool hasPlayerGoneThrough = false;
	protected Vector3 startScale = Vector3.zero;

	#endregion


	#region MonoBehaviour
	
	// Use this for initialization 
	protected void Start()
	{
		startScale = transform.localScale;
	}

	// Update is called once per frame 
	protected void Update() 
	{
		if (hasPlayerGoneThrough == true)
		{
			return;
		}
		
		if (isOpening)
		{
			scaleCube(-doorSpeed * Time.deltaTime);
			
			if (transform.localScale.y <= 0.5)
			{
				isOpening = false;
				GameEngine.Player.IsExternalForceActive = true;
				GameEngine.Player.ExternalForce = new Vector3 (playerSpeed, 0.0f, 0.0f);
				GameEngine.SoundManager.Stop(AirmanLevelSounds.BOSS_DOOR);
			}
		}
		
		else if (isClosing)
		{
			scaleCube(doorSpeed * Time.deltaTime);
			
			if (transform.localScale.y >= startScale.y)
			{
				isClosing = false;
				hasPlayerGoneThrough = true;
				GameEngine.Player.IsExternalForceActive = false;
				GameEngine.SoundManager.Stop(AirmanLevelSounds.BOSS_DOOR);
			}
		}
	}

	#endregion


	#region Protected Functions

	//
	protected void Reset()
	{
		isOpening = false;
		isClosing = false;
		hasPlayerGoneThrough = false;
	}
	
	//
	protected void scaleCube(float scaleAmount)
	{
		Vector3 sc = transform.localScale;
		Vector3 pos = transform.localPosition;
		
		//Scale the object on X axis in local units
		sc.y += scaleAmount;
		transform.localScale = sc;
 
		//Move the object on X axis in local units
		pos.y -= scaleAmount / 1.3f; 
		transform.localPosition = pos;
	}

	#endregion
	
	
	#region Public Functions

	//
	public void OpenDoor()
	{
		GameEngine.SoundManager.Play(AirmanLevelSounds.BOSS_DOOR);
		gameObject.GetComponent<Collider>().isTrigger = true;
		isOpening = true;
	}
	
	//
	public void CloseDoor()
	{
		GameEngine.SoundManager.Play(AirmanLevelSounds.BOSS_DOOR);
		gameObject.GetComponent<Collider>().isTrigger = false;
		isClosing = true;
	}	
	
	#endregion
}
