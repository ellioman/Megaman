using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour 
{
	#region Variables

	// Properties
	public Texture2D EmptyTex 	{ get; set; }
    public Texture2D FullTex 	{ get; set; }
	public bool ShowHealthBar 	{ get; set; }
	public Vector2 Position 	{ get; set; }
	public float HealthStatus 	{ get; set; }
    
	// Protected Instance Variables
	protected Vector2 size = Vector2.one;
    
	#endregion
	
	
	#region MonoBehaviour

	protected void OnGUI() 
	{
		if (ShowHealthBar == true)
		{
			size = new Vector2(Screen.width / 42f, Screen.height / 6f);
			
			//draw the background:
			GUI.BeginGroup(new Rect(Position.x, Position.y, size.x, size.y));
			{
				GUIStyle gg = new GUIStyle();
				
				GUI.Box(new Rect(0,0, size.x, size.y), FullTex, gg);
				
				//draw the filled-in part:
				GUI.BeginGroup(new Rect(0,0, size.x, size.y - size.y * HealthStatus));
				{
					GUI.Box(new Rect(0,0, size.x, size.y), EmptyTex, gg);
				}
	         	GUI.EndGroup();
			}
			GUI.EndGroup();
		}
    }

	#endregion
}
