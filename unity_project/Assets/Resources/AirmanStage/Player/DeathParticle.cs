using UnityEngine;
using System.Collections;

public class DeathParticle : MonoBehaviour 
{
	// Private Instance Variables
	private float m_lifeSpan = 2.6f;
	private float m_scaleSpeed = 0.5f;
	private Vector2 m_scaleAmount = new Vector2( .75f, .75f );
	private float m_timeStart;
	private Vector3 m_initialScale;
	
	/* Use this for initialization */
	void Start () {
		this.m_timeStart = Time.time;
		this.m_initialScale = transform.localScale;
	}
	
	/* Update is called once per frame */
	void Update () 
	{
		// If the scale amount isn't zero, animate the cloud...
		if ( m_scaleAmount.x > 0.0f && m_scaleAmount.y > 0.0f )
		{
			float scaleStatus = Time.time * this.m_scaleSpeed;
			transform.localScale = new Vector3(
	                    this.m_initialScale.x + Mathf.PingPong(scaleStatus, m_scaleAmount.x), 
						this.m_initialScale.y,
						this.m_initialScale.z + Mathf.PingPong(scaleStatus, m_scaleAmount.y));
			transform.Rotate(new Vector3(0, Time.time * Time.deltaTime, 0));
		}
		
		
		if ( Time.time - this.m_timeStart >= m_lifeSpan )
		{
			Destroy(this.gameObject);
		}
	}
}
