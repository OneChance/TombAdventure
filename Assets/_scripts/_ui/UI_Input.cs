﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class UI_Input : MonoBehaviour
{
	public Transform player;
	public GameObject dirButtons;
	public GameObject actButtons;
	private GlobalData gData;
	
	void Start ()
	{

		gData = GameObject.FindGameObjectWithTag ("GlobalData").GetComponent<GlobalData> ();

		//加载按钮文本
		Transform actbutton = actButtons.transform;
		actbutton.FindChild ("Bag").FindChild ("Text").GetComponent<Text> ().text = StringCollection.BAG;
		actbutton.FindChild ("Equip").FindChild ("Text").GetComponent<Text> ().text = StringCollection.EQUIPMENT;
		actbutton.FindChild ("DigStop").FindChild ("Text").GetComponent<Text> ().text = StringCollection.STOPDIG;
		actbutton.FindChild ("Dig").FindChild ("Text").GetComponent<Text> ().text = StringCollection.DIG;
		actbutton.FindChild ("Detect").FindChild ("Text").GetComponent<Text> ().text = StringCollection.DETECT;
	}

	public void left ()
	{
		player.SendMessage ("PlayerMove", PlayerAction.MOVEDIRECTION.LEFT);
	}

	public void right ()
	{
		player.SendMessage ("PlayerMove", PlayerAction.MOVEDIRECTION.RIGHT);
	}

	public void up ()
	{
		player.SendMessage ("PlayerMove", PlayerAction.MOVEDIRECTION.UP);
	}

	public void down ()
	{
		player.SendMessage ("PlayerMove", PlayerAction.MOVEDIRECTION.DOWN);
	}

	public void Detect ()
	{
		if (gData.currentTomb.sceneList [gData.currentFloor - 1].isTomb) {
			ShowHint.Hint (StringCollection.ISTOMB);
		} else {
			player.SendMessage ("PlayerDetect");
		}
	}

	public void Dig ()
	{
		if (gData.currentTomb.sceneList [gData.currentFloor - 1].isTomb) {
			ShowHint.Hint (StringCollection.ISTOMB);
		} else {

			gameObject.SendMessage ("closeAllBag");
			
			//隐藏UI上除了停止按钮意外的其他元素
			dirButtons.SetActive (false);
			for (int i=0; i<actButtons.transform.childCount; i++) {
				if (actButtons.transform.GetChild (i).name != "DigStop") {
					actButtons.transform.GetChild (i).gameObject.SetActive (false);
				} else {
					actButtons.transform.GetChild (i).gameObject.SetActive (true);
				}
			}
			
			player.SendMessage ("PlayerDig");
		}
	}

	public void DigStop ()
	{
		//隐藏停止,显示UI上除了停止按钮意外的其他元素
		dirButtons.SetActive (true);
		for (int i=0; i<actButtons.transform.childCount; i++) {
			if (actButtons.transform.GetChild (i).name != "DigStop") {
				actButtons.transform.GetChild (i).gameObject.SetActive (true);
			} else {
				actButtons.transform.GetChild (i).gameObject.SetActive (false);
			}
		}
		player.SendMessage ("StopDig");
	}
}
