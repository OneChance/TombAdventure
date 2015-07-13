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

			Character c = new Character (100, 50, 0, 0, "zhouhui", false,100,ProFactory.getPro("Geomancer","1"));
			
			HealthItem item = new HealthItem (Item.RangeType.SINGLE, 10, "1", "单体治疗药剂");
			List<Baggrid> bgList = new List<Baggrid> ();
			Baggrid bg = new Baggrid (item, 2);
			bgList.Add (bg);
			c.BgList = bgList;
			characterList.Add (c);

			Character c2 = new Character (100, 50, 0, 0, "unity", false,100,ProFactory.getPro("Settler","1"));
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

	public void PlayerMove(){
		//队长移动一次，减少一个体能；成员移动三次，减少一个体能
		for (int i=0; i<characterList.Count; i++) {
			
		}
	}
}
