using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAction : MonoBehaviour
{

	private GlobalData gData;
	public List<Character> characterList;
	private SceneGen sceneGen;
	private int stepCounter;
	public float moveDistance = 0.5f;

	public enum MOVEDIRECTION{
		LEFT,
		RIGHT,
		UP,
		DOWN
	};

	void Start ()
	{
		stepCounter = 0;
		gData = GameObject.FindGameObjectWithTag ("GlobalData").GetComponent<GlobalData> ();
		sceneGen = GameObject.FindGameObjectWithTag ("GameController").GetComponent<SceneGen>();
		//init data from server
		//character & item
		characterList = new List<Character> ();

		if (gData.characterList == null || gData.characterList.Count == 0) {

			Character c = new Character (30,100, 50, 0, 0, "zhouhui", false,100,100,ProFactory.getPro("Geomancer","1"));
			
			HealthItem item = new HealthItem (Item.RangeType.SINGLE, 10, "1", "单体治疗药剂");
			List<Baggrid> bgList = new List<Baggrid> ();
			Baggrid bg = new Baggrid (item, 2);
			bgList.Add (bg);
			c.BgList = bgList;
			characterList.Add (c);

			Character c2 = new Character (40, 100,50, 0, 0, "unity", false,100,100,ProFactory.getPro("Settler","1"));
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

	//玩家移动
	public void PlayerMove(MOVEDIRECTION dir){

		if(MoveStaminaCheck()){
			switch(dir){
				case MOVEDIRECTION.LEFT:transform.Translate (Vector2.left * moveDistance);break;
				case MOVEDIRECTION.RIGHT:transform.Translate (Vector2.right * moveDistance);break;
				case MOVEDIRECTION.UP:transform.Translate (Vector2.up * moveDistance);break;
				case MOVEDIRECTION.DOWN:transform.Translate (Vector2.down * moveDistance);break;
			}
			
			stepCounter++;
			
			//队长移动一次，减少一个体能；成员移动三次，减少一个体能
			characterList[0].Stamina = Mathf.Max(0,characterList[0].Stamina-1);
			
			if(stepCounter==3){
				for (int i=1; i<characterList.Count; i++) {
					characterList[i].Stamina = Mathf.Max(0,characterList[i].Stamina-1);
				}
				stepCounter = 0;
			}
		}else{
			Debug.Log("队长已经没有体力了!");
		}
	}

	public bool MoveStaminaCheck(){
		return characterList[0].Stamina > 0 ;
	}

	//玩家挖掘
	public void PlayerDig(){
		sceneGen.SendMessage("DigInMap");
	}
}
