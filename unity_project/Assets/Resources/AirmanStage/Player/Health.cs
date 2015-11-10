using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
	#region Variables
	
	// Unity Editor Variables
	public Texture2D emptyTex;
	public Texture2D fullTex;
	
	// Public Properties
	public float MaximumHealth { get; set; }
	public float HurtingTimer { get; set; }
	public float HurtingDelay { get; set; }
	public bool IsHurting { get; set; }
	public bool IsDead { get; set; }
	public bool ShowHealthBar { get { return healthbar.ShowHealthBar; } set { healthbar.ShowHealthBar = value; } }
	public Vector2 HealthbarPosition { get { return healthbar.Position; } set { healthbar.Position = value; } }
	public float CurrentHealth
	{ 
		get
		{
			return currentHealth;
		} 
		set
		{ 
			if (value > MaximumHealth) { currentHealth = MaximumHealth; }
			else if (value < 0.0f) { currentHealth = 0.0f; }
			else if (value <= MaximumHealth && value >= 0.0f) { currentHealth = value; }
			healthbar.HealthStatus = currentHealth / MaximumHealth;
		} 
	}
	
	// Protected Instance Variables
	protected HealthBar healthbar = null;
	protected float startHealth = 100f;
	protected float currentHealth = 100f;

	#endregion


	#region MonoBehaviour

	// Constructor
	protected void Awake ()
	{
		healthbar = gameObject.AddComponent<HealthBar>();
		healthbar.ShowHealthBar = false;
	}
	
	// Use this for initialization
	protected void Start ()
	{
		IsHurting = false;
		IsDead = false;
		MaximumHealth = 100.0f;
		HurtingDelay = 1.0f;
		
		currentHealth = startHealth;
		healthbar.HealthStatus = startHealth / MaximumHealth;
		healthbar.EmptyTex = emptyTex;
		healthbar.FullTex = fullTex;
	}

	#endregion
	
	
	#region Public Functions

	//
	public void Reset()
	{
		IsHurting = false;
		IsDead = false;
		MaximumHealth = 100.0f;
		HurtingDelay = 1.0f;
		
		currentHealth = startHealth;
		healthbar.HealthStatus = startHealth / MaximumHealth;
	}
	
	// 
	public void ChangeHealth(float healthChange)
	{
		IsHurting = true;
		HurtingTimer = Time.time;
		currentHealth += healthChange;
		healthbar.HealthStatus = currentHealth / MaximumHealth;
			
		if (currentHealth <= 0.0f)
		{
			IsDead = true;
		}		
	}

	#endregion
}