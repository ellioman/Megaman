using UnityEngine;
using System;
using System.Collections;

public class GameEngine
{
	public static Player Player { get; set; }
	public static SoundManager SoundManager { get; set; }
	public static AirmanBoss AirMan { get; set; }

	protected static event Action ResetCallbackList;


	public static void AddResetCallback(Action resetCallback)
	{
		ResetCallbackList += resetCallback;
	}
	
	public static void RemoveResetCallback(Action resetCallback)
	{
		ResetCallbackList -= resetCallback;
	}
}
