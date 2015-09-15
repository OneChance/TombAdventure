using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DigInfo : MonoBehaviour
{
	public int deep;
	public int currentDeep;
	public int texType;
	public int dbid;

	public static void updateDigInDigList (DigData digData, List<GameObject> digList)
	{
		for (int i=0; i<digList.Count; i++) {
			DigInfo digInfo = digList [i].GetComponent<DigInfo> ();

			if (digInfo.dbid == digData.dbid) {

				digInfo.deep = digData.deep;
				digInfo.currentDeep = digData.currentDeep;
				digInfo.texType = digData.texType;

				digList [i].GetComponent<SpriteRenderer> ().sprite = Resources.Load <Sprite> ("_images/_game/dig_" + Mathf.Min (2, digInfo.texType));
			}
		}
	} 
}
