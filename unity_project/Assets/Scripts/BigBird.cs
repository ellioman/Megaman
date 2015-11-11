using UnityEngine;
using System.Collections;

public class BigBird : MonoBehaviour 
{
	#region Variables

	// Protected Instance Variables
	protected Egg egg;
	protected bool moving = false;
	protected bool attacking = false;
	protected float speed = 10.0f;
	protected float lifeSpan = 5.0f;
	protected float lifeTimer;
	protected float damage = 20.0f;

	#endregion


	#region MonoBehaviour

	// Constructor 
	protected void Awake ()
	{
		egg = gameObject.GetComponentInChildren<Egg>();
	}

	// Update is called once per frame
	protected void Update () 
	{
		if (moving == true)
		{
			transform.position += (-Vector3.right * speed * Time.deltaTime);
			
			if (Time.time - lifeTimer >= lifeSpan)
			{
				Destroy (gameObject);	
			}
		}
		
		if (attacking == true)
		{
			if (Mathf.Abs(GameEngine.Player.transform.position.x - transform.position.x) <= 10.0f)
			{
				egg.ReleaseEgg(speed);
				attacking = false;
			}
		}
	}
	
	//
	protected void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			GameEngine.Player.TakeDamage(damage);
		}
	}

	#endregion


	#region Public Functions
	
	//
	public void TakeDamage(float dam)
	{
		GameEngine.SoundManager.Play(AirmanLevelSounds.BOSS_HURTING);
		Destroy (gameObject);
	}
	
	// 
	public void Attack()
	{
		lifeTimer = Time.time;
		moving = true;
		attacking = true;
	}

	//
	public void Reset()
	{

	}
	
	#endregion
}
