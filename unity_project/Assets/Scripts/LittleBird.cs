using UnityEngine;
using System.Collections;

public class LittleBird : MonoBehaviour 
{
	#region Variables

	// Protected Instance Variables
	protected bool attacking = false;
	protected Vector3 direction;
	protected float speed = 7.5f;
	protected float lifeSpan = 10.0f;
	protected float lifeTimer;
	protected float damage = 10.0f;

	#endregion


	#region MonoBehaviour
	
	// Called when the Collider other enters the trigger.
	protected void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			GameEngine.Player.TakeDamage(damage);
		}
	}
	
	// 
	protected void Update () 
	{
		if (attacking == true)
		{
			transform.position += (direction * speed * Time.deltaTime);
			
			if (Time.time - lifeTimer >= lifeSpan)
			{
				Destroy (gameObject);	
			}
		}
	}

	#endregion


	#region Public Functions

	// 
	public void Attack(bool goLeft, float birdSpeed)
	{
		attacking = true;
		direction = (goLeft == true) ? Vector3.left + Vector3.up * 0.15f : Vector3.right + Vector3.up * 0.15f;
		speed = birdSpeed;
		lifeTimer = Time.time;
	}
	
	// 
	public void TakeDamage(float dam)
	{
		GameEngine.SoundManager.Play(AirmanLevelSounds.BOSS_HURTING);
		Destroy (gameObject);
	}

	#endregion
}
