using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour 
{
	// Properties
	public Texture2D EmptyTex 	{ get; set; }
    public Texture2D FullTex 	{ get; set; }
	public bool ShowHealthBar 	{ get; set; }
	public Vector2 Position 	{ get; set; }
	public float HealthStatus 	{ get; set; }
    
	// Private Instance Variables
	private Vector2 m_size;
    	
	void OnGUI() 
	{
		if ( ShowHealthBar == true )
		{
			m_size = new Vector2( Screen.width / 42f, Screen.height / 6f );
			
			//draw the background:
			GUI.BeginGroup(new Rect(Position.x, Position.y, m_size.x, m_size.y));
			{
				GUIStyle gg = new GUIStyle();
				
				GUI.Box(new Rect(0,0, m_size.x, m_size.y), FullTex, gg);
				
				//draw the filled-in part:
				GUI.BeginGroup(new Rect(0,0, m_size.x, m_size.y - m_size.y * HealthStatus));
				{
					GUI.Box(new Rect(0,0, m_size.x, m_size.y), EmptyTex, gg);
				}
	         	GUI.EndGroup();
			}
			GUI.EndGroup();
		}
    }
}
