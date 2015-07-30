using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAction : MonoBehaviour
{

	private GlobalData gData;
	private SceneGen sceneGen;
	private int stepCounter;
	public float moveDistance = 0.5f;
	private UI_Input uiInput;

	public enum MOVEDIRECTION
	{
		LEFT,
		RIGHT,
		UP,
		DOWN
	}
	;

	void Start ()
	{
		stepCounter = 0;
		gData = GameObject.FindGameObjectWithTag ("GlobalData").GetComponent<GlobalData> ();
		sceneGen = GameObject.FindGameObjectWithTag ("GameController").GetComponent<SceneGen> ();
		uiInput = GameObject.FindGameObjectWithTag ("GameController").GetComponent<UI_Input> ();
	}

	void OnCollisionEnter2D (Collision2D coll)
	{
		if (coll.gameObject.tag.Equals ("Enemy")) {
			gData.currentEnemy = coll.gameObject.GetComponent<EnemyAI> ().enemy;
			gData.currentEnemyName = coll.gameObject.name;
			gData.playerPos = transform.position;

			sceneGen.SendMessage ("RecScene");
			DontDestroyOnLoad (gData);
			Application.LoadLevel ("battle");
		}
	}

	//玩家移动
	public void PlayerMove (MOVEDIRECTION dir)
	{

		if (MoveStaminaCheck ()) {
			switch (dir) {
			case MOVEDIRECTION.LEFT:
				transform.Translate (Vector2.left * moveDistance);
				break;
			case MOVEDIRECTION.RIGHT:
				transform.Translate (Vector2.right * moveDistance);
				break;
			case MOVEDIRECTION.UP:
				transform.Translate (Vector2.up * moveDistance);
				break;
			case MOVEDIRECTION.DOWN:
				transform.Translate (Vector2.down * moveDistance);
				break;
			}
			
			stepCounter++;

			//通知UI更新
			uiInput.SendMessage ("UpdateUIInfo");

			//队长移动一次，减少一个体能；成员移动三次，减少一个体能
			gData.characterList [0].Stamina = Mathf.Max (0, gData.characterList [0].Stamina - 1);
			
			if (stepCounter == 3) {
				for (int i=1; i<gData.characterList.Count; i++) {
					gData.characterList [i].Stamina = Mathf.Max (0, gData.characterList [i].Stamina - 1);
				}
				stepCounter = 0;
			}
		} else {
			Debug.Log (StringCollection.LEADERNOSTAMINA);
		}
	}

	public bool MoveStaminaCheck ()
	{
		return gData.characterList [0].Stamina > 0;
	}

	//挖掘动作
	public void PlayerDig ()
	{
		sceneGen.SendMessage ("DigInMap", gData.characterList);
	}

	public void StopDig ()
	{
		sceneGen.SendMessage ("StopDigInMap");
	}

	//探测动作
	public void PlayerDetect ()
	{
		bool haveGeomancer = false;

		int sumArcheology = 0;

		for (int i=0; i<gData.characterList.Count; i++) {
			if (gData.characterList [i].Pro.proname.Equals (StringCollection.GEOMANCER)) {
				haveGeomancer = true;
				sumArcheology += gData.characterList [i].archeology;
			}
		}

		if (!haveGeomancer) {
			Debug.Log (StringCollection.NOGEO);
		} else {
			//根据总智力属性,消耗一定的挖掘探测工具,给出信息（信息准确度由考古属性，当前挖掘层数决定）
			//消耗探测工具的逻辑未实现(未实现道具 铁椎)
			int detectLevel = sumArcheology - 3 * gData.currentFloor;
			//测试 45级别的探索  非常高
			sceneGen.SendMessage ("getDetectorResult", 45);
		}
	}

}
