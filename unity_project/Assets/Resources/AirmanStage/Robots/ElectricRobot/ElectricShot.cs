using UnityEngine;
using System.Collections;

public class ElectricShot : MonoBehaviour
{
	// Properties
	public Vector3 TargetDirection { 
		get { return m_targetDirection;}
		set {
			m_targetDirection = value - transform.position;
			m_targetDirection.Normalize();
			SetTextureScale();
		}
	}
	
	// Private Instance Variables
	private Vector3 m_targetDirection;
	private float m_lifeSpan = 3f;
	private float m_damage = 10f;
	private float m_speed = 150f;
	private float m_timeStart;
	private Vector2 m_texScaleRightDown = new Vector2(1.0f, -1.0f);
	private Vector2 m_texScaleLeftDown = new Vector2(-1.0f, -1.0f);
	private Vector2 m_texScaleRightUp = new Vector2(1.0f, 1.0f);
	private Vector2 m_texScaleLeftUp = new Vector2(-1.0f, 1.0f);
	
	/**/
	void SetTextureScale()
	{
		// Left?
		if ( m_targetDirection.x <= 0.0f )
		{
			if ( m_targetDirection.y <= 0.0f )
			{
				GetComponent<Renderer>().material.SetTextureScale("_MainTex", m_texScaleLeftDown);
			}
			else 
			{
				GetComponent<Renderer>().material.SetTextureScale("_MainTex", m_texScaleLeftUp);
			}
		}
		// Right...
		else
		{
			if ( m_targetDirection.y <= 0.0f )
			{
				GetComponent<Renderer>().material.SetTextureScale("_MainTex", m_texScaleRightDown);
			}
			else {
				GetComponent<Renderer>().material.SetTextureScale("_MainTex", m_texScaleRightUp);
			}
		}
	}
	
	/**/
	void InflictDamage( GameObject objectHit )
	{
		if ( objectHit.tag == "Player" )
		{
			objectHit.GetComponent<Player>().TakeDamage( m_damage );
			transform.parent.gameObject.GetComponent<ElectricRobot>().SetIsShooting( false );
			Destroy(gameObject);
		}
	}
	
	/**/
	void OnTriggerEnter(Collider other) 
	{
		InflictDamage( other.gameObject );
	}
	
	/**/
	void OnCollisionEnter( Collision collision ) 
	{
		InflictDamage( collision.gameObject );
	}
	
	/* Use this for initialization */
	void Start ()
	{
		m_timeStart = Time.time;
	}
	
	/* Update is called once per frame */
	void Update ()
	{
		GetComponent<Rigidbody>().velocity = m_targetDirection * m_speed * Time.deltaTime;
		
		if ( Time.time - m_timeStart >= m_lifeSpan )
		{
			transform.parent.gameObject.SendMessage("SetIsShooting", false);
			Destroy(gameObject);
		}
	}
}

