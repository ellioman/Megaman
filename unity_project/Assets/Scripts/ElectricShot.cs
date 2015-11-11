using UnityEngine;
using System.Collections;

public class ElectricShot : MonoBehaviour
{
	#region Variables

	// Public Properties
	public Vector3 TargetDirection
	{ 
		get { return targetDirection; }
		set 
		{
			targetDirection = value - transform.position;
			targetDirection.Normalize();
			SetTextureScale();
		}
	}
	
	// Protected Instance Variables
	protected Vector3 targetDirection;
	protected float lifeSpan = 3f;
	protected float damage = 10f;
	protected float speed = 150f;
	protected float timeStart;
	protected Vector2 texScaleRightDown = new Vector2(1.0f, -1.0f);
	protected Vector2 texScaleLeftDown = new Vector2(-1.0f, -1.0f);
	protected Vector2 texScaleRightUp = new Vector2(1.0f, 1.0f);
	protected Vector2 texScaleLeftUp = new Vector2(-1.0f, 1.0f);
	
	#endregion
	
	
	#region MonoBehaviour

	/* Use this for initialization */
	protected void Start ()
	{
		timeStart = Time.time;
	}
	
	/* Update is called once per frame */
	protected void Update ()
	{
		GetComponent<Rigidbody>().velocity = targetDirection * speed * Time.deltaTime;
		
		if (Time.time - timeStart >= lifeSpan)
		{
			transform.parent.gameObject.SendMessage("SetIsShooting", false);
			Destroy(gameObject);
		}
	}
	
	// Called when the Collider other enters the trigger.
	protected void OnTriggerEnter(Collider other) 
	{
		InflictDamage(other.gameObject);
	}
	
	// 
	protected void OnCollisionEnter(Collision collision) 
	{
		InflictDamage(collision.gameObject);
	}

	#endregion


	#region Protected Functions

	// 
	protected void SetTextureScale()
	{
		// Left?
		if (targetDirection.x <= 0.0f)
		{
			if (targetDirection.y <= 0.0f)
			{
				GetComponent<Renderer>().material.SetTextureScale("_MainTex", texScaleLeftDown);
			}
			else 
			{
				GetComponent<Renderer>().material.SetTextureScale("_MainTex", texScaleLeftUp);
			}
		}
		// Right...
		else
		{
			if (targetDirection.y <= 0.0f)
			{
				GetComponent<Renderer>().material.SetTextureScale("_MainTex", texScaleRightDown);
			}
			else
			{
				GetComponent<Renderer>().material.SetTextureScale("_MainTex", texScaleRightUp);
			}
		}
	}
	
	// 
	protected void InflictDamage(GameObject objectHit)
	{
		if (objectHit.tag == "Player")
		{
			GameEngine.Player.TakeDamage(damage);
			transform.parent.gameObject.GetComponent<ElectricRobot>().SetIsShooting(false);
			Destroy(gameObject);
		}
	}

	#endregion
}

