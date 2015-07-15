using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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
}
