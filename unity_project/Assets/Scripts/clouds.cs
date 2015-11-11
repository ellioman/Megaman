using UnityEngine;
using System.Collections;

public class clouds : MonoBehaviour 
{	
	#region Variables

	// Protected Instance Variables
	protected Vector3 initialScale = Vector3.one;
	protected float cloudSpeed = 0.3f;
	protected Vector2 scaleAmount = new Vector2(0.3f, 0.3f);

	#endregion


	#region MonoBehaviour

	/* Use this for initialization */
	protected void Start()
	{
		initialScale = transform.localScale;
		cloudSpeed = 0.3f;
		
		if (name == "Cloud") { scaleAmount = new Vector2(0.3f, 0.3f); }
		else if (name == "TransparentCloud1") { scaleAmount = new Vector2(0.5f, 0.5f); }
		else if (name == "TransparentCloud2") { scaleAmount = new Vector2(0.5f, 0.5f); }
	}
	
	/* Update is called once per frame */
	protected void Update() 
	{
		// If the scale amount isn't zero, animate the cloud...
		if (scaleAmount.x > 0.0f && scaleAmount.y > 0.0f)
		{
			float scaleStatus = Time.time * cloudSpeed;
			transform.localScale = 
				new Vector3(
	                    initialScale.x + Mathf.PingPong(scaleStatus, scaleAmount.x), 
						initialScale.y + Mathf.PingPong(scaleStatus, scaleAmount.y), 
						initialScale.z);
		}
	}

	#endregion
}
