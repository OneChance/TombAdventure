﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class UI_Create : MonoBehaviour
{

	private Transform canvas;
	private GlobalData gData;

	void Start ()
	{
		gData = GameObject.FindGameObjectWithTag ("GlobalData").GetComponent<GlobalData> ();
		canvas = GameObject.FindGameObjectWithTag ("UI").transform;
		//初始化文本
		canvas.FindChild ("Geomancer").FindChild ("Text").GetComponent<Text> ().text = StringCollection.GEOMANCER;
		canvas.FindChild ("Settler").FindChild ("Text").GetComponent<Text> ().text = StringCollection.SETTLER;
		canvas.FindChild ("Exorcist").FindChild ("Text").GetComponent<Text> ().text = StringCollection.EXORCIST;
		canvas.FindChild ("Doctor").FindChild ("Text").GetComponent<Text> ().text = StringCollection.DOCTOR;
		canvas.FindChild ("Create").FindChild ("Text").GetComponent<Text> ().text = StringCollection.CREATE;

		//初始化图片
		canvas.FindChild ("Geomancer").GetComponent<Image> ().sprite = Resources.Load<Sprite> ("_images/_game/Geomancer_1");
		canvas.FindChild ("Settler").GetComponent<Image> ().sprite = Resources.Load<Sprite> ("_images/_game/Settler_1");
		canvas.FindChild ("Exorcist").GetComponent<Image> ().sprite = Resources.Load<Sprite> ("_images/_game/Exorcist_1");
		canvas.FindChild ("Doctor").GetComponent<Image> ().sprite = Resources.Load<Sprite> ("_images/_game/Doctor_1");
	}

	public void Choose (Button button)
	{
		canvas.transform.FindChild ("Choose").GetComponent<Image> ().sprite = button.GetComponent<Image> ().sprite;
		canvas.transform.FindChild ("Proname").GetComponent<Text> ().text = button.transform.FindChild ("Text").GetComponent<Text> ().text;
	}

	public void Create ()
	{
		InputField name = canvas.FindChild ("Name").GetComponent<InputField> ();
		if (name.text.Trim ().Equals ("")) {
			ShowHint.Hint (StringCollection.NEEDCHARNAME);
			return;
		}

		if (canvas.transform.FindChild ("Choose").GetComponent<Image> ().sprite.name.Equals ("UISprite")) {
			ShowHint.Hint (StringCollection.NEEDPRO);
			return;
		}

		gData.isPlayer = false; //初始的时候总是佣兵模式，玩家在线后与其他玩家组队，才会变成联机模式
		gData.characterList = new List<Character> ();

		List<Equipment> eList = new List<Equipment> ();
		
		Character c = new Character (5000, 0, 0, 0, 0, name.text.Trim (), true, 0, 0, ProFactory.getPro (canvas.transform.FindChild ("Choose").GetComponent<Image> ().sprite.name.Split (new char[]{'_'}) [0], "1"), 1, 0, eList, -1);
		c.Health = c.MaxHealth;
		c.Stamina = c.maxStamina;

		List<Baggrid> bgList = new List<Baggrid> ();
		
		c.BgList = bgList;				
		gData.characterList.Add (c);				
		
		Application.LoadLevel ("city");

	}
}
