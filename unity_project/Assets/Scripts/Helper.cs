using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Helper : MonoBehaviour
{
	// 
	public static GameObject[] FindGameObjectsWithLayer (int layer) 
	{
		GameObject[] goArray = FindObjectsOfType(typeof(GameObject)) as GameObject[];
		List<GameObject> goList = new List<GameObject>();
		for (int i = 0; i < goArray.Length; i++) 
		{
			if (goArray[i].layer == layer) 
			{
				goList.Add(goArray[i]);
			}
		}
		if (goList.Count == 0) {
			return null;
		}
		return goList.ToArray();
	}
}