using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GlobalData : MonoBehaviour
{
	public int currentFloor = 0;
	public List<SceneInfo> scenes;

	void Awake ()
	{
		scenes = new List<SceneInfo> ();
	}
}
