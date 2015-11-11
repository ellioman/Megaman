using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

public class AirmanTrigger : MonoBehaviour
{
	#region Variables
	
	// Protected Instance Variables
	protected AirmanBoss airman;
	protected Collider col;

	#endregion


	#region MonoBehaviour
	
	// Constructor
	protected void Awake()
	{
		airman = FindObjectOfType<AirmanBoss>();
		Assert.IsNotNull(airman);

		col = GetComponent<Collider>();
		Assert.IsNotNull(col);
	}
	
	// Use this for initialization
	protected void Start()
	{
		airman.gameObject.SetActive(false);
	}

	// Called when the Collider other enters the trigger.
	protected void OnTriggerEnter(Collider other) 
	{
		airman.gameObject.SetActive(true);
		airman.SetUpAirman();
		col.enabled = false;
	}

	#endregion
}

