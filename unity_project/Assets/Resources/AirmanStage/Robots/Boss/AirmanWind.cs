using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AirmanWind : MonoBehaviour 
{
	// Unity Editor Variables
	public List<Material> m_materials;
	
	// Private Instance Variables
	private bool m_leaving = false;
	private bool m_beginSequence = true;
	private bool m_shouldBlowLeft = true;
	private float m_texChangeInterval = 0.1f;
	private float m_damage = 10.0f; 
	private int m_texIndex = 0;
	private Vector2 m_texScale;
	private Vector2 m_texScaleRight = new Vector2(1.0f, -1.0f);
	private Vector2 m_texScaleLeft = new Vector2(-1.0f, -1.0f);
	private Vector3 m_windPosition;
	
	/**/
	void OnTriggerEnter(Collider other) 
	{
		if ( other.tag == "Player" )
		{
			other.gameObject.SendMessage("TakeDamage", this.m_damage );
		}
		else if ( other.tag == "shot" )
		{
			if ( this.m_shouldBlowLeft == true )
			{
				other.GetComponent<Shot>().VelocityDirection = new Vector3( -1f, 1f, 0f );
			}
			else
			{
				other.GetComponent<Shot>().VelocityDirection = new Vector3( 1f, 1f, 0f );
			}
		}
	}
	
	/**/
	void SetPosition( Vector3 pos )
	{
		this.m_windPosition = pos;
		this.m_shouldBlowLeft = (pos.x - transform.position.x < 0.0f );
		this.m_texScale = (this.m_shouldBlowLeft) ? m_texScaleLeft : m_texScaleRight;
	}
	
	/**/
	void GoAway()
	{
		this.m_leaving = true;
		this.collider.isTrigger = true;
	}
	
	/* Use this for initialization */
	void Start () 
	{
		
	}
	
	/* Update is called once per frame */
	void Update () 
	{
		if ( this.m_beginSequence == true )
		{
			transform.position += (this.m_windPosition - transform.position) * Time.deltaTime * 3.0f;
			
			if ( (this.m_windPosition - transform.position).magnitude <= 1.0f )
			{
				this.m_beginSequence = false;
			}
		}
		
		// Update the textures...
		this.m_texIndex = (int) (Time.time / m_texChangeInterval);
		renderer.material = m_materials[m_texIndex % (m_materials.Count-1)];
		renderer.material.SetTextureScale("_MainTex", m_texScale);
		
		// If the wind is being blown away...
		if ( this.m_leaving == true )
		{
			// Move the wind away...
			if ( this.m_shouldBlowLeft )
			{
				transform.position -= new Vector3(20.0f * Time.deltaTime , 0f, 0f );
			}
			else
			{
				transform.position += new Vector3(20.0f * Time.deltaTime , 0f, 0f );
			}				
		}
	}
}