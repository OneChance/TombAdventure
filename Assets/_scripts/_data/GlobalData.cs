using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using KBEngine;
using UnityEngine.UI;

public class GlobalData : MonoBehaviour
{
	public int currentFloor = 1;
	public Enemy currentEnemy;
	public List<Character> characterList;
	public bool victory = true;
	public string currentEnemyName;
	public Vector3 playerPos; //record the pos of player when battle start
	public Baggrid currentItem;
	public Vector3 preDigPos;
	public bool isPlayer; //ture为玩家联机模式，false为单机佣兵模式
	public bool isShop = false;
	public Tomb currentTomb;
	public Dictionary<int, ServerItemData> siList;
	public List<int> itemShopConten;
	public List<int> assistShopContent;
	public List<int> equipShopContent;
	public Account account;
	public List<Tomb> tombs;
	public int enemyNeedRemove = 0;

	void Start ()
	{

	}

	void OnLevelWasLoaded (int level)
	{
		//更新玩家场景信息
		if (level > 1) {
			account.RecordSceneLevel (level);
		}
	}

	public void OffLineNoti (int playerId)
	{
		for (int i=1; i<characterList.Count; i++) {
			if (characterList [i].playerId == playerId) {
				ShowHint.Hint ("[" + characterList [i].ObjName + "]" + StringCollection.stringDict_CN ["OFFLINE"]);
				characterList [i].onLine = false;
				break;
			}
		}

		//更新UI
		UnityEngine.GameObject gameController = UnityEngine.GameObject.FindGameObjectWithTag ("GameController");
		if (gameController != null && gameController.GetComponent<UI_Bag> () != null) {
			gameController.GetComponent<UI_Bag> ().ShowCharInfo ();
		}
	}

	public void OnLineNoti (int playerId)
	{
		for (int i=1; i<characterList.Count; i++) {
			if (characterList [i].playerId == playerId) {
				ShowHint.Hint ("[" + characterList [i].ObjName + "]" + StringCollection.stringDict_CN ["ONLINE"]);
				characterList [i].onLine = true;
				break;
			}
		}
		
		//更新UI
		UnityEngine.GameObject gameController = UnityEngine.GameObject.FindGameObjectWithTag ("GameController");
		if (gameController != null && gameController.GetComponent<UI_Bag> () != null) {
			gameController.GetComponent<UI_Bag> ().ShowCharInfo ();
		}
	}

	public void LeaderChange (int leaderFlag)
	{
		characterList [0].isLeader = (leaderFlag == 1 ? true : false);
	}

	public void PlayerLeave (int playerId, int type)
	{	
		bool self = true;

		for (int i=1; i<characterList.Count; i++) {
			if (characterList [i].playerId == playerId) {
				ShowHint.Hint ("[" + characterList [i].ObjName + "]" + StringCollection.stringDict_CN ["PLAYERLEAVE" + type]);
				characterList.Remove (characterList [i]);
				self = false;
				break;
			}
		}

		if (self) {
			ShowHint.Hint (StringCollection.stringDict_CN ["PLAYERLEAVE" + type]);

			while (characterList.Count>1) {
				characterList.RemoveAt (1);
			}
		}

		//更新UI
		UnityEngine.GameObject gameController = UnityEngine.GameObject.FindGameObjectWithTag ("GameController");
		if (gameController != null && gameController.GetComponent<UI_Bag> () != null) {
			gameController.GetComponent<UI_Bag> ().ItemUseComplete();
			gameController.GetComponent<UI_Bag> ().ShowCharInfo ();
		}
	}
}
