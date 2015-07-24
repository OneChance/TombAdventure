using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameUtil{

	/*
	 * 目标闪动，选中效果
	 */
	public static void Focus (GameObject go)
	{
		float lerp = Mathf.PingPong (Time.time, 0.5f) * 2f;  
		
		Color c = go.GetComponent<Image> ().color;
		Color fromC = new Color (c.r, c.g, c.b, 1f);
		Color toC = new Color (c.r, c.g, c.b, 0.3f);
		
		go.GetComponent<Image> ().color = Color.Lerp (fromC, toC, lerp);
	}

	public static void UnFocus (List<GameObject> goList)
	{
		for(int i=0;i<goList.Count;i++){
			Color c = goList[i].GetComponent<Image> ().color;
			goList[i].GetComponent<Image> ().color = new Color (c.r, c.g, c.b, 1f);
		}
		goList.Clear();
	}
}
