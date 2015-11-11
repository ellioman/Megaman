using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;
using System.Collections.Generic;

public class AirmanWind : MonoBehaviour 
{
	#region Variables

	// Unity Editor Variables
	public List<Material> animationMaterials;
	
	// Protected Instance Variables
	protected int texIndex = 0;
	protected bool leaving = false;
	protected bool beginSequence = true;
	protected bool shouldBlowLeft = true;
	protected float texChangeInterval = 0.1f;
	protected float damage = 10.0f; 
	protected Vector2 texScale = Vector2.zero;
	protected Vector2 texScaleRight = new Vector2(1.0f, -1.0f);
	protected Vector2 texScaleLeft = new Vector2(-1.0f, -1.0f);
	protected Vector3 windPosition = Vector3.zero;
	protected Renderer rend = null;

	#endregion
	
	
	#region MonoBehaviour

	// Constructor
	protected void Awake()
	{
		rend = GetComponent<Renderer>();
		Assert.IsNotNull(rend);
	}

	// Update is called once per frame
	protected void Update () 
	{
		if (beginSequence == true)
		{
			transform.position += (windPosition - transform.position) * Time.deltaTime * 3.0f;
			
			if ((windPosition - transform.position).magnitude <= 1.0f)
			{
				beginSequence = false;
			}
		}
		
		// Update the textures...
		texIndex = (int) (Time.time / texChangeInterval);
		rend.material = animationMaterials[texIndex % (animationMaterials.Count-1)];
		rend.material.SetTextureScale("_MainTex", texScale);
		
		// If the wind is being blown away...
		if (leaving == true)
		{
			// Move the wind away...
			if (shouldBlowLeft)
			{
				transform.position -= new Vector3(20.0f * Time.deltaTime , 0f, 0f);
			}
			else
			{
				transform.position += new Vector3(20.0f * Time.deltaTime , 0f, 0f);
			}				
		}
	}

	// Called when the Collider other enters the trigger.
	protected void OnTriggerEnter(Collider other) 
	{
		if (other.tag == "Player")
		{
			other.gameObject.SendMessage("TakeDamage", damage);
		}
		else if (other.tag == "shot")
		{
			if (shouldBlowLeft == true)
			{
				other.GetComponent<Shot>().VelocityDirection = new Vector3(-1f, 1f, 0f);
			}
			else
			{
				other.GetComponent<Shot>().VelocityDirection = new Vector3(1f, 1f, 0f);
			}
		}
	}

	#endregion
	
	
	#region Protected Functions

	// 
	protected void SetPosition(Vector3 pos)
	{
		windPosition = pos;
		shouldBlowLeft = (pos.x - transform.position.x < 0.0f);
		texScale = (shouldBlowLeft) ? texScaleLeft : texScaleRight;
	}

	#endregion
	
	
	#region Public Functions

	//
	public void GoAway()
	{
		leaving = true;
		GetComponent<Collider>().isTrigger = true;
	}

	#endregion
}