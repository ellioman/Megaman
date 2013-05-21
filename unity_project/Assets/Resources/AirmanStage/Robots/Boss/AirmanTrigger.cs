using UnityEngine;
using System.Collections;

public class AirmanTrigger : MonoBehaviour
{
//	public Transform boss;
	private AirmanBoss m_airman;
		
	/* */
	void OnTriggerEnter(Collider other) 
	{
		m_airman.gameObject.SetActive( true );
		m_airman.SetUpAirman();
		this.collider.enabled = false;
	}
	
	/* Constructor */
	void Awake ()
	{
		m_airman = GameObject.Find("Airman").GetComponent<AirmanBoss>();
	}
	
	/* Constructor */
	void Start ()
	{
		m_airman.gameObject.SetActive( false );
	}
}

