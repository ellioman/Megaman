using UnityEngine;
using System.Collections;

public class BossDoor : MonoBehaviour 
{
	// protected Instance Variables
	protected float playerSpeed;
	protected float doorSpeed;
	protected bool isOpening = false;
	protected bool isClosing = false;
	protected bool hasPlayerGoneThrough = false;
	protected Vector3 startScale;

	
	// Use this for initialization 
	protected void Start () {
		startScale = transform.localScale;
		playerSpeed = 250.0f;
		doorSpeed = 10.0f;
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
				Player.Instance.IsExternalForceActive = true;
				Player.Instance.ExternalForce = new Vector3 (playerSpeed, 0.0f, 0.0f);
				SoundManager.Instance.Stop(AirmanLevelSounds.BOSS_DOOR);
			}
		}
		
		else if (isClosing)
		{
			scaleCube(doorSpeed * Time.deltaTime);
			
			if (transform.localScale.y >= startScale.y)
			{
				isClosing = false;
				hasPlayerGoneThrough = true;
				Player.Instance.IsExternalForceActive = false;
				SoundManager.Instance.Stop(AirmanLevelSounds.BOSS_DOOR);
			}
		}
	}
	
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
		pos.y -= scaleAmount/1.3f; 
		transform.localPosition = pos;
	}
	
	//
	protected void openDoor()
	{
		SoundManager.Instance.Play(AirmanLevelSounds.BOSS_DOOR);
		gameObject.GetComponent<Collider>().isTrigger = true;
		isOpening = true;
	}
	
	//
	protected void closeDoor()
	{
		SoundManager.Instance.Play(AirmanLevelSounds.BOSS_DOOR);
		gameObject.GetComponent<Collider>().isTrigger = false;
		isClosing = true;
	}	
	

}
