using UnityEngine;
using System.Collections;

public class clouds : MonoBehaviour 
{	
	// Private Instance Variables
	private Vector3 m_initialScale;
	private float m_cloudSpeed = 0.3f;
	private Vector2 m_scaleAmount = new Vector2( 0.3f, 0.3f );
	
	/* Use this for initialization */
	void Start () {
		m_initialScale = transform.localScale;
		m_cloudSpeed = 0.3f;
		
		if ( name == "Cloud" ) { m_scaleAmount = new Vector2( 0.3f, 0.3f ); }
		else if ( name == "TransparentCloud1" ) { m_scaleAmount = new Vector2( 0.5f, 0.5f ); }
		else if ( name == "TransparentCloud2" ) { m_scaleAmount = new Vector2( 0.5f, 0.5f ); }
	}
	
	/* Update is called once per frame */
	void Update () 
	{
		// If the scale amount isn't zero, animate the cloud...
		if ( m_scaleAmount.x > 0.0f && m_scaleAmount.y > 0.0f )
		{
			float scaleStatus = Time.time * m_cloudSpeed;
			transform.localScale = 
				new Vector3(
	                    m_initialScale.x + Mathf.PingPong(scaleStatus, m_scaleAmount.x), 
						m_initialScale.y + Mathf.PingPong(scaleStatus, m_scaleAmount.y), 
						m_initialScale.z);
		}
	}
}
