using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
	// Unity Editor Variables
	public Texture2D m_emptyTex;
	public Texture2D m_fullTex;
	
	// Properties
	public float MaximumHealth{ get; set; }
	public float HurtingTimer{ get; set; }
	public float HurtingDelay{ get; set; }
	public bool IsHurting { get; set; }
	public bool IsDead { get; set; }
	public bool ShowHealthBar { get { return m_healthbar.ShowHealthBar; } set { m_healthbar.ShowHealthBar = value; } }
	public Vector2 HealthbarPosition {get{return m_healthbar.Position;} set{m_healthbar.Position = value;} }
	public float CurrentHealth { 
		get{return m_currentHealth;} 
		set { 
			if ( value > MaximumHealth ) { m_currentHealth = MaximumHealth; }
			else if ( value < 0.0f ) { m_currentHealth = 0.0f; }
			else if ( value <= MaximumHealth && value >= 0.0f ) { m_currentHealth = value; }
			m_healthbar.HealthStatus = m_currentHealth / MaximumHealth;
		} 
	}
	
	// Private Instance Variables
	private HealthBar m_healthbar;
	private float m_beginHealth = 100f;
	private float m_currentHealth;
	
	/**/
	public void Reset()
	{
		IsHurting = false;
		IsDead = false;
		MaximumHealth = 100.0f;
		HurtingDelay = 1.0f;
		
		m_currentHealth = m_beginHealth;
		m_healthbar.HealthStatus = m_beginHealth / MaximumHealth;
	}
	
	/* Constructor */
	void Awake ()
	{
		m_healthbar = gameObject.AddComponent<HealthBar>();
		m_healthbar.ShowHealthBar = false;
	}
	
	/* Use this for initialization */
	void Start ()
	{
		IsHurting = false;
		IsDead = false;
		MaximumHealth = 100.0f;
		HurtingDelay = 1.0f;
		
		m_currentHealth = m_beginHealth;
		m_healthbar.HealthStatus = m_beginHealth / MaximumHealth;
		m_healthbar.EmptyTex = m_emptyTex;
		m_healthbar.FullTex = m_fullTex;
	}
	
	/**/
	public void ChangeHealth( float healthChange )
	{
		IsHurting = true;
		HurtingTimer = Time.time;
		m_currentHealth += healthChange;
		m_healthbar.HealthStatus = m_currentHealth / MaximumHealth;
			
		if ( m_currentHealth <= 0.0f )
		{
			IsDead = true;
		}		
	}
	
//	/* Update is called once per frame */
//	void Update ()
//	{
//		// If the object is hurting...
//		if ( health.isHurting )
//		{
//			// See if it's time to stop...
//			if ( Time.time - health.hurtingTimer >= health.hurtingDelay )
//			{
//				movement.isHurting = false;
//				health.isHurting = false;	
//			}
//		}
//	}
}