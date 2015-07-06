using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAction : MonoBehaviour
{

	private GlobalData gData;
	private List<Character> characterList;
	private SceneGen sceneGen;

	void Start ()
	{
		gData = GameObject.FindGameObjectWithTag ("GlobalData").GetComponent<GlobalData> ();
		sceneGen = GameObject.FindGameObjectWithTag ("GameController").GetComponent<SceneGen>();
		//init data from server
		//character & item
		characterList = new List<Character> ();
		Character c = new Character (100, 50, 0, 0, "zhouhui", gameObject.name);

		HealthItem item = new HealthItem (Item.RangeType.SINGLE,10,"1","单体治疗药剂");
		List<Baggrid> bgList = new List<Baggrid> ();
		Baggrid bg = new Baggrid (item,2);
		bgList.Add (bg);
		c.BgList = bgList;
		characterList.Add (c);


		Character c2 = new Character (100, 50, 0, 0, "unity", gameObject.name);
		List<Baggrid> bgList2 = new List<Baggrid> ();
		c2.BgList = bgList2;
		characterList.Add (c2);


	}

	void OnCollisionEnter2D (Collision2D coll)
	{
		if (coll.gameObject.tag == "Enemy") {
			gData.currentEnemy = coll.gameObject.GetComponent<EnemyAI> ().Enemy;
			gData.characterList = characterList;
			//tell gdata to record current enemies'pos
			sceneGen.SendMessage("RecScene");
			DontDestroyOnLoad (gData);
			Application.LoadLevel ("battle");
		}
	}
}
