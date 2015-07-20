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

		//从服务器端获取玩家数据初始化
		characterList = new List<Character> ();

		if (gData.characterList == null || gData.characterList.Count == 0) {

			Character c = new Character (30,100, 50, 0, 0, "zhouhui", false,100,100,ProFactory.getPro("Geomancer","1"),1,0);
			
			HealthItem item = new HealthItem (Item.RangeType.SINGLE, 10, "1", "单体治疗药剂");
			List<Baggrid> bgList = new List<Baggrid> ();
			Baggrid bg = new Baggrid (item, 2);
			bgList.Add (bg);
			c.BgList = bgList;
			characterList.Add (c);

			Character c2 = new Character (40, 100,50, 0, 0, "unity", false,100,100,ProFactory.getPro("Settler","1"),1,0);
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

		int sumIntelligence = 0;

		for(int i=0;i<characterList.Count;i++){
			if(characterList[i].Pro.proname.Equals("风水师")){
				haveGeomancer = true;
			}
			sumIntelligence+=characterList[i].intelligence;
		}

		if(!haveGeomancer){
			Debug.Log("队伍中没有风水师,无法进行探测");
		}else{
			//根据总智力属性,消耗一定的挖掘探测工具,给出信息（信息准确度由智力属性，当前挖掘层数决定）
			int detectLevel = sumIntelligence - 3 * gData.currentFloor;
			sceneGen.SendMessage("getDetectorResult",45);
		}
	}

}
