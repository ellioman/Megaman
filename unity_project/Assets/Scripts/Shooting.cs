using UnityEngine;
using System.Collections;

public class Shooting : MonoBehaviour
{
	// Unity Editor Variables
	public Rigidbody m_shotRigidBody;
	
	// Properties
	public bool CanShoot 	{ get; set; }
	public bool IsShooting 	{ get; set; }
	
	// Private Instance Variables
	private Vector3 m_shotPos;
	private float m_shotSpeed = 20f;
	private float m_delayBetweenShots = 0.2f;
	private float m_shootingTimer;
	
	/**/
	public void Reset()
	{
		CanShoot = true;
		IsShooting = false;
	}
	
	/* Use this for initialization */
	void Start ()
	{
		CanShoot = true;
		IsShooting = false;
	}
	
	/**/
	public void Shoot( bool isTurningLeft )
	{
		IsShooting = true;
		m_shootingTimer = Time.time;
		m_shotPos = transform.position + transform.right * ( ( isTurningLeft == true) ? -1.6f : 1.6f );
		
		Rigidbody rocketClone = (Rigidbody) Instantiate(m_shotRigidBody, m_shotPos, transform.rotation);
		rocketClone.transform.Rotate(90,0,0);
		Physics.IgnoreCollision(rocketClone.GetComponent<Collider>(), GetComponent<Collider>());
		
		Shot s = rocketClone.GetComponent<Shot>();
		s.VelocityDirection = ( isTurningLeft == true) ? -transform.right : transform.right;
		s.ShotSpeed = m_shotSpeed;
	}
	
	/* Update is called once per frame */
	void Update ()
	{
		if ( IsShooting == true )
		{
			if ( Time.time - m_shootingTimer >= m_delayBetweenShots )
			{
				IsShooting = false;	
			}
		}
	}
}

