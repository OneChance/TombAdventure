using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using KBEngine;
using System;

public class UI_Create : MonoBehaviour
{

	private Transform canvas;
	private GlobalData gData;

	void Start ()
	{
		gData = UnityEngine.GameObject.FindGameObjectWithTag ("GlobalData").GetComponent<GlobalData> ();
		canvas = UnityEngine.GameObject.FindGameObjectWithTag ("UI").transform;
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

		string proname = canvas.transform.FindChild ("Choose").GetComponent<Image> ().sprite.name.Split (new char[]{'_'}) [0];

		Account account = (Account)KBEngineApp.app.player();

		account.reqCreateRole (name.text.Trim (), proname);

	}

	public void OnCreateRole(Dictionary<string, object> role){

		gData.isPlayer = false; //初始的时候总是佣兵模式，玩家在线后与其他玩家组队，才会变成联机模式
		gData.characterList = new List<Character> ();

		gData.characterList = DataHelper.GetCharacterFromServer(role,gData.siList);

		Application.LoadLevel ("city");
	}
}
