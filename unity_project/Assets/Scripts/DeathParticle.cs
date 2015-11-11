using UnityEngine;
using System.Collections;

public class DeathParticle : MonoBehaviour 
{
	#region Variables

	// Protected Instance Variables
	protected float lifeSpan = 2.6f;
	protected float scaleSpeed = 0.5f;
	protected float timeStart;
	protected Vector3 initialScale = Vector3.one;
	protected Vector2 scaleAmount = new Vector2(.75f, .75f);

	#endregion
	
	
	#region MonoBehaviour

	// Use this for initialization
	protected void Start()
	{
		this.timeStart = Time.time;
		this.initialScale = transform.localScale;
	}
	
	// Update is called once per frame
	protected void Update() 
	{
		// If the scale amount isn't zero, animate the cloud...
		if (scaleAmount.x > 0.0f && scaleAmount.y > 0.0f)
		{
			float scaleStatus = Time.time * this.scaleSpeed;
			transform.localScale = new Vector3(
	                    this.initialScale.x + Mathf.PingPong(scaleStatus, scaleAmount.x), 
						this.initialScale.y,
						this.initialScale.z + Mathf.PingPong(scaleStatus, scaleAmount.y));
			transform.Rotate(new Vector3(0, Time.time * Time.deltaTime, 0));
		}
		
		if (Time.time - this.timeStart >= lifeSpan)
		{
			Destroy(this.gameObject);
		}
	}

	#endregion
}
