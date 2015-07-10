using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAction : MonoBehaviour
{

	private GlobalData gData;
	public List<Character> characterList;
	private SceneGen sceneGen;

	void Start ()
	{
		gData = GameObject.FindGameObjectWithTag ("GlobalData").GetComponent<GlobalData> ();
		sceneGen = GameObject.FindGameObjectWithTag ("GameController").GetComponent<SceneGen>();
		//init data from server
		//character & item
		characterList = new List<Character> ();

		if (gData.characterList == null || gData.characterList.Count == 0) {

			Geomancer geo = new Geomancer();
			Character c = new Character (100, 50, 0, 0, "zhouhui", gameObject.name, false,100,geo);
			
			HealthItem item = new HealthItem (Item.RangeType.SINGLE, 10, "1", "单体治疗药剂");
			List<Baggrid> bgList = new List<Baggrid> ();
			Baggrid bg = new Baggrid (item, 2);
			bgList.Add (bg);
			c.BgList = bgList;
			characterList.Add (c);

			Settler settler = new Settler();
			Character c2 = new Character (100, 50, 0, 0, "unity", gameObject.name, false,100,settler);
			characterList.Add (c2);
		} else {
			characterList = gData.characterList;
		}
	}

	void OnCollisionEnter2D (Collision2D coll)
	{
		if (coll.gameObject.tag == "Enemy") {
			gData.currentEnemy = coll.gameObject.GetComponent<EnemyAI> ().Enemy;
			gData.currentEnemyName = coll.gameObject.name;
			gData.characterList = characterList;
			gData.playerPos = transform.position;
			//tell gdata to record current enemies'pos
			sceneGen.SendMessage("RecScene");
			DontDestroyOnLoad (gData);
			Application.LoadLevel ("battle");
		}
	}
}
