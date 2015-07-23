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
	private UI_Input uiInput;
	public bool isPlayer; //ture为玩家联机模式，false为单机佣兵模式

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
		uiInput = GameObject.FindGameObjectWithTag("GameController").GetComponent<UI_Input>();

		//从服务器端获取玩家数据初始化
		isPlayer = false; //初始的时候总是佣兵模式，玩家在线后与其他玩家组队，才会变成联机模式
		characterList = new List<Character> ();

		if (gData.characterList == null || gData.characterList.Count == 0) {

			Equipment e = new Equipment(1,2,Equipment.EquipPos.BODY,"2","学者的思考",1);
			List<Equipment> eList = new List<Equipment>();
			eList.Add(e);
			
			Character c = new Character (30,100, 50, 0, 0, "zhouhui", false,200,200,ProFactory.getPro("Geomancer","1"),1,0,eList);
			
			HealthItem item = new HealthItem (Item.RangeType.SINGLE, 10, "1", "单体治疗药剂");
			List<Baggrid> bgList = new List<Baggrid> ();
			Baggrid bg = new Baggrid (item, 2);
			bgList.Add (bg);

			Equipment e2 = new Equipment(1,2,Equipment.EquipPos.BODY,"3","学者的幻想",1);
			Baggrid bg2 = new Baggrid (e2, 0);
			bgList.Add (bg2);

			c.BgList = bgList;

			characterList.Add (c);

			Character c2 = new Character (40, 100,50, 0, 0, "unity", false,100,100,ProFactory.getPro("Settler","1"),1,0,null);
			characterList.Add (c2);
			gData.characterList = characterList;
		} else {
			characterList = gData.characterList;
		}
	}

	void OnCollisionEnter2D (Collision2D coll)
	{
		if (coll.gameObject.tag.Equals("Enemy")) {
			gData.currentEnemy = coll.gameObject.GetComponent<EnemyAI> ().Enemy;
			gData.currentEnemyName = coll.gameObject.name;
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

			//通知UI更新
			uiInput.SendMessage("UpdateUIInfo");

			//队长移动一次，减少一个体能；成员移动三次，减少一个体能
			characterList[0].Stamina = Mathf.Max(0,characterList[0].Stamina-1);
			
			if(stepCounter==3){
				for (int i=1; i<characterList.Count; i++) {
					characterList[i].Stamina = Mathf.Max(0,characterList[i].Stamina-1);
				}
				stepCounter = 0;
			}
		}else{
			Debug.Log(StringCollection.LEADERNOSTAMINA);
		}
	}

	public bool MoveStaminaCheck(){
		return characterList[0].Stamina > 0 ;
	}

	//挖掘动作
	public void PlayerDig(){
		sceneGen.SendMessage("DigInMap",characterList);
	}
	public void StopDig(){
		sceneGen.SendMessage("StopDigInMap");
	}

	//探测动作
	public void PlayerDetect(){
		bool haveGeomancer = false;

		int sumArcheology = 0;

		for(int i=0;i<characterList.Count;i++){
			if(characterList[i].Pro.proname.Equals(StringCollection.GEOMANCER)){
				haveGeomancer = true;
				sumArcheology+=characterList[i].archeology;
			}
		}

		if(!haveGeomancer){
			Debug.Log(StringCollection.NOGEO);
		}else{
			//根据总智力属性,消耗一定的挖掘探测工具,给出信息（信息准确度由考古属性，当前挖掘层数决定）
			//消耗探测工具的逻辑未实现(未实现道具 铁椎)
			int detectLevel = sumArcheology - 3 * gData.currentFloor;
			//测试 45级别的探索  非常高
			sceneGen.SendMessage("getDetectorResult",45);
		}
	}

}
